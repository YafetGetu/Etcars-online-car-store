// Cart.cs
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using carstore; // Ensure this namespace is included
using MySql.Data.MySqlClient;
namespace carstore
{
    // *** ADDED: Define the CartItem class here if it's not in a separate file ***
    // You can also put this in your DatabaseConnection.cs or a dedicated Models.cs
    public class CartItem
    {
        public Car Car { get; set; }
        public int Quantity { get; set; }
    }
    // *** END ADDED CartItem class definition ***


    public partial class Cart : Form // Keep 'partial' if you use a designer file
    {
        // *** MODIFIED: Use a list of CartItem to hold items in the user's cart and their quantities ***
        private List<CartItem> cartItems;
        private User currentUser;
        private Panel mainPanel; // Declare here, initialize in InitializeComponent or code
        private Label lblTotal;  // Declare here, initialize in InitializeComponent or code
        // private Button btnPayment; // Declare here, initialize in InitializeComponent or code

        private decimal totalAmount = 0;

        // Modified constructor to accept the logged-in User object and List<CartItem>
        public Cart(List<CartItem> items, User user) // *** MODIFIED parameter type ***
        {
            // If using a designer, InitializeComponent will create designer-added controls
            InitializeComponent();

            this.cartItems = items ?? new List<CartItem>(); // Handle potential null input
            this.currentUser = user; // Assign the passed-in user

            // If mainPanel, lblTotal, etc. are NOT in the designer,
            // you should initialize them here or in InitializeCartForm.
            // Based on your code, they seem to be created programmatically in InitializeCartForm,
            // but let's ensure InitializeCartForm is always called after InitializeComponent.
            InitializeCartForm();

            // After InitializeCartForm has created mainPanel, lblTotal, etc.,
            // their member variables should be non-null.
            // We can now safely call display and layout adjustments.

            // Calculate total amount initially - this is now done in RefreshCartDisplay
            // totalAmount = this.cartItems.Sum(item => item.Price); // REMOVED

            // Display cart items and adjust layout
            RefreshCartDisplay(); // This will call DisplayCartItems and AdjustTotalAndButtonPosition
        }

        private void InitializeCartForm()
        {
            // Form setup
            this.Text = "Shopping Cart";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(0, 40, 85);

            // Check if mainPanel is null (if not added by designer) and create it
            if (mainPanel == null)
            {
                mainPanel = new Panel();
                mainPanel.Dock = DockStyle.Fill;
                mainPanel.AutoScroll = true;
                mainPanel.BackColor = Color.Transparent;
                mainPanel.Padding = new Padding(20); // Add padding to the main panel
                this.Controls.Add(mainPanel);
            }


            // Title
            // Check if title label already exists (if added by designer) or create it
            Control[] foundTitle = mainPanel.Controls.Find("lblTitle", false); // Use a name to find it
            Label lblTitle = null;
            if (foundTitle.Length > 0 && foundTitle[0] is Label existingTitle)
            {
                lblTitle = existingTitle;
            }
            else
            {
                lblTitle = new Label
                {
                    Name = "lblTitle", // Give it a name for finding later
                    Text = "Your Shopping Cart",
                    Font = new Font("Segoe UI", 20, FontStyle.Bold),
                    ForeColor = Color.White,
                    AutoSize = true,
                    Location = new Point(mainPanel.Padding.Left, mainPanel.Padding.Top) // Position based on panel padding
                };
                mainPanel.Controls.Add(lblTitle);
            }

            // Check if lblTotal is null (if not added by designer) and create it
            if (lblTotal == null)
            {
                lblTotal = new Label
                {
                    Name = "lblTotal", // Give it a name for finding later
                    Font = new Font("Segoe UI", 16, FontStyle.Bold),
                    ForeColor = Color.LimeGreen, // Initial color
                    AutoSize = true,
                    // Initial position will be adjusted later
                    Location = new Point(mainPanel.Padding.Left, 0)
                };
                mainPanel.Controls.Add(lblTotal);
            }


            // Check if btnPayment is null (if not added by designer) and create it
            Control[] foundPaymentButton = mainPanel.Controls.Find("btnPayment", false); // Find by name
            Button btnPayment = null; // Use local variable initially
            if (foundPaymentButton.Length > 0 && foundPaymentButton[0] is Button existingPaymentButton)
            {
                btnPayment = existingPaymentButton;
            }
            else
            {
                btnPayment = new Button
                {
                    Name = "btnPayment", // Give it a name for finding later
                    Text = "PROCEED TO PAYMENT",
                    Size = new Size(250, 50),
                    // Initial position will be adjusted later
                    Location = new Point(0, 0),
                    BackColor = Color.FromArgb(0, 50, 0),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat
                };
                // Add black border to the button
                btnPayment.FlatAppearance.BorderSize = 2;
                btnPayment.FlatAppearance.BorderColor = Color.Black;

                // Hover effect
                btnPayment.MouseEnter += (s, args) => { btnPayment.BackColor = Color.FromArgb(0, 70, 0); }; // Darker green on hover
                btnPayment.MouseLeave += (s, args) => { btnPayment.BackColor = Color.FromArgb(0, 50, 0); }; // Original green

                btnPayment.Click += BtnPayment_Click;
                mainPanel.Controls.Add(btnPayment);
            }

            // Note: We don't need the class-level btnPayment variable anymore if we find it by name or ensure it's added.
            // If you prefer using the member variable, initialize it here like mainPanel and lblTotal.
            // Example for btnPayment member variable:
            // if (this.btnPayment == null) { this.btnPayment = new Button { /* properties */ }; this.btnPayment.Click += BtnPayment_Click; mainPanel.Controls.Add(this.btnPayment); }
            // And then use this.btnPayment in AdjustTotalAndButtonPosition and BtnPayment_Click.
            // For now, sticking closer to your original code's use of finding by name for the button.

            // Attach resize handler to mainPanel to adjust control positions when it resizes
            mainPanel.Resize += (s, e) => AdjustTotalAndButtonPosition();

            // The initial display and positioning will be handled by RefreshCartDisplay
        }


