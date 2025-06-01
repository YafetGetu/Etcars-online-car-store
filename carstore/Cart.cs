using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using carstore;

namespace carstore
{
    
    public class CartItem
    {
        public Car Car { get; set; }
        public int Quantity { get; set; }
    }


    public partial class Cart : Form 
    {
        private List<CartItem> cartItems;
        private User currentUser;
        private Panel mainPanel; 
        private Label lblTotal;  

        private decimal totalAmount = 0;

        public Cart(List<CartItem> items, User user) 
        {
            InitializeComponent();

            this.cartItems = items ?? new List<CartItem>();
            this.currentUser = user; 
            InitializeCartForm();

            
            RefreshCartDisplay(); 
        }

        private void InitializeCartForm()
        {
            this.Text = "Shopping Cart";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(0, 40, 85);

            if (mainPanel == null)
            {
                mainPanel = new Panel();
                mainPanel.Dock = DockStyle.Fill;
                mainPanel.AutoScroll = true;
                mainPanel.BackColor = Color.Transparent;
                mainPanel.Padding = new Padding(20); 
                this.Controls.Add(mainPanel);
            }


            Control[] foundTitle = mainPanel.Controls.Find("lblTitle", false); 
            Label lblTitle = null;
            if (foundTitle.Length > 0 && foundTitle[0] is Label existingTitle)
            {
                lblTitle = existingTitle;
            }
            else
            {
                lblTitle = new Label
                {
                    Name = "lblTitle", 
                    Text = "Your Shopping Cart",
                    Font = new Font("Segoe UI", 20, FontStyle.Bold),
                    ForeColor = Color.White,
                    AutoSize = true,
                    Location = new Point(mainPanel.Padding.Left, mainPanel.Padding.Top) 
                };
                mainPanel.Controls.Add(lblTitle);
            }

            if (lblTotal == null)
            {
                lblTotal = new Label
                {
                    Name = "lblTotal", 
                    Font = new Font("Segoe UI", 16, FontStyle.Bold),
                    ForeColor = Color.LimeGreen, 
                    AutoSize = true,
                    Location = new Point(mainPanel.Padding.Left, 0)
                };
                mainPanel.Controls.Add(lblTotal);
            }


            Control[] foundPaymentButton = mainPanel.Controls.Find("btnPayment", false); 
            Button btnPayment = null; 
            if (foundPaymentButton.Length > 0 && foundPaymentButton[0] is Button existingPaymentButton)
            {
                btnPayment = existingPaymentButton;
            }
            else
            {
                btnPayment = new Button
                {
                    Name = "btnPayment", 
                    Text = "PROCEED TO PAYMENT",
                    Size = new Size(250, 50),
                    Location = new Point(0, 0),
                    BackColor = Color.FromArgb(0, 50, 0),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat
                };
                btnPayment.FlatAppearance.BorderSize = 2;
                btnPayment.FlatAppearance.BorderColor = Color.Black;

                // Hover effect
                btnPayment.MouseEnter += (s, args) => { btnPayment.BackColor = Color.FromArgb(0, 70, 0); }; 
                btnPayment.MouseLeave += (s, args) => { btnPayment.BackColor = Color.FromArgb(0, 50, 0); }; 

                btnPayment.Click += BtnPayment_Click;
                mainPanel.Controls.Add(btnPayment);
            }

            mainPanel.Resize += (s, e) => AdjustTotalAndButtonPosition();

        }


