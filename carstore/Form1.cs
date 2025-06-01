using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq; 
using System.Runtime.InteropServices;
using System.Windows.Forms;
using carstore;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;

namespace carstore
{
    public partial class Form1 : Form
    {
        private List<Car> carList;
        private int currentCarIndex = 0;
        private User currentUser = null;
        private Button btnLoginSignup = null;

        private List<CartItem> cartItems = new List<CartItem>();
        private Button btnAddToCart = null; 

        public Form1()
        {
            InitializeComponent();

            SetupLeftMenu();
            SetupCarInfoPanel();
            SetupNavigationArrows();
            SetupAddToCartButton(); 

            carList = DatabaseConnection.GetAllCars();

            AdjustPanelLayout();

            if (carList != null && carList.Count > 0)
            {
                DisplayCar(currentCarIndex);
            }
            else
            {
                MessageBox.Show("No cars found in the database or error loading cars.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string imgPath = Application.StartupPath + @"\asset\bg\bg1.jpg";
            if (File.Exists(imgPath))
            {
                try
                {
                    this.BackgroundImage = Image.FromFile(imgPath);
                    this.BackgroundImageLayout = ImageLayout.Stretch;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error loading background image: " + ex.Message);
                }
            }
            else
            {
                Debug.WriteLine("Background image not found: " + imgPath);
            }

            AdjustPanelLayout();

            if (carList != null && carList.Count > 0)
            {
                LoadAndDisplayCarListPanels();
            }
        }

        private void AdjustPanelLayout()
        {
            if (panel1 == null || panel2 == null || panel10 == null || pictureBox1 == null || panel11 == null || panel12 == null || panel13 == null)
            {
                Debug.WriteLine("AdjustPanelLayout: One or more panels are null.");
                return; 
            }

            panel2.Size = new Size(400, this.ClientSize.Height - 120);
            panel2.Location = new Point(this.ClientSize.Width - panel2.Width - 20, 60);

            int spaceBetweenPanels = this.ClientSize.Width - panel1.Width - panel2.Width;
            panel10.Location = new Point(panel1.Width + (spaceBetweenPanels - panel10.Width) / 2, 12); 

            int availableWidth = this.ClientSize.Width - panel1.Width - panel2.Width;
            int availableHeight = this.ClientSize.Height;
            int baseImageWidth = 1000;
            int baseImageHeight = 550;
            int adjustedWidth = (int)(availableWidth * 0.8); 
            int adjustedHeight = (int)(baseImageHeight * (adjustedWidth / (float)baseImageWidth));
            pictureBox1.Size = new Size(adjustedWidth, adjustedHeight);

            int offsetX = 0;
            int offsetY = 50; 

            pictureBox1.Left = panel1.Width + (availableWidth - pictureBox1.Width) / 2 + offsetX;
            pictureBox1.Top = (availableHeight - pictureBox1.Height) / 2 + offsetY; 


            panel11.Size = new Size(135, 125); 
            panel12.Size = new Size(131, 125); 

            panel11.Location = new Point(pictureBox1.Right + 10, pictureBox1.Top + (pictureBox1.Height - panel11.Height) / 2);
            panel12.Location = new Point(pictureBox1.Left - panel12.Width - 10, pictureBox1.Top + (pictureBox1.Height - panel12.Height) / 2);


            panel13.Size = new Size(302, 60);
            panel13.Location = new Point(panel1.Width + (availableWidth - panel13.Width) / 2, pictureBox1.Bottom + 20);


            this.Invalidate();
            this.Refresh();
        }

        private void SetupLeftMenu()
        {
            if (panel1 == null)
            {
                Debug.WriteLine("SetupLeftMenu: panel1 is null.");
                return;
            }

            panel1.Controls.Clear();
            panel1.BackColor = Color.FromArgb(100, 60, 60, 80);

            PictureBox logo = new PictureBox();
            logo.Name = "logo";
            string logoPath = Application.StartupPath + @"\asset\bg\logo.jpg";

            if (File.Exists(logoPath))
            {
                try
                {
                    using (Image originalLogo = Image.FromFile(logoPath))
                    {
                        Image resizedLogo = new Bitmap(originalLogo, new Size(250, 200));
                        logo.Image = resizedLogo;
                        logo.SizeMode = PictureBoxSizeMode.Zoom;
                        logo.Size = new Size(300, 200);
                        int centerX = (panel1.Width - logo.Width) / 2;
                        logo.Location = new Point(centerX, -25);
                        logo.BackColor = Color.Transparent;
                        panel1.Controls.Add(logo);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error loading logo image: " + ex.Message);
                }
            }

            int buttonYPos = 170; 
            int buttonHeight = 50;
            int buttonSpacing = 10;


            Button btnOrder = CreateMenuButton("btnOrder", "  Orders", @"asset\icon\order.png", new Point(0, buttonYPos));
            btnOrder.MouseEnter += (s, args) => { btnOrder.BackColor = Color.FromArgb(255, 30, 30, 90); };
            btnOrder.MouseLeave += (s, args) => { btnOrder.BackColor = Color.Transparent; };
            btnOrder.Click += (s, e) =>
            {
                if (currentUser == null) 
                {
                    MessageBox.Show("Please login to view your orders.", "Login Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                order orderForm = new order(currentUser); 
                orderForm.ShowDialog();
            };
            panel1.Controls.Add(btnOrder);
            buttonYPos += buttonHeight + buttonSpacing;

            Button btnCart = CreateMenuButton("btnCart", "  Cart", @"asset\icon\iconl.png", new Point(0, buttonYPos));
            btnCart.MouseEnter += (s, args) => { btnCart.BackColor = Color.FromArgb(255, 50, 30, 30); };
            btnCart.MouseLeave += (s, args) => { btnCart.BackColor = Color.Transparent; };
            btnCart.Click += (s, e) =>
            {
                
                Cart cartForm = new Cart(cartItems, currentUser);
                DialogResult result = cartForm.ShowDialog();

                if (result == DialogResult.OK) 
                {
                    cartItems.Clear(); 



                    MessageBox.Show("Payment successful! Your cart has been cleared.", "Cart Cleared", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else if (result == DialogResult.Cancel)
                {
 
                }

            };
            panel1.Controls.Add(btnCart);
            buttonYPos += buttonHeight + buttonSpacing; 


            
            int bottomOffset = 250; 
                                    
                                    
            int idealBottomY = panel1.Height - bottomOffset;
            
            int calculatedY = Math.Max(buttonYPos, idealBottomY);


            btnLoginSignup = CreateMenuButton("btnLoginSignup", "  Login/Signup", @"asset\icon\login.png", new Point(0, calculatedY));


            btnLoginSignup.MouseEnter += (s, args) => { btnLoginSignup.BackColor = Color.FromArgb(255, 30, 50, 70); };
            btnLoginSignup.MouseLeave += (s, args) => { btnLoginSignup.BackColor = Color.Transparent; };
            btnLoginSignup.Click += (s, e) =>
            {
                if (currentUser == null)
                {
                    login loginForm = new login(); 
                    if (loginForm.ShowDialog() == DialogResult.OK)
                    {
                       
                        if (!string.IsNullOrEmpty(loginForm.LoggedInUsername)) 
                        {
                            currentUser = DatabaseConnection.GetUserData(loginForm.LoggedInUsername); 
                            UpdateLoginButton();
                            if (currentUser != null)
                            {
                                MessageBox.Show($"Welcome, {currentUser.fullName}!", "Login Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                        }
                    }
                }
                else
                {
                    profile profileForm = new profile(currentUser); 
                    profileForm.ShowDialog();
                    if (currentUser != null)
                    {
                        currentUser = DatabaseConnection.GetUserData(currentUser.fullName); 
                        UpdateLoginButton(); 
                    }
                }
            };
            panel1.Controls.Add(btnLoginSignup);

            buttonYPos = btnLoginSignup.Bottom + buttonSpacing;


            Button btnAbout = CreateMenuButton("btnAbout", "  About", @"asset\icon\about.png", new Point(0, buttonYPos));
            btnAbout.MouseEnter += (s, args) => { btnAbout.BackColor = Color.FromArgb(255, 60, 30, 60); };
            btnAbout.MouseLeave += (s, args) => { btnAbout.BackColor = Color.Transparent; };
            btnAbout.Click += (s, args) =>
            {
                about aboutForm = new about(); 
                aboutForm.ShowDialog();
            };
            panel1.Controls.Add(btnAbout);
            buttonYPos += buttonHeight + buttonSpacing;


            Button btnSettings = CreateMenuButton("btnSettings", "  Settings", @"asset\icon\setting.png", new Point(0, buttonYPos));
            btnSettings.MouseEnter += (s, args) => { btnSettings.BackColor = Color.FromArgb(255, 40, 40, 80); };
            btnSettings.MouseLeave += (s, args) => { btnSettings.BackColor = Color.Transparent; };
            btnSettings.Click += (s, args) =>
            {
                if (currentUser == null)
                {
                    MessageBox.Show("Please login to access settings.", "Login Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                setting settingsForm = new setting(currentUser, cartItems); // Pass currentUser and cartItems
                DialogResult settingsResult = settingsForm.ShowDialog(); // Show the settings form modally

                // *** THIS IS THE CRITICAL PART IN Form1.cs FOR LOGOUT ***
                if (settingsResult == DialogResult.OK) // Check if the settings form returned OK
                {
                    // This block executes if the settings form returned DialogResult.OK (i.e., user clicked Logout and confirmed)
                    currentUser = null; // Perform the logout: Clear the current user in Form1
                    cartItems.Clear(); // Clear the cart in Form1 for the logged-out user
                    UpdateLoginButton(); // Update the UI in Form1 (changes "Account" back to "Login/Signup")
                    MessageBox.Show("You have been logged out.", "Logged Out", MessageBoxButtons.OK, MessageBoxIcon.Information); // Show the logout message in Form1

                    // Optional: Refresh the car display if needed after logout
                    // LoadAndDisplayCarListPanels(); // Uncomment if you want to refresh the main car list after logout
                }
            };
            panel1.Controls.Add(btnSettings);
            buttonYPos += buttonHeight + buttonSpacing;


            Button btnContact = CreateMenuButton("btnContact", "  Contact Us", @"asset\icon\contact.png", new Point(0, buttonYPos));
            btnContact.MouseEnter += (s, args) => { btnContact.BackColor = Color.FromArgb(255, 30, 60, 60); };
            btnContact.MouseLeave += (s, args) => { btnContact.BackColor = Color.Transparent; };
            btnContact.Click += (s, e) =>
            {
                MessageBox.Show("Please contact us at support@carstore.com", "Contact");
            };
            panel1.Controls.Add(btnContact);

            UpdateLoginButton();
        }

        private Button CreateMenuButton(string name, string text, string iconRelativePath, Point location)
        {
            Button btn = new Button();
            btn.Name = name;
            btn.Text = text;
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.ImageAlign = ContentAlignment.MiddleLeft;
            btn.TextImageRelation = TextImageRelation.ImageBeforeText;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Color.Transparent;
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            btn.Size = new Size(panel1.Width, 50);
            btn.Location = location;
            btn.Margin = new Padding(10);
            btn.Padding = new Padding(10, 0, 0, 0);

            string iconPath = Application.StartupPath + @"\" + iconRelativePath;
            if (File.Exists(iconPath))
            {
                try
                {
                    using (Image originalIcon = Image.FromFile(iconPath))
                    {
                        Image resizedIcon = new Bitmap(originalIcon, new Size(35, 35));
                        btn.Image = resizedIcon;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error loading icon {iconRelativePath}: " + ex.Message);
                }
            }

            btn.MouseEnter += (s, args) =>
            {
                btn.BackColor = Color.FromArgb(100, 65, 65, 100);
            };

            btn.MouseLeave += (s, args) =>
            {
                btn.BackColor = Color.Transparent;
            };

            return btn;
        }

        private void UpdateLoginButton()
        {
            if (btnLoginSignup == null) return;

            if (currentUser != null)
            {
                btnLoginSignup.Text = "  Account";
                string accountIconPath = Application.StartupPath + @"\asset\icon\account.png"; 
                if (File.Exists(accountIconPath))
                {
                    try
                    {
                        using (Image originalIcon = Image.FromFile(accountIconPath))
                        {
                            Image resizedIcon = new Bitmap(originalIcon, new Size(35, 35));
                            btnLoginSignup.Image = resizedIcon;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error loading account icon: {ex.Message}");
                    }
                }
            }
            else
            {
                btnLoginSignup.Text = "  Login/Signup";
                // Reset to login icon
                string loginIconPath = Application.StartupPath + @"\asset\icon\login.png";
                if (File.Exists(loginIconPath))
                {
                    try
                    {
                        using (Image originalIcon = Image.FromFile(loginIconPath))
                        {
                            Image resizedIcon = new Bitmap(originalIcon, new Size(35, 35));
                            btnLoginSignup.Image = resizedIcon;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error loading login icon: {ex.Message}");
                    }
                }
            }
        }

        private void DisplayCar(int index)
        {
            if (carList == null || pictureBox1 == null || panel10 == null)
            {
                Debug.WriteLine("DisplayCar: carList, pictureBox1, or panel10 is null.");
                if (pictureBox1 != null) pictureBox1.Image = null;
                UpdateCarInfo("No Car Data", "", "");
                return;
            }


            if (index >= 0 && index < carList.Count)
            {
                currentCarIndex = index; 

                Car car = carList[currentCarIndex];

                if (!string.IsNullOrEmpty(car.ImageBase64))
                {
                    try
                    {
                        byte[] imageBytes = Convert.FromBase64String(car.ImageBase64);
                        using (MemoryStream ms = new MemoryStream(imageBytes))
                        {
                            if (pictureBox1.Image != null)
                            {
                                pictureBox1.Image.Dispose();
                            }
                            pictureBox1.Image = Image.FromStream(ms);
                        }
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBox1.BackColor = Color.Transparent;

                        AdjustPanelLayout(); 


                    }
                    catch (FormatException)
                    {
                        Debug.WriteLine($"CarID {car.CarID}: Invalid Base64 string for main image display.");
                        pictureBox1.Image = null; 
                        MessageBox.Show($"Could not display image for {car.Brand} {car.Model}: Invalid image data.", "Image Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (ArgumentException)
                    {
                        Debug.WriteLine($"CarID {car.CarID}: Argument error displaying main image from Base64.");
                        pictureBox1.Image = null; 
                        MessageBox.Show($"Could not display image for {car.Brand} {car.Model}: Image data format error.", "Image Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex) 
                    {
                        Debug.WriteLine($"Error loading main car image for CarID {car.CarID}: {ex.Message}");
                        pictureBox1.Image = null; 
                        MessageBox.Show($"Could not display image for {car.Brand} {car.Model}: {ex.Message}", "Image Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    pictureBox1.Image = null; 
                    Debug.WriteLine($"CarID {car.CarID}: No image data found.");
                    AdjustPanelLayout();
                }


                UpdateCarInfo(
                    $"{car.Brand} {car.Model} {car.Year}", 
                    $"${car.Price:#,##0.00} ETB", 
                    car.Description 
                );
            }
            else 
            {
                if (carList != null && carList.Count > 0)
                {
                    if (index >= carList.Count)
                    {
                        DisplayCar(0); 
                    }
                    else if (index < 0)
                    {
                        DisplayCar(carList.Count - 1); 
                    }
                }
                else
                {
                    pictureBox1.Image = null;
                    UpdateCarInfo("No Car Selected", "", "Please load cars first.");
                    AdjustPanelLayout();
                }
            }
        }


        private void UpdateCarInfo(string carName, string price, string specs)
        {
            if (panel10 == null)
            {
                Debug.WriteLine("UpdateCarInfo: panel10 is null.");
                return;
            }

            Control[] foundNameLabels = panel10.Controls.Find("lblCarName", false);
            if (foundNameLabels.Length > 0 && foundNameLabels[0] is Label lblCarName)
            {
                lblCarName.Text = carName;
            }

            Control[] foundPriceLabels = panel10.Controls.Find("lblPrice", false);
            if (foundPriceLabels.Length > 0 && foundPriceLabels[0] is Label lblPrice)
            {
                lblPrice.Text = price;
            }

            Control[] foundSpecsLabels = panel10.Controls.Find("lblSpecs", false);
            if (foundSpecsLabels.Length > 0 && foundSpecsLabels[0] is Label lblSpecs)
            {
                lblSpecs.Text = specs;
            }
        }


        private Image AdjustBrightness(Image image, float brightness)
        {
            if (image == null) return null;

            Bitmap bmp = new Bitmap(image.Width, image.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                float[][] matrix = {
                    new float[] {brightness, 0, 0, 0, 0},
                    new float[] {0, brightness, 0, 0, 0},
                    new float[] {0, 0, brightness, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1}
                };

                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(new ColorMatrix(matrix));

                g.DrawImage(image,
                    new Rectangle(0, 0, image.Width, image.Height),
                    0, 0, image.Width, image.Height,
                    GraphicsUnit.Pixel, attributes);
            }
            return bmp;
        }

        private void LoadAndDisplayCarListPanels()
        {
            if (panel2 == null)
            {
                Debug.WriteLine("LoadAndDisplayCarListPanels: panel2 is null.");
                return;
            }

            panel2.Controls.Clear();

            int startY = 5; 
            int panelHeight = 50; 
            int panelSpacing = 5; 
            int panelWidth = panel2.ClientSize.Width - 20 - (panel2.AutoScroll ? SystemInformation.VerticalScrollBarWidth : 0);
            int leftMargin = 10; 


            if (carList != null && carList.Count > 0)
            {
                for (int i = 0; i < carList.Count; i++)
                {
                    Car car = carList[i];



                    Panel modelItem = new Panel();
                    modelItem.Name = "modelItem" + car.CarID; 
                    modelItem.Size = new Size(panelWidth, panelHeight);
                    modelItem.BackColor = car.IsAvailable ? Color.FromArgb(100, 60, 60, 80) : Color.FromArgb(100, 100, 100, 100); // Semi-transparent; grey if unavailable

                    modelItem.Cursor = Cursors.Hand;
                    modelItem.Location = new Point(leftMargin, startY); 
                    modelItem.Tag = i; 

                    PictureBox pic = new PictureBox();
                    pic.Size = new Size(55, 40); 
                    pic.SizeMode = PictureBoxSizeMode.Zoom;
                    pic.Location = new Point(5, 5);
                    pic.BackColor = Color.Transparent;
                    pic.Cursor = Cursors.Hand;

                    if (!string.IsNullOrEmpty(car.ImageBase64))
                    {
                        try
                        {
                            byte[] imageBytes = Convert.FromBase64String(car.ImageBase64);
                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            {
                                pic.Image = new Bitmap(Image.FromStream(ms), pic.Size);
                            }
                        }
                        catch (FormatException)
                        {
                            Debug.WriteLine($"CarID {car.CarID}: Invalid Base64 string for image.");
                            pic.Image = null; 
                        }
                        catch (ArgumentException)
                        {
                            Debug.WriteLine($"CarID {car.CarID}: Argument error creating image from Base64.");
                            pic.Image = null; 
                        }
                        catch (Exception ex) 
                        {
                            Debug.WriteLine($"Error loading thumbnail image for CarID {car.CarID}: {ex.Message}");
                            pic.Image = null; 
                        }
                    }
                    else
                    {
                        pic.Image = null; 
                    }

                    EventHandler carClickHandler = (s, args) =>
                    {
                        int clickedIndex = (int)modelItem.Tag;
                        DisplayCar(clickedIndex); 
                    };

                    pic.Click += carClickHandler; 
                    modelItem.Controls.Add(pic);


                   
                    Label lblModel = new Label();
                    lblModel.Text = car.Brand + " " + car.Model; 
                    lblModel.ForeColor = car.IsAvailable ? Color.White : Color.LightGray; 
                    lblModel.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                    lblModel.AutoSize = true;
                    lblModel.Location = new Point(pic.Right + 10, 15);
                    lblModel.BackColor = Color.Transparent;
                    lblModel.Cursor = Cursors.Hand;
                    lblModel.Click += carClickHandler; 

                    lblModel.MouseEnter += (s, args) => { lblModel.ForeColor = Color.FromArgb(255, 50, 100); };
                    lblModel.MouseLeave += (s, args) => { lblModel.ForeColor = car.IsAvailable ? Color.White : Color.LightGray; }; 

                    modelItem.Controls.Add(lblModel);

                    Label lblStatus = new Label();
                    if (car.IsAvailable)
                    {
                        lblStatus.Text = $"${car.Price:#,##0.00} ETB"; 
                        lblStatus.ForeColor = Color.LimeGreen;
                    }
                    else
                    {
                        lblStatus.Text = "SOLD";
                        lblStatus.ForeColor = Color.Red;
                    }

                    lblStatus.Name = "lblStatus" + car.CarID; 
                    lblStatus.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                    lblStatus.AutoSize = true;
                    lblStatus.Location = new Point(lblModel.Right + 20, 15); 
                    lblStatus.BackColor = Color.Transparent;
                    lblStatus.Cursor = Cursors.Hand;
                    lblStatus.Click += carClickHandler; 
                    modelItem.Controls.Add(lblStatus);


                    panel2.SuspendLayout(); 
                    panel2.Controls.Add(modelItem);
                    panel2.ResumeLayout();

                    
                    startY += panelHeight + panelSpacing;


                    
                    modelItem.Click += carClickHandler;
                }
                
                panel2.VerticalScroll.Maximum = startY + 10; 
                panel2.AutoScroll = true; 

            }
            else
            {
                Label noCarsLabel = new Label()
                {
                    Text = "No cars available at the moment.",
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 12, FontStyle.Italic),
                    AutoSize = true,
                    Location = new Point(10, 10)
                };
                panel2.Controls.Add(noCarsLabel);
                panel2.AutoScroll = false;
            }
            
        }



        private void SetupCarInfoPanel()
        {
            if (panel10 == null)
            {
                Debug.WriteLine("SetupCarInfoPanel: panel10 is null.");
                return;
            }

            panel10.Controls.Clear();

            panel10.BackColor = Color.Transparent;
            panel10.AutoScroll = false; 

            Label lblCarName = new Label();
            lblCarName.Name = "lblCarName"; 
            lblCarName.Text = "Select a car model"; 
            lblCarName.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblCarName.ForeColor = Color.White;
            lblCarName.BackColor = Color.Transparent;
            lblCarName.AutoSize = true; 
            lblCarName.Location = new Point(80, 50);
            panel10.Controls.Add(lblCarName);

            Label lblPrice = new Label();
            lblPrice.Name = "lblPrice"; 
            lblPrice.Text = ""; 
            lblPrice.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblPrice.ForeColor = Color.LimeGreen;
            lblPrice.BackColor = Color.Transparent;
            lblPrice.AutoSize = true; 
            lblPrice.Location = new Point(80, 75); 
            panel10.Controls.Add(lblPrice);

            Label lblSpecs = new Label();
            lblSpecs.Name = "lblSpecs"; 
            lblSpecs.Text = ""; 
            lblSpecs.Font = new Font("Segoe UI", 9);
            lblSpecs.ForeColor = Color.White;
            lblSpecs.BackColor = Color.Transparent;
            lblSpecs.AutoSize = false;
            lblSpecs.Size = new Size(panel10.ClientSize.Width - 100, panel10.ClientSize.Height - 120); 
            lblSpecs.Location = new Point(80, 105); 
            panel10.Controls.Add(lblSpecs);
        }

        
        private void SetupNavigationArrows()
        {
            if (panel11 == null || panel12 == null)
            {
                Debug.WriteLine("SetupNavigationArrows: panel11 or panel12 is null.");
                return;
            }

            panel11.Controls.Clear(); 
            panel11.BackColor = Color.Transparent; 

            string nextImagePath = Path.Combine(Application.StartupPath, @"asset\icon\nextt.png"); 
            if (File.Exists(nextImagePath))
            {
                try
                {
                    PictureBox picNext = new PictureBox();
                    picNext.Name = "nextIcon"; 
                    picNext.Size = new Size(panel11.Width - 10, panel11.Height - 10); 
                    picNext.Location = new Point(5, 5); 

                    using (Image original = Image.FromFile(nextImagePath))
                    {
                        picNext.Image = new Bitmap(original, picNext.Size); 
                    }
                    picNext.SizeMode = PictureBoxSizeMode.Zoom;
                    picNext.BackColor = Color.Transparent;
                    picNext.Cursor = Cursors.Hand;

                    picNext.Click += (s, args) =>
                    {
                        if (carList != null && carList.Count > 0)
                        {
                            currentCarIndex++;
                            if (currentCarIndex >= carList.Count)
                            {
                                currentCarIndex = 0; 
                            }
                            DisplayCar(currentCarIndex); 
                        }
                        else
                        {
                            MessageBox.Show("No cars to navigate.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    };

                    


                    panel11.Controls.Add(picNext);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error setting up Next arrow: " + ex.Message);
                }
            }
            else
            {
                Debug.WriteLine("Next arrow image not found: " + nextImagePath);
            }

            panel12.Controls.Clear(); 
            panel12.BackColor = Color.Transparent; 
            string prevImagePath = Path.Combine(Application.StartupPath, @"asset\icon\previ.png"); 
            if (File.Exists(prevImagePath))
            {
                try
                {
                    PictureBox picPrev = new PictureBox();
                    picPrev.Name = "previousIcon"; 
                    picPrev.Size = new Size(panel12.Width + 20, panel12.Height - 10); 
                    picPrev.Location = new Point(5, 5); 

                    using (Image original = Image.FromFile(prevImagePath))
                    {
                        picPrev.Image = new Bitmap(original, picPrev.Size); 
                    }
                    picPrev.SizeMode = PictureBoxSizeMode.Zoom;
                    picPrev.BackColor = Color.Transparent;
                    picPrev.Cursor = Cursors.Hand;

                    picPrev.Click += (s, args) =>
                    {
                        if (carList != null && carList.Count > 0)
                        {
                            currentCarIndex--;
                            if (currentCarIndex < 0)
                            {
                                currentCarIndex = carList.Count - 1; 
                            }
                            DisplayCar(currentCarIndex); 
                        }
                        else
                        {
                            MessageBox.Show("No cars to navigate.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    };

              

                    panel12.Controls.Add(picPrev);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error setting up Previous arrow: " + ex.Message);
                }
            }
            else
            {
                Debug.WriteLine("Previous arrow image not found: " + prevImagePath);
            }
        }



        private void SetupAddToCartButton()
        {
            if (panel13 == null)
            {
                Debug.WriteLine("SetupAddToCartButton: panel13 is null.");
                return;
            }

            panel13.Controls.Clear(); 
            panel13.BackColor = Color.Transparent; 

            btnAddToCart = new Button(); 
            btnAddToCart.Name = "btnAddToCart"; 
            btnAddToCart.Text = "ADD TO CART";
            btnAddToCart.TextAlign = ContentAlignment.MiddleCenter;
            btnAddToCart.FlatStyle = FlatStyle.Flat;

            btnAddToCart.FlatAppearance.BorderSize = 2;
            btnAddToCart.FlatAppearance.BorderColor = Color.Black;

            btnAddToCart.BackColor = Color.FromArgb(50, 50, 100); 
            btnAddToCart.ForeColor = Color.White;
            btnAddToCart.Font = new Font("Segoe UI Semibold", 12, FontStyle.Bold);

            btnAddToCart.Size = new Size(panel13.Width - 20, panel13.Height - 20); 
            btnAddToCart.Location = new Point(10, 10); 
            btnAddToCart.Margin = new Padding(0); 

            btnAddToCart.MouseEnter += (s, args) =>
            {
                btnAddToCart.BackColor = Color.FromArgb(70, 70, 120); 
                btnAddToCart.Cursor = Cursors.Hand;
            };
            btnAddToCart.MouseLeave += (s, args) =>
            {
                btnAddToCart.BackColor = Color.FromArgb(50, 50, 100); 
            };

            btnAddToCart.MouseDown += (s, args) =>
            {
                btnAddToCart.BackColor = Color.FromArgb(30, 30, 80); 
            };
            btnAddToCart.MouseUp += (s, args) =>
            {
                if (btnAddToCart.ClientRectangle.Contains(btnAddToCart.PointToClient(Cursor.Position)))
                {
                    btnAddToCart.BackColor = Color.FromArgb(70, 70, 120);
                }
                else
                {
                    btnAddToCart.BackColor = Color.FromArgb(50, 50, 100); 
                }
            };


            string cartIconPath = Application.StartupPath + @"\asset\icon\addtocart.png"; 
            if (File.Exists(cartIconPath))
            {
                try
                {
                    using (Image originalCartIcon = Image.FromFile(cartIconPath))
                    {
                        Image resizedCartIcon = new Bitmap(originalCartIcon, new Size(24, 24));
                        btnAddToCart.Image = resizedCartIcon;
                        btnAddToCart.ImageAlign = ContentAlignment.MiddleRight;
                        btnAddToCart.TextImageRelation = TextImageRelation.TextBeforeImage;
                        btnAddToCart.Padding = new Padding(0, 0, 15, 0);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error loading Add to Cart button icon: " + ex.Message);
                }
            }

            btnAddToCart.Click += (s, args) =>
            {
                if (currentUser == null)
                {
                    MessageBox.Show("Please login to add items to your cart.", "Login Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (carList == null || carList.Count == 0 || currentCarIndex < 0 || currentCarIndex >= carList.Count)
                {
                    MessageBox.Show("Please select a car to add to cart.", "No Car Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Car selectedCar = carList[currentCarIndex];

                if (!selectedCar.IsAvailable)
                {
                    MessageBox.Show("This car is currently unavailable and cannot be added to the cart.", "Item Unavailable", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                DialogResult confirmResult = MessageBox.Show(
                    $"Add the {selectedCar.Brand} {selectedCar.Model} to your cart?",
                    "Confirm Add to Cart",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    CartItem existingCartItem = cartItems.FirstOrDefault(item => item.Car.CarID == selectedCar.CarID);

                    if (existingCartItem != null)
                    {
                        existingCartItem.Quantity++;
                        MessageBox.Show($"{selectedCar.Brand} {selectedCar.Model} quantity updated in cart. New quantity: {existingCartItem.Quantity}", "Cart Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        cartItems.Add(new CartItem { Car = selectedCar, Quantity = 1 });
                        MessageBox.Show($"{selectedCar.Brand} {selectedCar.Model} has been added to your cart.", "Added to Cart", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    
                }
                else
                {
                    MessageBox.Show("Adding to cart cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            panel13.Controls.Add(btnAddToCart);
        }


        private void Form1_Resize(object sender, EventArgs e)
        {
            AdjustPanelLayout();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void panel2_Paint(object sender, PaintEventArgs e) { }
        private void panel10_Paint(object sender, PaintEventArgs e) { }
        private void panel11_Paint(object sender, PaintEventArgs e) { }
        private void panel12_Paint(object sender, PaintEventArgs e) { }
        private void panel13_Paint(object sender, PaintEventArgs e) { }
    }
}