        private void DisplayCartItems()
        {
            // Use the bottom of the title label as the starting point for the first item
            Control[] foundTitle = mainPanel.Controls.Find("lblTitle", false);
            int yOffset = (foundTitle.Length > 0 && foundTitle[0] is Label lblTitle) ? lblTitle.Bottom + 20 : mainPanel.Padding.Top + 50; // Start below title or with padding

            // Clear existing item panels and the empty message if present
            // Find controls that are NOT the title, total, or payment button
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
                control.Dispose(); // Release resources
            }


            if (cartItems.Count > 0)
            {
                foreach (CartItem cartItem in cartItems) // *** MODIFIED: Iterate through CartItem ***
                {
                    // Create panel for each cart item
                    Panel itemPanel = new Panel
                    {
                        Name = "cartItemPanel" + cartItem.Car.CarID, // Use CarID for unique name
                        // Adjust width dynamically based on mainPanel's client size
                        Size = new Size(mainPanel.ClientSize.Width - mainPanel.Padding.Left - mainPanel.Padding.Right - (mainPanel.AutoScroll ? SystemInformation.VerticalScrollBarWidth : 0), 120), // Account for scrollbar width
                        Location = new Point(mainPanel.Padding.Left, yOffset), // Position with padding
                        BackColor = Color.FromArgb(100, 60, 60, 80),
                        BorderStyle = BorderStyle.FixedSingle // Optional: Add border
                    };

                    // Car image
                    PictureBox picCar = new PictureBox
                    {
                        Size = new Size(100, 80),
                        Location = new Point(10, 20),
                        SizeMode = PictureBoxSizeMode.Zoom,
                        BackColor = Color.Transparent
                    };

                    if (!string.IsNullOrEmpty(cartItem.Car.ImageBase64)) // *** MODIFIED: Access Car object from CartItem ***
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
                            picCar.Image = null; // Handle invalid Base64
                        }
                    }

                    // Car details
                    Label lblCarName = new Label
                    {
                        Text = $"{cartItem.Car.Brand} {cartItem.Car.Model} {cartItem.Car.Year}", // *** MODIFIED: Access Car object from CartItem ***
                        Font = new Font("Segoe UI", 12, FontStyle.Bold),
                        ForeColor = Color.White,
                        AutoSize = true,
                        Location = new Point(120, 20)
                    };

                    // *** MODIFIED: Display Quantity and Total Price for the item ***
                    Label lblQuantity = new Label
                    {
                        Text = $"Quantity: {cartItem.Quantity}",
                        Font = new Font("Segoe UI", 10, FontStyle.Regular), // Regular font for quantity
                        ForeColor = Color.White,
                        AutoSize = true,
                        Location = new Point(120, lblCarName.Bottom + 5) // Position below car name
                    };