        private void DisplayCartItems()
        {
            Control[] foundTitle = mainPanel.Controls.Find("lblTitle", false);
            int yOffset = (foundTitle.Length > 0 && foundTitle[0] is Label lblTitle) ? lblTitle.Bottom + 20 : mainPanel.Padding.Top + 50; // Start below title or with padding

            List<Control> itemsAndEmptyToRemove = new List<Control>();
            Control[] specialControls = mainPanel.Controls.Find("lblTitle", false)
                                       .Concat(mainPanel.Controls.Find("lblTotal", false))
                                       .Concat(mainPanel.Controls.Find("btnPayment", false))
                                       .ToArray();

            foreach (Control control in mainPanel.Controls)
            {
                if (!specialControls.Contains(control))
                {
                    itemsAndEmptyToRemove.Add(control);
                }
            }

            foreach (Control control in itemsAndEmptyToRemove)
            {
                mainPanel.Controls.Remove(control);
                control.Dispose(); 
            }


            if (cartItems.Count > 0)
            {
                foreach (CartItem cartItem in cartItems) 
                {
                    Panel itemPanel = new Panel
                    {
                        Name = "cartItemPanel" + cartItem.Car.CarID, 
                        Size = new Size(mainPanel.ClientSize.Width - mainPanel.Padding.Left - mainPanel.Padding.Right - (mainPanel.AutoScroll ? SystemInformation.VerticalScrollBarWidth : 0), 120), // Account for scrollbar width
                        Location = new Point(mainPanel.Padding.Left, yOffset), 
                        BackColor = Color.FromArgb(100, 60, 60, 80),
                        BorderStyle = BorderStyle.FixedSingle 
                    };

                    PictureBox picCar = new PictureBox
                    {
                        Size = new Size(100, 80),
                        Location = new Point(10, 20),
                        SizeMode = PictureBoxSizeMode.Zoom,
                        BackColor = Color.Transparent
                    };

                    if (!string.IsNullOrEmpty(cartItem.Car.ImageBase64)) 
                    {
                        try
                        {
                            byte[] imageBytes = Convert.FromBase64String(cartItem.Car.ImageBase64);
                            using (var ms = new System.IO.MemoryStream(imageBytes))
                            {
                                picCar.Image = Image.FromStream(ms);
                            }
                        }
                        catch
                        {
                            picCar.Image = null; 
                        }
                    }

                    Label lblCarName = new Label
                    {
                        Text = $"{cartItem.Car.Brand} {cartItem.Car.Model} {cartItem.Car.Year}",
                        Font = new Font("Segoe UI", 12, FontStyle.Bold),
                        ForeColor = Color.White,
                        AutoSize = true,
                        Location = new Point(120, 20)
                    };

                    Label lblQuantity = new Label
                    {
                        Text = $"Quantity: {cartItem.Quantity}",
                        Font = new Font("Segoe UI", 10, FontStyle.Regular),
                        ForeColor = Color.White,
                        AutoSize = true,
                        Location = new Point(120, lblCarName.Bottom + 5) 
                    };

                    Label lblItemTotal = new Label
                    {
                        Text = $"Subtotal: ${cartItem.Car.Price * cartItem.Quantity:#,##0.00} ETB", 
                        Font = new Font("Segoe UI", 12, FontStyle.Bold),
                        ForeColor = Color.LimeGreen,
                        AutoSize = true,
                        Location = new Point(120, lblQuantity.Bottom + 5) 
                    };

                    Button btnRemove = new Button
                    {
                        Text = "Remove",
                        Size = new Size(80, 30),
                        Location = new Point(itemPanel.ClientSize.Width - 90, 45),
                        BackColor = Color.FromArgb(200, 50, 50),
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat
                    };
                    btnRemove.FlatAppearance.BorderSize = 0;
                    btnRemove.Tag = cartItem; 
                    btnRemove.Click += BtnRemove_Click;


                    itemPanel.Controls.Add(picCar);
                    itemPanel.Controls.Add(lblCarName);
                    itemPanel.Controls.Add(lblQuantity); 
                    itemPanel.Controls.Add(lblItemTotal); 
                    itemPanel.Controls.Add(btnRemove);

                    mainPanel.Controls.Add(itemPanel);

                    yOffset += itemPanel.Height + 10; 
                }
                mainPanel.AutoScroll = true; 
            }
            else
            {
                Label lblEmpty = new Label
                {
                    Text = "Your cart is empty",
                    Font = new Font("Segoe UI", 14, FontStyle.Bold),
                    ForeColor = Color.White,
                    AutoSize = true,
                    Name = "lblEmpty",
                    Location = new Point(mainPanel.Padding.Left, yOffset + 20) 
                };
                mainPanel.Controls.Add(lblEmpty);
                yOffset += lblEmpty.Height + 20; 
                mainPanel.AutoScroll = false; 
            }

        }


        private void RefreshCartDisplay()
        {
            CalculateTotalAmount();

            DisplayCartItems(); 

            AdjustTotalAndButtonPosition();

            mainPanel.SuspendLayout();
            mainPanel.ResumeLayout();
            mainPanel.Invalidate();
            mainPanel.Refresh();
        }


        private void CalculateTotalAmount()
        {
            totalAmount = 0;
            if (cartItems != null)
            {
                totalAmount = cartItems.Sum(item => item.Car.Price * item.Quantity);
            }
            if (lblTotal != null) 
            {
                lblTotal.Text = $"Total: ${totalAmount:#,##0.00} ETB";
                lblTotal.ForeColor = totalAmount > 0 ? Color.LimeGreen : Color.White;
            }
        }


