// Form1.cs
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq; // Added for using LINQ
using System.Runtime.InteropServices;
using System.Windows.Forms;
using carstore;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;
// Removed unnecessary static using System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace carstore
{
    public partial class Form1 : Form
    {
        private List<Car> carList;
        private int currentCarIndex = 0;
        private User currentUser = null;
        private Button btnLoginSignup = null;

        // *** MODIFIED: Use a list of CartItem to hold items in the user's cart and their quantities ***
        private List<CartItem> cartItems = new List<CartItem>();
        private Button btnAddToCart = null; // Declare the Add to Cart button

        public Form1()
        {
            InitializeComponent();

            // Setup UI elements
            SetupLeftMenu();
            SetupCarInfoPanel();
            SetupNavigationArrows();
            SetupAddToCartButton(); // Setup the Add to Cart button

            // Load cars from database on initial startup
            carList = DatabaseConnection.GetAllCars();

            // Initial layout adjustment
            AdjustPanelLayout();

            // If cars were loaded, display the first one
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
            // Set background image
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

            // Adjust panel layout
            AdjustPanelLayout();

            // Load and display car list panels - this should ideally happen only on initial load or explicit refresh
            if (carList != null && carList.Count > 0)
            {
                LoadAndDisplayCarListPanels();
            }
        }

        private void AdjustPanelLayout()
        {
            // Add null checks for panels just in case InitializeComponent failed
            if (panel1 == null || panel2 == null || panel10 == null || pictureBox1 == null || panel11 == null || panel12 == null || panel13 == null)
            {
                Debug.WriteLine("AdjustPanelLayout: One or more panels are null.");
                return; // Exit if controls aren't initialized
            }

            // Adjust panel2 (right car list) position and size
            panel2.Size = new Size(400, this.ClientSize.Height - 120);
            panel2.Location = new Point(this.ClientSize.Width - panel2.Width - 20, 60);

            // Adjust panel10 (car info) position
            int spaceBetweenPanels = this.ClientSize.Width - panel1.Width - panel2.Width;
            // panel10.Size = new Size(412, 282); // Let SetupCarInfoPanel handle size if needed
            panel10.Location = new Point(panel1.Width + (spaceBetweenPanels - panel10.Width) / 2, 12); // Keep Y position near top

            // Adjust pictureBox1 (main car image) position and size
            int availableWidth = this.ClientSize.Width - panel1.Width - panel2.Width;
            int availableHeight = this.ClientSize.Height;

            // Make image bigger
            int baseImageWidth = 1000;
            int baseImageHeight = 550;
            int adjustedWidth = (int)(availableWidth * 0.8); // Increased size to 80% of available width
            int adjustedHeight = (int)(baseImageHeight * (adjustedWidth / (float)baseImageWidth));
            pictureBox1.Size = new Size(adjustedWidth, adjustedHeight);

            int offsetX = 0;
            // Push image a little bit downward
            int offsetY = 50; // Changed from -50 to 50 to move it down

            pictureBox1.Left = panel1.Width + (availableWidth - pictureBox1.Width) / 2 + offsetX;
            pictureBox1.Top = (availableHeight - pictureBox1.Height) / 2 + offsetY; // Adjusted vertical position


            // Adjust panel11 (Next arrow) and panel12 (Previous arrow) positions relative to the *new* pictureBox1 position and size
            panel11.Size = new Size(135, 125); // Keep original arrow panel size
            panel12.Size = new Size(131, 125); // Keep original arrow panel size

            panel11.Location = new Point(pictureBox1.Right + 10, pictureBox1.Top + (pictureBox1.Height - panel11.Height) / 2);
            panel12.Location = new Point(pictureBox1.Left - panel12.Width - 10, pictureBox1.Top + (pictureBox1.Height - panel12.Height) / 2);


            // Adjust panel13 (Add to Cart button) position relative to pictureBox1's new position
            panel13.Size = new Size(302, 60); // Keep original button panel size
            panel13.Location = new Point(panel1.Width + (availableWidth - panel13.Width) / 2, pictureBox1.Bottom + 20);


            this.Invalidate();
            this.Refresh();
        }

        private void SetupLeftMenu()
        {
            // Add null check for panel1
            if (panel1 == null)
            {
                Debug.WriteLine("SetupLeftMenu: panel1 is null.");
                return;
            }

            panel1.Controls.Clear();
            panel1.BackColor = Color.FromArgb(100, 60, 60, 80);

            // Add Logo
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

            // --- Create Menu Buttons (Buy, Order, Account, About, Settings, Contact Us) ---
            int buttonYPos = 170; // Starting Y position for the first button
            int buttonHeight = 50;
            int buttonSpacing = 10;


            // Create Buy button (Now this button doesn't do anything directly,
            // the purchase flow is Add to Cart -> Cart -> Payment)
            // I'll keep it for now, but you might want to repurpose or remove it.
            //Button btnBuy = CreateMenuButton("btnBuy", "  Buy (Not Active)", @"asset\icon\buy.png", new Point(0, buttonYPos));
            //btnBuy.MouseEnter += (s, args) => { btnBuy.BackColor = Color.FromArgb(255, 10, 30, 32); };
            //btnBuy.MouseLeave += (s, args) => { btnBuy.BackColor = Color.Transparent; };
            //// btnBuy.Click += (s, args) => { /* This button's logic is now in the Cart form */ };
            //panel1.Controls.Add(btnBuy);
            //buttonYPos += buttonHeight + buttonSpacing;


            // Create Order button (Now this button shows past orders)
            Button btnOrder = CreateMenuButton("btnOrder", "  Orders", @"asset\icon\order.png", new Point(0, buttonYPos));
            btnOrder.MouseEnter += (s, args) => { btnOrder.BackColor = Color.FromArgb(255, 30, 30, 90); };
            btnOrder.MouseLeave += (s, args) => { btnOrder.BackColor = Color.Transparent; };
            btnOrder.Click += (s, e) =>
            {
                if (currentUser == null) // Add login check
                {
                    MessageBox.Show("Please login to view your orders.", "Login Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                // Pass the currentUser object to the order form
                order orderForm = new order(currentUser); // Assuming 'order' form exists and takes User
                orderForm.ShowDialog();
            };
            panel1.Controls.Add(btnOrder);
            buttonYPos += buttonHeight + buttonSpacing;

            // Create Cart button
            Button btnCart = CreateMenuButton("btnCart", "  Cart", @"asset\icon\iconl.png", new Point(0, buttonYPos)); // Assuming you have a cart icon
            btnCart.MouseEnter += (s, args) => { btnCart.BackColor = Color.FromArgb(255, 50, 30, 30); };
            btnCart.MouseLeave += (s, args) => { btnCart.BackColor = Color.Transparent; };
            btnCart.Click += (s, e) =>
            {
                // Open the Cart form and pass the cart items and the current user
                // *** MODIFIED: Pass the List<CartItem> to the Cart form ***
                Cart cartForm = new Cart(cartItems, currentUser);
                DialogResult result = cartForm.ShowDialog();

                // Optionally handle results from the Cart form, e.g., if an item was removed
                if (result == DialogResult.OK) // Assuming Cart form returns OK if payment was successful and cart should be cleared
                {
                    cartItems.Clear(); // Clear the cart after successful payment

                    // --- REMOVED: Code that reloads and redisplays the car list ---
                    // The list in panel2 will remain as it was before opening the cart.

                    MessageBox.Show("Payment successful! Your cart has been cleared.", "Cart Cleared", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else if (result == DialogResult.Cancel)
                {
                    // Cart was closed without payment, no change needed for cartItems in Form1's list
                    // If the Cart form allows removing items, the cartItems list here will be updated when the Cart form modifies the list object passed to it.
                }
                // Handle other potential DialogResults if needed
            };
            panel1.Controls.Add(btnCart);
            buttonYPos += buttonHeight + buttonSpacing; // Position the next button correctly


            // Create Login/Signup (Account) button
            int bottomOffset = 250; // Adjust this offset to position the button from the bottom
                                    // Ensure there's enough space, if not, dynamically calculate position
                                    // Calculate the ideal position from the bottom
            int idealBottomY = panel1.Height - bottomOffset;
            // Ensure the button doesn't overlap with the previous button
            int calculatedY = Math.Max(buttonYPos, idealBottomY);


            btnLoginSignup = CreateMenuButton("btnLoginSignup", "  Login/Signup", @"asset\icon\login.png", new Point(0, calculatedY));


            btnLoginSignup.MouseEnter += (s, args) => { btnLoginSignup.BackColor = Color.FromArgb(255, 30, 50, 70); };
            btnLoginSignup.MouseLeave += (s, args) => { btnLoginSignup.BackColor = Color.Transparent; };
            btnLoginSignup.Click += (s, e) =>
            {
                if (currentUser == null)
                {
                    login loginForm = new login(); // Assuming 'login' form exists
                    if (loginForm.ShowDialog() == DialogResult.OK)
                    {
                        // User successfully logged in
                        // Use the LoggedInUsername property from the login form
                        // You need a way for the login form to expose the logged-in username or User object
                        if (!string.IsNullOrEmpty(loginForm.LoggedInUsername)) // Assuming loginForm has a public property or method for this
                        {
                            currentUser = DatabaseConnection.GetUserData(loginForm.LoggedInUsername); // Assuming GetUserData exists
                            UpdateLoginButton();
                            // Optionally show a welcome message after successful login
                            if (currentUser != null)
                            {
                                MessageBox.Show($"Welcome, {currentUser.fullName}!", "Login Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            // Handle case where login form closed but didn't provide username (e.g., cancelled)
                        }
                    }
                }
                else
                {
                    // If user is logged in, open the profile page
                    profile profileForm = new profile(currentUser); // Assuming 'profile' form exists and takes User
                    profileForm.ShowDialog();
                    // After the profile form is closed, refresh user data in case it was updated
                    if (currentUser != null)
                    {
                        // Re-fetch user data in case profile form made changes
                        currentUser = DatabaseConnection.GetUserData(currentUser.fullName); // Assuming GetUserData exists
                        UpdateLoginButton(); // Update button text/icon if needed
                    }
                }
            };
            panel1.Controls.Add(btnLoginSignup);

            // Update buttonYPos for buttons below Login/Signup (if any)
            buttonYPos = btnLoginSignup.Bottom + buttonSpacing;


            // Create About button
            Button btnAbout = CreateMenuButton("btnAbout", "  About", @"asset\icon\about.png", new Point(0, buttonYPos));
            btnAbout.MouseEnter += (s, args) => { btnAbout.BackColor = Color.FromArgb(255, 60, 30, 60); };
            btnAbout.MouseLeave += (s, args) => { btnAbout.BackColor = Color.Transparent; };
            btnAbout.Click += (s, args) =>
            {
                about aboutForm = new about(); // Assuming you renamed the class to 'About'
                aboutForm.ShowDialog();
            };
            panel1.Controls.Add(btnAbout);
            buttonYPos += buttonHeight + buttonSpacing;


            // Create Settings button
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
                setting settingsForm = new setting(); // Assuming 'setting' form exists
                settingsForm.ShowDialog();
            };
            panel1.Controls.Add(btnSettings);
            buttonYPos += buttonHeight + buttonSpacing;


            // Create Contact Us button
            Button btnContact = CreateMenuButton("btnContact", "  Contact Us", @"asset\icon\contact.png", new Point(0, buttonYPos));
            btnContact.MouseEnter += (s, args) => { btnContact.BackColor = Color.FromArgb(255, 30, 60, 60); };
            btnContact.MouseLeave += (s, args) => { btnContact.BackColor = Color.Transparent; };
            btnContact.Click += (s, e) =>
            {
                MessageBox.Show("Please contact us at support@carstore.com", "Contact");
            };
            panel1.Controls.Add(btnContact);

            // Ensure the login/account button state is updated on initial load
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

        // Updated to reflect 'Account' state
        private void UpdateLoginButton()
        {
            // Add null check for btnLoginSignup
            if (btnLoginSignup == null) return;

            if (currentUser != null)
            {
                btnLoginSignup.Text = "  Account";
                // Update icon if you have an account icon
                string accountIconPath = Application.StartupPath + @"\asset\icon\account.png"; // Make sure you have this icon
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

        // Method to display a specific car's details and image in the main area
        private void DisplayCar(int index)
        {
            // Add null check for carList and pictureBox1, panel10
            if (carList == null || pictureBox1 == null || panel10 == null)
            {
                Debug.WriteLine("DisplayCar: carList, pictureBox1, or panel10 is null.");
                // Optionally clear display if carList is null/empty
                if (pictureBox1 != null) pictureBox1.Image = null;
                UpdateCarInfo("No Car Data", "", "");
                return;
            }


            // Ensure the index is within the valid range of the carList
            if (index >= 0 && index < carList.Count)
            {
                currentCarIndex = index; // Update the current index

                Car car = carList[currentCarIndex];

                // Display the main car image (from Base64) in pictureBox1
                if (!string.IsNullOrEmpty(car.ImageBase64))
                {
                    try
                    {
                        byte[] imageBytes = Convert.FromBase64String(car.ImageBase64);
                        using (MemoryStream ms = new MemoryStream(imageBytes))
                        {
                            // Dispose the previous image before loading a new one to free up memory
                            if (pictureBox1.Image != null)
                            {
                                pictureBox1.Image.Dispose();
                            }
                            pictureBox1.Image = Image.FromStream(ms);
                        }
                        // Keep existing size and centering logic for pictureBox1 if needed
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBox1.BackColor = Color.Transparent;

                        // Apply the adjusted size and position by calling AdjustPanelLayout
                        AdjustPanelLayout(); // Re-calculate size and position based on new window size if needed


                    }
                    catch (FormatException)
                    {
                        Debug.WriteLine($"CarID {car.CarID}: Invalid Base64 string for main image display.");
                        pictureBox1.Image = null; // Clear picture box on invalid Base64
                        MessageBox.Show($"Could not display image for {car.Brand} {car.Model}: Invalid image data.", "Image Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (ArgumentException)
                    {
                        Debug.WriteLine($"CarID {car.CarID}: Argument error displaying main image from Base64.");
                        pictureBox1.Image = null; // Clear picture box on argument error
                        MessageBox.Show($"Could not display image for {car.Brand} {car.Model}: Image data format error.", "Image Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex) // Catch other potential errors during image loading
                    {
                        Debug.WriteLine($"Error loading main car image for CarID {car.CarID}: {ex.Message}");
                        pictureBox1.Image = null; // Clear picture box on error
                        MessageBox.Show($"Could not display image for {car.Brand} {car.Model}: {ex.Message}", "Image Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    pictureBox1.Image = null; // Clear image if no Base64 data
                    Debug.WriteLine($"CarID {car.CarID}: No image data found.");
                    // Also adjust layout if the image is cleared (size might change)
                    AdjustPanelLayout();
                }


                // Update the car info panel (panel10)
                UpdateCarInfo(
                    $"{car.Brand} {car.Model} {car.Year}", // Example car name + year
                    $"${car.Price:#,##0.00} ETB", // Formatted price
                    car.Description // Use description from DB
                                    // You could format the description into a specs list here if needed
                );
            }
            else // This case handles index out of bounds for a non-empty list, or empty list
            {
                // Handle wrapping if index is out of bounds
                if (carList != null && carList.Count > 0)
                {
                    if (index >= carList.Count)
                    {
                        DisplayCar(0); // Wrap to first car
                    }
                    else if (index < 0)
                    {
                        DisplayCar(carList.Count - 1); // Wrap to last car
                    }
                }
                else
                {
                    // Handle case where carList is null or empty
                    pictureBox1.Image = null;
                    UpdateCarInfo("No Car Selected", "", "Please load cars first.");
                    // Adjust layout since no image is displayed
                    AdjustPanelLayout();
                    // Optionally disable navigation arrows if no cars
                }
            }
        }


        // This method updates the text of the labels in panel10
        private void UpdateCarInfo(string carName, string price, string specs)
        {
            // Add null check for panel10
            if (panel10 == null)
            {
                Debug.WriteLine("UpdateCarInfo: panel10 is null.");
                return;
            }

            // Find the labels by their names and update their text
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
                // You might need to adjust size or handle wrapping for lblSpecs if AutoSize is false
            }
        }


        // Keep the AdjustBrightness helper method
        private Image AdjustBrightness(Image image, float brightness)
        {
            // Add null check for input image
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

        // This method creates and displays the list of car thumbnails/names on the right panel (panel2)
        private void LoadAndDisplayCarListPanels()
        {
            // Add null check for panel2
            if (panel2 == null)
            {
                Debug.WriteLine("LoadAndDisplayCarListPanels: panel2 is null.");
                return;
            }

            // Clear any existing controls from the right container panel
            panel2.Controls.Clear();

            int startY = 5; // Starting Y position within panel2
            int panelHeight = 50; // Height of each car item panel
            int panelSpacing = 5; // Vertical spacing between panels
                                  // Adjust panel width based on panel2 client size and scrollbar
            int panelWidth = panel2.ClientSize.Width - 20 - (panel2.AutoScroll ? SystemInformation.VerticalScrollBarWidth : 0);
            int leftMargin = 10; // Left margin within panel2


            if (carList != null && carList.Count > 0)
            {
                for (int i = 0; i < carList.Count; i++)
                {
                    Car car = carList[i];

                    // Display ALL cars regardless of availability
                    // if (!car.IsAvailable) continue; // REMOVED this line


                    Panel modelItem = new Panel();
                    modelItem.Name = "modelItem" + car.CarID; // Use CarID for unique name
                    modelItem.Size = new Size(panelWidth, panelHeight);
                    // Use a different background color for unavailable cars
                    modelItem.BackColor = car.IsAvailable ? Color.FromArgb(100, 60, 60, 80) : Color.FromArgb(100, 100, 100, 100); // Semi-transparent; grey if unavailable

                    modelItem.Cursor = Cursors.Hand;
                    // Position the panel; need to track yOffset for available cars
                    modelItem.Location = new Point(leftMargin, startY); // Use current startY
                    modelItem.Tag = i; // Store the index of the car in the list for easy access on click

                    // Create and add PictureBox for car thumbnail
                    PictureBox pic = new PictureBox();
                    pic.Size = new Size(55, 40); // Thumbnail size
                    pic.SizeMode = PictureBoxSizeMode.Zoom;
                    pic.Location = new Point(5, 5);
                    pic.BackColor = Color.Transparent;
                    pic.Cursor = Cursors.Hand;

                    // Load image from Base64 if available and valid
                    if (!string.IsNullOrEmpty(car.ImageBase64))
                    {
                        try
                        {
                            byte[] imageBytes = Convert.FromBase64String(car.ImageBase64);
                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            {
                                // Create image and resize for thumbnail
                                pic.Image = new Bitmap(Image.FromStream(ms), pic.Size);
                            }
                        }
                        catch (FormatException)
                        {
                            Debug.WriteLine($"CarID {car.CarID}: Invalid Base64 string for image.");
                            pic.Image = null; // Show nothing if Base64 is invalid
                        }
                        catch (ArgumentException)
                        {
                            Debug.WriteLine($"CarID {car.CarID}: Argument error creating image from Base64.");
                            pic.Image = null; // Show nothing on argument error
                        }
                        catch (Exception ex) // Catch other potential errors during image loading
                        {
                            Debug.WriteLine($"Error loading thumbnail image for CarID {car.CarID}: {ex.Message}");
                            pic.Image = null; // Show nothing if image loading fails
                        }
                    }
                    else
                    {
                        pic.Image = null; // Show nothing if no image data
                    }

                    // Event handler for clicking the thumbnail or panel
                    EventHandler carClickHandler = (s, args) =>
                    {
                        // Get the index of the car from the Tag property of the clicked panel
                        int clickedIndex = (int)modelItem.Tag;
                        DisplayCar(clickedIndex); // Display the clicked car's details
                    };

                    pic.Click += carClickHandler; // Make picture clickable
                    modelItem.Controls.Add(pic);


                    // Create and add Label for car model name
                    Label lblModel = new Label();
                    lblModel.Text = car.Brand + " " + car.Model; // Use Brand and Model from DB
                    lblModel.ForeColor = car.IsAvailable ? Color.White : Color.LightGray; // Dim text if unavailable
                    lblModel.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                    lblModel.AutoSize = true; // Let label size itself based on text
                    lblModel.Location = new Point(pic.Right + 10, 15); // 10 pixels right of picture box
                    lblModel.BackColor = Color.Transparent;
                    lblModel.Cursor = Cursors.Hand;
                    lblModel.Click += carClickHandler; // Make label clickable

                    // Hover effect for the model name label
                    lblModel.MouseEnter += (s, args) => { lblModel.ForeColor = Color.FromArgb(255, 50, 100); };
                    lblModel.MouseLeave += (s, args) => { lblModel.ForeColor = car.IsAvailable ? Color.White : Color.LightGray; }; // Revert based on availability

                    modelItem.Controls.Add(lblModel);

                    // Create and add Label for car price or "SOLD" indicator
                    Label lblStatus = new Label();
                    if (car.IsAvailable)
                    {
                        lblStatus.Text = $"${car.Price:#,##0.00} ETB"; // Format price with currency and comma separator
                        lblStatus.ForeColor = Color.LimeGreen;
                    }
                    else
                    {
                        lblStatus.Text = "SOLD";
                        lblStatus.ForeColor = Color.Red;
                    }

                    lblStatus.Name = "lblStatus" + car.CarID; // Give it a unique name
                    lblStatus.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                    lblStatus.AutoSize = true; // Let label size itself
                    // Position to the right of the model label, with some spacing
                    lblStatus.Location = new Point(lblModel.Right + 20, 15); // Adjust spacing as needed

                    lblStatus.BackColor = Color.Transparent;
                    lblStatus.Cursor = Cursors.Hand;
                    lblStatus.Click += carClickHandler; // Make price/status label clickable

                    modelItem.Controls.Add(lblStatus);


                    // Add the dynamically created car item panel to the right container panel (panel2)
                    panel2.SuspendLayout(); // Use SuspendLayout/ResumeLayout for better performance when adding many controls
                    panel2.Controls.Add(modelItem);
                    panel2.ResumeLayout();

                    // Increment Y offset for the next car
                    startY += panelHeight + panelSpacing;


                    // Also make the main item panel itself clickable
                    modelItem.Click += carClickHandler;
                }
                // Adjust VerticalScroll Maximum based on the actual number of panels added
                panel2.VerticalScroll.Maximum = startY + 10; // Add some padding at the bottom
                panel2.AutoScroll = true; // Enable auto-scrolling

            }
            else
            {
                // Display a message if no cars are available to list
                Label noCarsLabel = new Label()
                {
                    Text = "No cars available at the moment.",
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 12, FontStyle.Italic),
                    AutoSize = true,
                    Location = new Point(10, 10)
                };
                panel2.Controls.Add(noCarsLabel);
                panel2.AutoScroll = false; // Disable scrolling
            }
            // After adding all car items, adjust panel2's size if needed and refresh
            // panel2.Invalidate();
            // panel2.Refresh();
        }


        /// <summary>
        /// Sets up the labels for the car information panel (panel10).
        /// </summary>
        private void SetupCarInfoPanel()
        {
            // Add null check for panel10
            if (panel10 == null)
            {
                Debug.WriteLine("SetupCarInfoPanel: panel10 is null.");
                return;
            }

            // Clear existing controls
            panel10.Controls.Clear();

            // Configure panel as fully transparent
            panel10.BackColor = Color.Transparent;
            panel10.AutoScroll = false; // Set to true if description can be long

            // Car name label (compact but visible)
            Label lblCarName = new Label();
            lblCarName.Name = "lblCarName"; // Give it a name to find it later
            lblCarName.Text = "Select a car model"; // Default text
            lblCarName.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblCarName.ForeColor = Color.White;
            lblCarName.BackColor = Color.Transparent;
            lblCarName.AutoSize = true; // Let it size based on text
            lblCarName.Location = new Point(80, 50);
            panel10.Controls.Add(lblCarName);

            // Price label
            Label lblPrice = new Label();
            lblPrice.Name = "lblPrice"; // Give it a name
            lblPrice.Text = ""; // Empty initially
            lblPrice.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblPrice.ForeColor = Color.LimeGreen;
            lblPrice.BackColor = Color.Transparent;
            lblPrice.AutoSize = true; // Let it size based on text
            lblPrice.Location = new Point(80, 75); // Below car name
            panel10.Controls.Add(lblPrice);

            // Compact specifications/description label
            Label lblSpecs = new Label();
            lblSpecs.Name = "lblSpecs"; // Give it a name
            lblSpecs.Text = ""; // Empty initially
            lblSpecs.Font = new Font("Segoe UI", 9);
            lblSpecs.ForeColor = Color.White;
            lblSpecs.BackColor = Color.Transparent;
            // Set AutoSize to false and manage size/wrap, or set to true and let it expand
            lblSpecs.AutoSize = false;
            // Adjust size based on panel10 client size
            lblSpecs.Size = new Size(panel10.ClientSize.Width - 100, panel10.ClientSize.Height - 120); // Adjusted size
            lblSpecs.Location = new Point(80, 105); // Below price, maybe add a separator
            panel10.Controls.Add(lblSpecs);
        }

        /// <summary>
        /// Sets up the PictureBoxes for the navigation arrows (panel11 for Next, panel12 for Previous).
        /// </summary>
        private void SetupNavigationArrows()
        {
            // Add null checks for panels
            if (panel11 == null || panel12 == null)
            {
                Debug.WriteLine("SetupNavigationArrows: panel11 or panel12 is null.");
                return;
            }

            // Setup Next arrow (panel11)
            panel11.Controls.Clear(); // Clear existing controls
            panel11.BackColor = Color.Transparent; // Ensure panel is transparent

            string nextImagePath = Path.Combine(Application.StartupPath, @"asset\icon\nextt.png"); // Adjust path
            if (File.Exists(nextImagePath))
            {
                try
                {
                    PictureBox picNext = new PictureBox();
                    picNext.Name = "nextIcon"; // Give it a name
                                               // Use a larger size for the icon that fits the panel better
                    picNext.Size = new Size(panel11.Width - 10, panel11.Height - 10); // Adjust size to fit panel with padding
                    picNext.Location = new Point(5, 5); // Add some padding

                    using (Image original = Image.FromFile(nextImagePath))
                    {
                        picNext.Image = new Bitmap(original, picNext.Size); // Resize image to PictureBox size
                    }
                    picNext.SizeMode = PictureBoxSizeMode.Zoom;
                    picNext.BackColor = Color.Transparent;
                    picNext.Cursor = Cursors.Hand;

                    // Attach the click handler
                    picNext.Click += (s, args) =>
                    {
                        if (carList != null && carList.Count > 0)
                        {
                            currentCarIndex++;
                            if (currentCarIndex >= carList.Count)
                            {
                                currentCarIndex = 0; // Wrap around to the first car
                            }
                            DisplayCar(currentCarIndex); // Display the next car
                        }
                        else
                        {
                            MessageBox.Show("No cars to navigate.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    };

                    // Hover effects (optional, re-add if desired)
                    // Assuming you have a logic for hover effects that creates different images
                    // Re-implementing simple hover effect for demonstration
                    // picNext.MouseEnter += (s, args) => { /* Apply hover effect */ }; // Add hover effect logic here
                    // picNext.MouseLeave += (s, args) => { /* Remove hover effect */ }; // Add mouse leave logic here


                    panel11.Controls.Add(picNext);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error setting up Next arrow: " + ex.Message);
                    // Handle error
                }
            }
            else
            {
                Debug.WriteLine("Next arrow image not found: " + nextImagePath);
                // Handle missing image
            }

            // Setup Previous arrow (panel12)
            panel12.Controls.Clear(); // Clear existing controls
            panel12.BackColor = Color.Transparent; // Ensure panel is transparent

            string prevImagePath = Path.Combine(Application.StartupPath, @"asset\icon\previ.png"); // Adjust path
            if (File.Exists(prevImagePath))
            {
                try
                {
                    PictureBox picPrev = new PictureBox();
                    picPrev.Name = "previousIcon"; // Give it a name
                                                   // Use a larger size for the icon that fits the panel better
                    picPrev.Size = new Size(panel12.Width + 20, panel12.Height - 10); // Adjust size to fit panel with padding
                    picPrev.Location = new Point(5, 5); // Add some padding

                    using (Image original = Image.FromFile(prevImagePath))
                    {
                        picPrev.Image = new Bitmap(original, picPrev.Size); // Resize image to PictureBox size
                    }
                    picPrev.SizeMode = PictureBoxSizeMode.Zoom;
                    picPrev.BackColor = Color.Transparent;
                    picPrev.Cursor = Cursors.Hand;

                    // Attach the click handler
                    picPrev.Click += (s, args) =>
                    {
                        if (carList != null && carList.Count > 0)
                        {
                            currentCarIndex--;
                            if (currentCarIndex < 0)
                            {
                                currentCarIndex = carList.Count - 1; // Wrap around to the last car
                            }
                            DisplayCar(currentCarIndex); // Display the previous car
                        }
                        else
                        {
                            MessageBox.Show("No cars to navigate.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    };

                    // Hover effects (optional, re-add if desired)
                    // picPrev.MouseEnter += (s, args) => { /* Apply hover effect */ }; // Add hover effect logic here
                    // picPrev.MouseLeave += (s, args) => { /* Remove hover effect */ }; // Add mouse leave logic here

                    panel12.Controls.Add(picPrev);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error setting up Previous arrow: " + ex.Message);
                    // Handle error
                }
            }
            else
            {
                Debug.WriteLine("Previous arrow image not found: " + prevImagePath);
                // Handle missing image
            }
        }


        /// <summary>
        /// Sets up the "Add to Cart" button in panel13.
        /// </summary>
        private void SetupAddToCartButton()
        {
            // Add null check for panel13
            if (panel13 == null)
            {
                Debug.WriteLine("SetupAddToCartButton: panel13 is null.");
                return;
            }

            panel13.Controls.Clear(); // Clear existing controls
            panel13.BackColor = Color.Transparent; // Ensure panel is transparent

            btnAddToCart = new Button(); // Use the class-level variable
            btnAddToCart.Name = "btnAddToCart"; // Give it a name
            btnAddToCart.Text = "ADD TO CART";
            btnAddToCart.TextAlign = ContentAlignment.MiddleCenter;
            btnAddToCart.FlatStyle = FlatStyle.Flat;

            // Add black border
            btnAddToCart.FlatAppearance.BorderSize = 2;
            btnAddToCart.FlatAppearance.BorderColor = Color.Black;

            // Style it differently from the old buy button
            btnAddToCart.BackColor = Color.FromArgb(50, 50, 100); // A shade of blue/purple
            btnAddToCart.ForeColor = Color.White;
            btnAddToCart.Font = new Font("Segoe UI Semibold", 12, FontStyle.Bold);

            // Size and position with better spacing to fill panel13
            btnAddToCart.Size = new Size(panel13.Width - 20, panel13.Height - 20); // Adjust size to fit panel with padding
            btnAddToCart.Location = new Point(10, 10); // Add padding
            btnAddToCart.Margin = new Padding(0); // Remove margin as size/location are set

            // Hover effects
            btnAddToCart.MouseEnter += (s, args) =>
            {
                btnAddToCart.BackColor = Color.FromArgb(70, 70, 120); // Slightly lighter on hover
                btnAddToCart.Cursor = Cursors.Hand;
            };
            btnAddToCart.MouseLeave += (s, args) =>
            {
                btnAddToCart.BackColor = Color.FromArgb(50, 50, 100); // Original color
            };

            // Click effect
            btnAddToCart.MouseDown += (s, args) =>
            {
                btnAddToCart.BackColor = Color.FromArgb(30, 30, 80); // Darker when pressed
            };
            btnAddToCart.MouseUp += (s, args) =>
            {
                // Return to hover color only if mouse is still over the button
                if (btnAddToCart.ClientRectangle.Contains(btnAddToCart.PointToClient(Cursor.Position)))
                {
                    btnAddToCart.BackColor = Color.FromArgb(70, 70, 120);
                }
                else
                {
                    btnAddToCart.BackColor = Color.FromArgb(50, 50, 100); // Orignal color if mouse left
                }
            };


            // Add icon with proper alignment
            string cartIconPath = Application.StartupPath + @"\asset\icon\addtocart.png"; // Assuming you have an add-to-cart icon
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
                    // Handle error
                }
            }

            // On click -> Add the current car to the cart
            btnAddToCart.Click += (s, args) =>
            {
                // 1. Check if user is logged in
                if (currentUser == null)
                {
                    MessageBox.Show("Please login to add items to your cart.", "Login Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Check if a car is currently displayed
                if (carList == null || carList.Count == 0 || currentCarIndex < 0 || currentCarIndex >= carList.Count)
                {
                    MessageBox.Show("Please select a car to add to cart.", "No Car Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Car selectedCar = carList[currentCarIndex];

                // Check if the car is available before adding to cart
                if (!selectedCar.IsAvailable)
                {
                    MessageBox.Show("This car is currently unavailable and cannot be added to the cart.", "Item Unavailable", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                // Optional: Add a confirmation before adding to cart
                DialogResult confirmResult = MessageBox.Show(
                    $"Add the {selectedCar.Brand} {selectedCar.Model} to your cart?",
                    "Confirm Add to Cart",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    // *** MODIFIED: Check if the car is already in the cartItems as a CartItem ***
                    CartItem existingCartItem = cartItems.FirstOrDefault(item => item.Car.CarID == selectedCar.CarID);

                    if (existingCartItem != null)
                    {
                        // If the item exists, increment its quantity
                        existingCartItem.Quantity++;
                        MessageBox.Show($"{selectedCar.Brand} {selectedCar.Model} quantity updated in cart. New quantity: {existingCartItem.Quantity}", "Cart Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // If the item does not exist, create a new CartItem with quantity 1 and add it
                        cartItems.Add(new CartItem { Car = selectedCar, Quantity = 1 });
                        MessageBox.Show($"{selectedCar.Brand} {selectedCar.Model} has been added to your cart.", "Added to Cart", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    // Optional: Update a cart item count display in the UI if you have one
                    // UpdateCartCountDisplay();
                }
                else
                {
                    // User cancelled adding to cart
                    MessageBox.Show("Adding to cart cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            // Add the button to panel13
            panel13.Controls.Add(btnAddToCart);
        }


        private void Form1_Resize(object sender, EventArgs e)
        {
            AdjustPanelLayout();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Keep empty for now, can be implemented if needed
        }

        // Keep all your existing Paint event handlers
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void panel2_Paint(object sender, PaintEventArgs e) { }
        private void panel10_Paint(object sender, PaintEventArgs e) { }
        private void panel11_Paint(object sender, PaintEventArgs e) { }
        private void panel12_Paint(object sender, PaintEventArgs e) { }
        private void panel13_Paint(object sender, PaintEventArgs e) { }
    }
}