                    Label lblItemTotal = new Label
                    {
                        Text = $"Subtotal: ${cartItem.Car.Price * cartItem.Quantity:#,##0.00} ETB", // Calculate subtotal
                        Font = new Font("Segoe UI", 12, FontStyle.Bold), // Bold for price
                        ForeColor = Color.LimeGreen,
                        AutoSize = true,
                        Location = new Point(120, lblQuantity.Bottom + 5) // Position below quantity
                    };


                    // Remove button (optional, if you want to allow removing items)
                    // ... (Add button similar to Payment button, with a click handler)
                    Button btnRemove = new Button
                    {
                        Text = "Remove",
                        Size = new Size(80, 30),
                        // Position relative to itemPanel's client size
                        Location = new Point(itemPanel.ClientSize.Width - 90, 45),
                        BackColor = Color.FromArgb(200, 50, 50),
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat
                    };
                    btnRemove.FlatAppearance.BorderSize = 0;
                    // Store the ENTIRE CartItem object in the Tag for easy access in the event handler
                    btnRemove.Tag = cartItem; // *** MODIFIED: Store CartItem object ***
                    btnRemove.Click += BtnRemove_Click;


                    // Add controls to item panel
                    itemPanel.Controls.Add(picCar);
                    itemPanel.Controls.Add(lblCarName);
                    itemPanel.Controls.Add(lblQuantity); // Add quantity label
                    itemPanel.Controls.Add(lblItemTotal); // Add item total label
                    itemPanel.Controls.Add(btnRemove);

                    // Add item panel to main panel
                    mainPanel.Controls.Add(itemPanel);