        private void AdjustTotalAndButtonPosition()
        {
            if (mainPanel == null || lblTotal == null) return;

            Control[] specialControls = mainPanel.Controls.Find("lblTitle", false)
                                      .Concat(mainPanel.Controls.Find("lblTotal", false))
                                      .Concat(mainPanel.Controls.Find("btnPayment", false))
                                      .ToArray();

            Control lastDynamicControl = mainPanel.Controls.Cast<Control>()
                                             .Where(c => !specialControls.Contains(c)) 
                                             .OrderByDescending(c => c.Bottom)
                                             .FirstOrDefault();

            
            Control[] foundTitle = mainPanel.Controls.Find("lblTitle", false);
            int titleBottom = (foundTitle.Length > 0 && foundTitle[0] is Label lblTitle) ? lblTitle.Bottom : mainPanel.Padding.Top;

            int yPosition = (lastDynamicControl != null) ? lastDynamicControl.Bottom + 20 : titleBottom + 50; 


            lblTotal.Location = new Point(mainPanel.Padding.Left, yPosition);
            lblTotal.BringToFront(); 


            Control[] foundPaymentButton = mainPanel.Controls.Find("btnPayment", false);
            if (foundPaymentButton.Length > 0 && foundPaymentButton[0] is Button currentPaymentButton)
            {
                int buttonXPosition = mainPanel.ClientSize.Width - mainPanel.Padding.Right - currentPaymentButton.Width - (mainPanel.AutoScroll ? SystemInformation.VerticalScrollBarWidth : 0);
                currentPaymentButton.Location = new Point(buttonXPosition, yPosition); 
                currentPaymentButton.BringToFront();

                currentPaymentButton.Enabled = cartItems.Count > 0 && currentUser != null;
            }

            int requiredHeight = yPosition + Math.Max(lblTotal.Height, (foundPaymentButton.Length > 0 ? foundPaymentButton[0].Height : 0)) + mainPanel.Padding.Bottom;
            if (mainPanel.VerticalScroll.Visible) 
            {
                requiredHeight += SystemInformation.VerticalScrollBarWidth;
            }

            if (mainPanel.AutoScrollMinSize.Height < requiredHeight)
            {
                mainPanel.AutoScrollMinSize = new Size(0, requiredHeight);
            }
        }


        private void BtnRemove_Click(object sender, EventArgs e)
        {
            Button btnRemove = sender as Button;
            if (btnRemove != null && btnRemove.Tag is CartItem cartItemToRemove)
            {
                DialogResult result = MessageBox.Show(
                    $"Are you sure you want to remove one {cartItemToRemove.Car.Brand} {cartItemToRemove.Car.Model} from your cart?",
                    "Confirm Removal",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    cartItemToRemove.Quantity--; 

                    if (cartItemToRemove.Quantity <= 0)
                    {
                        cartItems.Remove(cartItemToRemove);
                        MessageBox.Show($"{cartItemToRemove.Car.Brand} {cartItemToRemove.Car.Model} removed from cart.", "Item Removed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"{cartItemToRemove.Car.Brand} {cartItemToRemove.Car.Model} quantity decreased. New quantity: {cartItemToRemove.Quantity}", "Cart Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    RefreshCartDisplay();
                }
            }
        }


        private void BtnPayment_Click(object sender, EventArgs e) 
        {
            if (cartItems.Count == 0)
            {
                MessageBox.Show("Your cart is empty!", "Empty Cart", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (currentUser == null)
            {
                MessageBox.Show("You must be logged in to complete the purchase.", "Login Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            DialogResult confirmResult = MessageBox.Show(
                $"Total amount to pay: ${totalAmount:#,##0.00} ETB\n\nDo you want to proceed to payment?",
                "Confirm Payment",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                payment payForm = new payment(); 
                DialogResult paymentResult = payForm.ShowDialog();

                if (paymentResult == DialogResult.OK)
                {
                    bool allOrdersCreated = true;
                    foreach (var cartItem in cartItems)
                    {
                        for (int i = 0; i < cartItem.Quantity; i++)
                        {
                            if (!DatabaseConnection.CreateOrder(currentUser.UserID, cartItem.Car.CarID, cartItem.Car.Price)) 
                            {
                                allOrdersCreated = false; 
                                Debug.WriteLine($"Failed to create order for CarID: {cartItem.Car.CarID} (Instance {i + 1} of {cartItem.Quantity})");
                                
                            }
                            else
                            {
                               
                                // DatabaseConnection.MarkCarAsUnavailable(cartItem.Car.CarID); 
                            }
                        }
                    }


                    if (allOrdersCreated)
                    {
                        MessageBox.Show("Payment successful! Your orders have been confirmed.",
                            "Orders Placed", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.DialogResult = DialogResult.OK; 
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Payment was successful, but there was an error creating some order records.\nThe remaining items are still in your cart. Please contact support.",
                            "Order Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.DialogResult = DialogResult.None; 
                    }
                }
                else
                {
                    MessageBox.Show("Payment process was cancelled.",
                        "Payment Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.Cancel; 
                }
            }
            else
            {
                MessageBox.Show("Order process cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.Cancel; 
            }
        }

        private void Cart_Resize(object sender, EventArgs e)
        {
            
            AdjustTotalAndButtonPosition(); 
            RefreshCartDisplay(); 
        }

        private void Cart_Load(object sender, EventArgs e)
        {
            AdjustTotalAndButtonPosition();
        }

    }
}