                    // Increment Y offset for next item
                    yOffset += itemPanel.Height + 10; // Space for the next item panel (Panel Height + Spacing)
                }
                mainPanel.AutoScroll = true; // Enable scrolling if items exceed panel height
            }
            else
            {
                // Display a message if cart is empty
                Label lblEmpty = new Label
                {
                    Text = "Your cart is empty",
                    Font = new Font("Segoe UI", 14, FontStyle.Bold),
                    ForeColor = Color.White,
                    AutoSize = true,
                    Name = "lblEmpty", // Give it a name to potentially exclude it later if needed
                    Location = new Point(mainPanel.Padding.Left, yOffset + 20) // Position below the title/previous items
                };
                mainPanel.Controls.Add(lblEmpty);
                yOffset += lblEmpty.Height + 20; // Adjust yOffset after adding the label
                mainPanel.AutoScroll = false; // Disable scrolling
            }

            // The positioning of total and button is now handled by AdjustTotalAndButtonPosition
        }


        // *** ADDED: Helper method to refresh the entire cart display ***
        private void RefreshCartDisplay()
        {
            // Recalculate total amount before re-displaying
            CalculateTotalAmount();

            // Re-display the cart items and the empty message if necessary
            DisplayCartItems(); // This method now only adds the item panels or the empty message

            // Adjust the positions of the total label and payment button
            AdjustTotalAndButtonPosition();

            // Ensure the panel updates visually
            // Using SuspendLayout/ResumeLayout can improve performance for multiple control changes
            mainPanel.SuspendLayout();
            mainPanel.ResumeLayout();
            mainPanel.Invalidate();
            mainPanel.Refresh();
        }


        // *** ADDED: Method to calculate the total amount ***
        private void CalculateTotalAmount()
        {
            totalAmount = 0;
            if (cartItems != null)
            {
                // *** MODIFIED: Calculate total based on price * quantity ***
                totalAmount = cartItems.Sum(item => item.Car.Price * item.Quantity);
            }
            // Update the total label text
            if (lblTotal != null) // Add null check
            {
                lblTotal.Text = $"Total: ${totalAmount:#,##0.00} ETB";
                // Update the color based on total amount (e.g., Red if 0, Green otherwise)
                lblTotal.ForeColor = totalAmount > 0 ? Color.LimeGreen : Color.White;
            }
        }


        private void AdjustTotalAndButtonPosition()
        {
            // Add null checks for mainPanel and lblTotal
            if (mainPanel == null || lblTotal == null) return;

            // Find the last dynamically added control (either the last item panel or the "empty" label)
            // Exclude the title label, total label, and payment button
            Control[] specialControls = mainPanel.Controls.Find("lblTitle", false)
                                      .Concat(mainPanel.Controls.Find("lblTotal", false))
                                      .Concat(mainPanel.Controls.Find("btnPayment", false))
                                      .ToArray();

            Control lastDynamicControl = mainPanel.Controls.Cast<Control>()
                                             .Where(c => !specialControls.Contains(c)) // Exclude special controls
                                             .OrderByDescending(c => c.Bottom)
                                             .FirstOrDefault();

            // Calculate the y-position below the last dynamic control, or below the title if no dynamic controls
            // Use the bottom of the title label as a fallback if no dynamic controls are found
            Control[] foundTitle = mainPanel.Controls.Find("lblTitle", false);
            int titleBottom = (foundTitle.Length > 0 && foundTitle[0] is Label lblTitle) ? lblTitle.Bottom : mainPanel.Padding.Top;

            int yPosition = (lastDynamicControl != null) ? lastDynamicControl.Bottom + 20 : titleBottom + 50; // 20 pixels spacing below the last item/empty message


            // Update Total label position
            lblTotal.Location = new Point(mainPanel.Padding.Left, yPosition);
            lblTotal.BringToFront(); // Ensure it's on top


            // Update Payment button position (find by name)
            Control[] foundPaymentButton = mainPanel.Controls.Find("btnPayment", false);
            if (foundPaymentButton.Length > 0 && foundPaymentButton[0] is Button currentPaymentButton)
            {
                // Adjust position considering mainPanel client size and potential scrollbar
                int buttonXPosition = mainPanel.ClientSize.Width - mainPanel.Padding.Right - currentPaymentButton.Width - (mainPanel.AutoScroll ? SystemInformation.VerticalScrollBarWidth : 0);
                currentPaymentButton.Location = new Point(buttonXPosition, yPosition); // Align right
                currentPaymentButton.BringToFront(); // Ensure it's on top

                // Ensure payment button is enabled only if cart is not empty and user is logged in
                currentPaymentButton.Enabled = cartItems.Count > 0 && currentUser != null;
            }

            // Ensure the scrollable area accommodates the new position of the total and button
            int requiredHeight = yPosition + Math.Max(lblTotal.Height, (foundPaymentButton.Length > 0 ? foundPaymentButton[0].Height : 0)) + mainPanel.Padding.Bottom;
            if (mainPanel.VerticalScroll.Visible) // Consider scrollbar only if visible
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
            // *** MODIFIED: Tag now holds the CartItem object ***
            if (btnRemove != null && btnRemove.Tag is CartItem cartItemToRemove)
            {
                DialogResult result = MessageBox.Show(
                    $"Are you sure you want to remove one {cartItemToRemove.Car.Brand} {cartItemToRemove.Car.Model} from your cart?",
                    "Confirm Removal",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    cartItemToRemove.Quantity--; // Decrease quantity

                    if (cartItemToRemove.Quantity <= 0)
                    {
                        // If quantity is 0 or less, remove the item completely
                        cartItems.Remove(cartItemToRemove);
                        MessageBox.Show($"{cartItemToRemove.Car.Brand} {cartItemToRemove.Car.Model} removed from cart.", "Item Removed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // Otherwise, just update the display to show the new quantity
                        MessageBox.Show($"{cartItemToRemove.Car.Brand} {cartItemToRemove.Car.Model} quantity decreased. New quantity: {cartItemToRemove.Quantity}", "Cart Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    // Recalculate total amount and refresh the UI
                    RefreshCartDisplay();
                }
            }
        }


        private void BtnPayment_Click(object sender, EventArgs e) // Corrected EventArgs type
        {
            if (cartItems.Count == 0)
            {
                MessageBox.Show("Your cart is empty!", "Empty Cart", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check if user is logged in before proceeding with payment/order creation
            if (currentUser == null)
            {
                MessageBox.Show("You must be logged in to complete the purchase.", "Login Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            // Show confirmation message
            DialogResult confirmResult = MessageBox.Show(
                $"Total amount to pay: ${totalAmount:#,##0.00} ETB\n\nDo you want to proceed to payment?",
                "Confirm Payment",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                // Open payment form
                payment payForm = new payment(); // Ensure your payment form is named 'payment'
                DialogResult paymentResult = payForm.ShowDialog();

                if (paymentResult == DialogResult.OK)
                {
                    // Payment successful (simulated) - create orders for each item quantity
                    bool allOrdersCreated = true;
                    // We don't need to track successfully ordered cars here if we clear the cart completely on success

                    // *** MODIFIED: Loop through cart items and create an order for each car quantity ***
                    foreach (var cartItem in cartItems)
                    {
                        // For each quantity of the car, create a separate order entry
                        for (int i = 0; i < cartItem.Quantity; i++)
                        {
                            // Use DatabaseConnection.CreateOrder method from your provided code
                            // Pass currentUser.UserID, cartItem.Car.CarID, and cartItem.Car.Price for a single item order
                            if (!DatabaseConnection.CreateOrder(currentUser.UserID, cartItem.Car.CarID, cartItem.Car.Price)) // Use the price of a single car for each order entry
                            {
                                allOrdersCreated = false; // Mark as failed if any order creation fails
                                // Optionally log which specific order failed
                                Debug.WriteLine($"Failed to create order for CarID: {cartItem.Car.CarID} (Instance {i + 1} of {cartItem.Quantity})");
                                // Decide how to handle partial failures: stop, or continue and report later?
                                // For simplicity, we'll continue and report overall success/failure.
                            }
                            else
                            {
                                // If you want to mark the car as unavailable (set IsAvailable = 0) in the database,
                                // you would do it here. However, based on your previous request to REMOVE the MarkCarAsUnavailable call,
                                // I will not add it here. If you want to mark as unavailable for EACH ordered quantity,
                                // you would need logic to prevent marking it unavailable more than once per car.
                                // DatabaseConnection.MarkCarAsUnavailable(cartItem.Car.CarID); // Add this back if you want cars marked unavailable on purchase
                            }
                        }
                    }


                    if (allOrdersCreated)
                    {
                        MessageBox.Show("Payment successful! Your orders have been confirmed.",
                            "Orders Placed", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Clear the cart in the Form1 instance by signaling success
                        this.DialogResult = DialogResult.OK; // Signal success to parent form (Form1) to clear cart
                        this.Close();
                    }
                    else
                    {
                        // If some orders failed, inform the user and keep the cart open with items that failed
                        MessageBox.Show("Payment was successful, but there was an error creating some order records.\nThe remaining items are still in your cart. Please contact support.",
                            "Order Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        // The cartItems list in Form1 will NOT be cleared because DialogResult is not OK.
                        // The current cartItems list in THIS Cart form instance reflects the items that were attempted to be ordered.
                        // If you wanted to remove successfully ordered items from THIS list on partial failure,
                        // you would need to track them during the loop. For now, I'm assuming if allOrdersCreated is false,
                        // the user needs to see the full list they attempted to order.

                        // Do NOT set DialogResult to OK, keep the cart open
                        this.DialogResult = DialogResult.None; // Explicitly set to None
                    }
                }
                else
                {
                    MessageBox.Show("Payment process was cancelled.",
                        "Payment Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.Cancel; // Signal cancellation
                }
            }
            else
            {
                // User cancelled the payment confirmation message
                MessageBox.Show("Order process cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.Cancel; // Signal cancellation
            }
        }

        // Optional: Add a handler for when the form is resized to reposition controls
        private void Cart_Resize(object sender, EventArgs e)
        {
            // Re-display items to adjust their width and reposition total/button
            // Ensure DisplayCartItems handles control widths based on parent ClientSize
            AdjustTotalAndButtonPosition(); // Only need to adjust positioning on resize if items' width is fixed
                                            // If item width is dynamic based on mainPanel.ClientSize, call RefreshCartDisplay
            RefreshCartDisplay(); // Call RefreshCartDisplay to redraw items with new width and adjust layout
        }

        // Add the Load handler if you need to do anything when the form loads
        private void Cart_Load(object sender, EventArgs e)
        {
            // Initial setup is done in the constructor, but you could add things here if needed
            // For example, ensure the initial layout is correct after the form is fully loaded
            AdjustTotalAndButtonPosition();
        }


        // If you are using a designer, the Dispose method will be in Cart.Designer.cs.
        // If not using a designer, you might need a Dispose method here to clean up resources.
        // protected override void Dispose(bool disposing) { ... }
    }
}