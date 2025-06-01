using System;
using System.Collections.Generic; // Added for List
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using carstore; // Ensure this is here to use DatabaseConnection and Car classes

namespace carstore
{
    public partial class Form1 : Form
    {
        private List<Car> carList;
        private int currentCarIndex = 0; // To keep track of the currently displayed car

        // Declare panels and PictureBox controls if they are added via designer
        // If you are adding them fully programmatically and not using the designer,
        // uncomment their declarations here and remove InitializeComponent()
        // If using designer, ensure these controls exist with these names.
        // private Panel panel1; // Left menu panel
        // private Panel panel2; // Right car list container
        // private Panel panel10; // Car info panel
        // private Panel panel11; // Next arrow panel/container
        // private Panel panel12; // Previous arrow panel/container
        // private Panel panel13; // Proceed to Payment button panel/container
        // private PictureBox pictureBox1; // Main car image display
        // private TrackBar trackBar1; // If you are using this, declaration should be here or designer

        public Form1()
        {
            // InitializeComponent() is needed if you use the designer to place controls.
            // If you are creating ALL controls programmatically, you would remove this
            // and manually create/add controls to the form. Assuming you are using a mix
            // or mostly designer based on your previous code.
            InitializeComponent();

            // Setup UI elements that were previously in Paint events
            SetupLeftMenu();
            SetupCarInfoPanel();
            SetupNavigationArrows();
            SetupBuyPaymentButton();
            // Note: panel2 (right car list) content will be added by LoadAndDisplayCarListPanels in Form_Load

            // Load cars from database when the form is initialized
            carList = DatabaseConnection.GetAllCars();

            // Initial layout adjustment - this helps ensure panels have correct relative sizes before loading content
            AdjustPanelLayout();

            // If cars were loaded, display the first one. The list panels will be loaded in Form_Load.
            if (carList != null && carList.Count > 0)
            {
                DisplayCar(currentCarIndex); // Display the first car (index 0)
                // LoadAndDisplayCarListPanels() will be called in Form_Load
            }
            else
            {
                MessageBox.Show("No cars found in the database or error loading cars.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Optionally disable UI elements related to car selection/purchase
                // For example: panel11.Visible = false; panel12.Visible = false; panel13.Visible = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Set background image - keep this as it's a form background
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
                    // Handle the error, maybe set a default background color
                }
            }
            else
            {
                Debug.WriteLine("Background image not found: " + imgPath);
                // Handle the case where the background image is missing
            }

            // Adjust the positions and sizes of main panels based on the form size after loading
            // This ensures panels are sized correctly before loading dynamic content
            AdjustPanelLayout();

            // Now that panel2 is correctly sized, load and display the car list panels
            if (carList != null && carList.Count > 0)
            {
                LoadAndDisplayCarListPanels(); // Dynamically create the list of car thumbnails/names
            }
        }

        // Method to adjust panel layout based on form size
        private void AdjustPanelLayout()
        {
            // Adjust panel1 (left menu) - Dock.Left handles horizontal positioning
            // Consider adjusting vertical size if needed based on number of buttons
            // panel1.Height = this.ClientSize.Height; // Example if you want it to fill height

            // Adjust panel2 (right car list) position and size
            // Position it to the right of panel1 and fill the remaining height on the right side
            panel2.Size = new Size(400, this.ClientSize.Height - 120); // Set a fixed width, adjust height dynamically
            panel2.Location = new Point(this.ClientSize.Width - panel2.Width - 20, 60); // 20 pixels from right edge


            // Adjust panel10 (car info) position - Centered horizontally between panels 1 and 2, near the top
            int spaceBetweenPanels = this.ClientSize.Width - panel1.Width - panel2.Width;
            panel10.Size = new Size(412, 282); // Keep existing size or adjust dynamically
            panel10.Location = new Point(panel1.Width + (spaceBetweenPanels - panel10.Width) / 2, 12); // Centered horizontally


            // Adjust pictureBox1 (main car image) position and size - Centered horizontally/vertically in the main area
            int availableWidth = this.ClientSize.Width - panel1.Width - panel2.Width;
            int availableHeight = this.ClientSize.Height;

            // Recalculate position and size based on adjusted available area
            int baseImageWidth = 1000; // Use a base size for calculation
            int baseImageHeight = 550;
            // Scale down the image size to fit the main area better, maybe based on the available width
            int adjustedWidth = (int)(availableWidth * 0.6); // Use a percentage of available width
            int adjustedHeight = (int)(baseImageHeight * (adjustedWidth / (float)baseImageWidth)); // Maintain aspect ratio


            pictureBox1.Size = new Size(adjustedWidth, adjustedHeight);


            int offsetX = 0; // Adjust horizontal offset if needed
            int offsetY = -50; // Adjust vertical offset upwards to make space for the button
            pictureBox1.Left = panel1.Width + (availableWidth - pictureBox1.Width) / 2 + offsetX;
            pictureBox1.Top = (availableHeight - pictureBox1.Height) / 2 + offsetY;


            // Adjust panel11 (Next arrow) and panel12 (Previous arrow) positions
            // Position them relative to pictureBox1
            panel11.Size = new Size(135, 125); // Keep designer size
            panel12.Size = new Size(131, 125); // Keep designer size

            panel11.Location = new Point(pictureBox1.Right + 10, pictureBox1.Top + (pictureBox1.Height - panel11.Height) / 2); // To the right of pictureBox1, vertically centered
            panel12.Location = new Point(pictureBox1.Left - panel12.Width - 10, pictureBox1.Top + (pictureBox1.Height - panel12.Height) / 2); // To the left of pictureBox1, vertically centered

            // Adjust panel13 (Buy button) position - Centered horizontally below pictureBox1
            panel13.Size = new Size(302, 60); // Keep designer size
            panel13.Location = new Point(panel1.Width + (availableWidth - panel13.Width) / 2, pictureBox1.Bottom + 20); // Below pictureBox1 with 20px margin


            // Adjust positions of panels 3 through 9 (Assuming they are part of the right car list or similar)
            // If panels 3-9 are meant to be part of the right car list (panel2), they should be added as controls
            // within panel2 and their positions should be relative to panel2's client area.
            // If they are separate elements, position them relative to other main elements or the form.
            // Based on the designer, they seem to be fixed position. If they are meant to be the dynamic car list items,
            // you should remove them from the designer and rely solely on the programmatic creation in LoadAndDisplayCarListPanels.
            // Assuming for now they are not the dynamic car list items and need fixed positions relative to panel2's area.
            // If they are indeed the static list items, this part needs to be removed.
            int rightPanelStartX = panel2.Location.X;
            // These fixed positions will likely overlap with the dynamic list in panel2 if panels 3-9 are on the right side.
            // Consider if these panels are necessary or if their content should be part of the dynamic list items.
            // For now, keeping their designer-defined locations but be aware of potential overlaps.
            //panel3.Location = new Point(rightPanelStartX, 145); // Adjust X based on panel2's X
            //panel4.Location = new Point(rightPanelStartX, 234); // Adjust X
            //panel5.Location = new Point(rightPanelStartX, 328); // Adjust X
            //panel6.Location = new Point(rightPanelStartX, 416); // Adjust X
            //panel7.Location = new Point(rightPanelStartX, 506); // Adjust X
            //panel8.Location = new Point(rightPanelStartX, 591); // Adjust X
            //panel9.Location = new Point(rightPanelStartX, 672); // Adjust X


            // Invalidate and refresh to redraw the layout
            this.Invalidate();
            this.Refresh();
        }

        /// <summary>
        /// Sets up the static controls for the left menu panel (panel1).
        /// </summary>
        private void SetupLeftMenu()
        {
            // Clear existing controls to prevent duplicates if called multiple times (though it shouldn't be after constructor)
            panel1.Controls.Clear();

            // Configure the panel
            panel1.BackColor = Color.FromArgb(100, 60, 60, 80); // Semi-transparent gray
            // panel1.Dock = DockStyle.Left; // Already set in designer, ensure it is

            // Add Logo at the top center of the panel using resize logic
            PictureBox logo = new PictureBox();
            logo.Name = "logo"; // Give it a name
            string logoPath = Application.StartupPath + @"\asset\bg\logo.jpg"; // Path to your logo

            if (File.Exists(logoPath))
            {
                try
                {
                    using (Image originalLogo = Image.FromFile(logoPath))
                    {
                        // Resize logic remains the same
                        Image resizedLogo = new Bitmap(originalLogo, new Size(250, 200)); // Resize logo

                        logo.Image = resizedLogo;
                        logo.SizeMode = PictureBoxSizeMode.Zoom;
                        logo.Size = new Size(300, 200);

                        // Center horizontally in panel
                        int centerX = (panel1.Width - logo.Width) / 2;
                        logo.Location = new Point(centerX, -25); //top center

                        logo.BackColor = Color.Transparent;

                        panel1.Controls.Add(logo);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error loading logo image: " + ex.Message);
                    // Handle error
                }
            }
            else
            {
                Debug.WriteLine("Logo image not found: " + logoPath);
                // Handle missing image
            }


            // --- Create Menu Buttons (Profile, Buy, Order, Login/Signup, About, Settings, Contact Us) ---
            int buttonYPos = 170; // Starting Y position for the first button
            int buttonHeight = 50;
            int buttonSpacing = 10; // Vertical spacing between buttons

            // Create Profile button
            Button btnProfile = CreateMenuButton("btnProfile", "  Profile", @"asset\icon\model.png", new Point(0, buttonYPos));
            btnProfile.Click += (s, args) =>
            {
                profile profileForm = new profile();
                profileForm.ShowDialog();
            };
            panel1.Controls.Add(btnProfile);
            buttonYPos += buttonHeight + buttonSpacing; // Move down for next button


            // Create Buy button
            Button btnBuy = CreateMenuButton("btnBuy", "  Buy", @"asset\icon\buy.png", new Point(0, buttonYPos));
            // Adjust hover color
            btnBuy.MouseEnter += (s, args) => { btnBuy.BackColor = Color.FromArgb(255, 10, 30, 32); };
            btnBuy.MouseLeave += (s, args) => { btnBuy.BackColor = Color.Transparent; };
            btnBuy.Click += (s, args) =>
            {
                payment payForm = new payment();
                payForm.StartPosition = FormStartPosition.CenterScreen;
                payForm.ShowDialog();
            };
            panel1.Controls.Add(btnBuy);
            buttonYPos += buttonHeight + buttonSpacing;


            // Create Order button
            Button btnOrder = CreateMenuButton("btnOrder", "  Order", @"asset\icon\order.png", new Point(0, buttonYPos));
            // Adjust hover color
            btnOrder.MouseEnter += (s, args) => { btnOrder.BackColor = Color.FromArgb(255, 30, 30, 90); };
            btnOrder.MouseLeave += (s, args) => { btnOrder.BackColor = Color.Transparent; };
            btnOrder.Click += (s, e) =>
            {
                order orderForm = new order();
                orderForm.ShowDialog();
            };
            panel1.Controls.Add(btnOrder);
            buttonYPos += buttonHeight + buttonSpacing;


            // Create Login/Signup button
            // Position this button near the bottom, relative to panel height
            int bottomOffset = 250; // Adjust this to position button from the bottom
            Button btnLoginSignup = CreateMenuButton("btnLoginSignup", "  Login/Signup", @"asset\icon\login.png", new Point(0, panel1.Height - bottomOffset));
            // Adjust hover color
            btnLoginSignup.MouseEnter += (s, args) => { btnLoginSignup.BackColor = Color.FromArgb(255, 30, 50, 70); };
            btnLoginSignup.MouseLeave += (s, args) => { btnLoginSignup.BackColor = Color.Transparent; };
            btnLoginSignup.Click += (s, e) =>
            {
                login loginForm = new login();
                loginForm.ShowDialog();
            };
            panel1.Controls.Add(btnLoginSignup);
            // Update buttonYPos for subsequent buttons below Login/Signup
            buttonYPos = btnLoginSignup.Bottom + 10;


            // Create About button
            Button btnAbout = CreateMenuButton("btnAbout", "  About", @"asset\icon\about.png", new Point(0, buttonYPos));
            // Adjust hover color
            btnAbout.MouseEnter += (s, args) => { btnAbout.BackColor = Color.FromArgb(255, 60, 30, 60); };
            btnAbout.MouseLeave += (s, args) => { btnAbout.BackColor = Color.Transparent; };
            btnAbout.Click += (s, args) =>
            {
                about aboutForm = new about();
                aboutForm.ShowDialog();
            };
            panel1.Controls.Add(btnAbout);
            buttonYPos += buttonHeight + buttonSpacing;


            // Create Settings button
            Button btnSettings = CreateMenuButton("btnSettings", "  Settings", @"asset\icon\setting.png", new Point(0, buttonYPos));
            // Adjust hover color
            btnSettings.MouseEnter += (s, args) => { btnSettings.BackColor = Color.FromArgb(255, 40, 40, 80); };
            btnSettings.MouseLeave += (s, args) => { btnSettings.BackColor = Color.Transparent; };
            btnSettings.Click += (s, args) =>
            {
                setting settingsForm = new setting();
                settingsForm.ShowDialog();
            };
            panel1.Controls.Add(btnSettings);
            buttonYPos += buttonHeight + buttonSpacing;


            // Create Contact Us button
            Button btnContact = CreateMenuButton("btnContact", "  Contact Us", @"asset\icon\contact.png", new Point(0, buttonYPos));
            // Adjust hover color
            btnContact.MouseEnter += (s, args) => { btnContact.BackColor = Color.FromArgb(255, 30, 60, 60); };
            btnContact.MouseLeave += (s, args) => { btnContact.BackColor = Color.Transparent; };
            btnContact.Click += (s, e) =>
            {
                MessageBox.Show("Please contact us support@carstore.com ", "Contact");
            };
            panel1.Controls.Add(btnContact);

            // Adjust panel height to fit buttons dynamically if needed (Optional)
            // int lastButtonBottom = btnContact.Bottom;
            // if (panel1.Height < lastButtonBottom + 20) // Add some padding at the bottom
            // {
            //     panel1.Height = lastButtonBottom + 20;
            // }
        }

        /// <summary>
        /// Helper method to create and style a standard menu button.
        /// </summary>
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

            // Size and positioning
            btn.Size = new Size(panel1.Width, 50);   // Full width of panel1
            btn.Location = location; // Use the provided location
            btn.Margin = new Padding(10);
            btn.Padding = new Padding(10, 0, 0, 0);  // Space between icon and text

            // Load and resize icon
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
                    // Handle error
                }
            }


            // Default hover effect (can be overridden for specific buttons)
            btn.MouseEnter += (s, args) =>
            {
                btn.BackColor = Color.FromArgb(100, 65, 65, 100);  // Default hover color
            };

            btn.MouseLeave += (s, args) =>
            {
                btn.BackColor = Color.Transparent;
            };

            return btn;
        }


        /// <summary>
        /// Sets up the labels for the car information panel (panel10).
        /// </summary>
        private void SetupCarInfoPanel()
        {
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
            lblSpecs.Size = new Size(panel10.Width - 20, panel10.Height - 70); // Adjust size as needed
            lblSpecs.Location = new Point(80, 105); // Below price, maybe add a separator
            panel10.Controls.Add(lblSpecs);
        }

        /// <summary>
        /// Sets up the PictureBoxes for the navigation arrows (panel11 for Next, panel12 for Previous).
        /// </summary>
        private void SetupNavigationArrows()
        {
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
                    Image normalImage = new Bitmap(Image.FromFile(nextImagePath), new Size(90, 50)); // Adjust size
                    Image hoverImage = AdjustBrightness(normalImage, 1.3f);
                    picNext.MouseEnter += (s, args) => picNext.Image = hoverImage;
                    picNext.MouseLeave += (s, args) => picNext.Image = normalImage;


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
                    picPrev.Name = "prevIcon"; // Give it a name
                    // Use a larger size for the icon that fits the panel better
                    picPrev.Size = new Size(panel12.Width - 10, panel12.Height - 10); // Adjust size to fit panel with padding
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
                    Image normalImage = new Bitmap(Image.FromFile(prevImagePath), new Size(90, 50)); // Adjust size
                    Image hoverImage = AdjustBrightness(normalImage, 1.3f);
                    picPrev.MouseEnter += (s, args) => picPrev.Image = hoverImage;
                    picPrev.MouseLeave += (s, args) => picPrev.Image = normalImage;


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
        /// Sets up the "Proceed to Payment" button in panel13.
        /// </summary>
        private void SetupBuyPaymentButton()
        {
            panel13.Controls.Clear(); // Clear existing controls
            panel13.BackColor = Color.Transparent; // Ensure panel is transparent

            Button btnBuyPayment = new Button();
            btnBuyPayment.Name = "btnBuyPayment"; // Give it a name
            btnBuyPayment.Text = "PROCEED TO PAYMENT";
            btnBuyPayment.TextAlign = ContentAlignment.MiddleCenter;
            btnBuyPayment.FlatStyle = FlatStyle.Flat;

            // Add black border
            btnBuyPayment.FlatAppearance.BorderSize = 2;
            btnBuyPayment.FlatAppearance.BorderColor = Color.Black;

            // Dark green color scheme
            btnBuyPayment.BackColor = Color.FromArgb(0, 50, 0); // Dark green background
            btnBuyPayment.ForeColor = Color.White;
            btnBuyPayment.Font = new Font("Segoe UI Semibold", 12, FontStyle.Bold);

            // Size and position with better spacing to fill panel13
            btnBuyPayment.Size = new Size(panel13.Width - 20, panel13.Height - 20); // Adjust size to fit panel with padding
            btnBuyPayment.Location = new Point(10, 10); // Add padding
            btnBuyPayment.Margin = new Padding(0); // Remove margin as size/location are set

            // Hover effects with smooth transition colors
            btnBuyPayment.MouseEnter += (s, args) =>
            {
                btnBuyPayment.BackColor = Color.FromArgb(0, 70, 0); // Slightly lighter green on hover
                btnBuyPayment.Cursor = Cursors.Hand;
            };
            btnBuyPayment.MouseLeave += (s, args) =>
            {
                btnBuyPayment.BackColor = Color.FromArgb(0, 50, 0); // Original dark green
            };

            // Click effect
            btnBuyPayment.MouseDown += (s, args) =>
            {
                btnBuyPayment.BackColor = Color.FromArgb(0, 30, 0); // Darker green when pressed
            };
            btnBuyPayment.MouseUp += (s, args) =>
            {
                btnBuyPayment.BackColor = Color.FromArgb(0, 70, 0); // Return to hover color
            };

            // Add icon with proper alignment
            string buyIconPath = Application.StartupPath + @"\asset\icon\pay.png"; // Adjust path
            if (File.Exists(buyIconPath))
            {
                try
                {
                    using (Image originalBuyIcon = Image.FromFile(buyIconPath))
                    {
                        Image resizedBuyIcon = new Bitmap(originalBuyIcon, new Size(24, 24));
                        btnBuyPayment.Image = resizedBuyIcon;
                        btnBuyPayment.ImageAlign = ContentAlignment.MiddleRight;
                        btnBuyPayment.TextImageRelation = TextImageRelation.TextBeforeImage;
                        btnBuyPayment.Padding = new Padding(0, 0, 15, 0);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error loading Payment button icon: " + ex.Message);
                    // Handle error
                }
            }

            // On click → open payment page
            btnBuyPayment.Click += (s, args) =>
            {
                payment payForm = new payment(); // Make sure 'payment' form exists
                payForm.StartPosition = FormStartPosition.CenterScreen;
                payForm.ShowDialog();
            };

            // Add the button to panel13
            panel13.Controls.Add(btnBuyPayment);
        }


        // This method creates and displays the list of car thumbnails/names on the right panel (panel2)
        private void LoadAndDisplayCarListPanels()
        {
            // Clear any existing controls from the right container panel
            panel2.Controls.Clear();

            int startY = 5; // Starting Y position within panel2
            int panelHeight = 50; // Height of each car item panel
            int panelSpacing = 5; // Vertical spacing between panels
            int panelWidth = panel2.ClientSize.Width - 20; // Make panel width adapt to panel2 width, with padding
            int leftMargin = 10; // Left margin within panel2


            if (carList != null && carList.Count > 0)
            {
                for (int i = 0; i < carList.Count; i++)
                {
                    Car car = carList[i];

                    Panel modelItem = new Panel();
                    modelItem.Name = "modelItem" + car.CarID; // Use CarID for unique name
                    modelItem.Size = new Size(panelWidth, panelHeight);
                    modelItem.BackColor = Color.FromArgb(100, 60, 60, 80); // Semi-transparent
                    modelItem.Cursor = Cursors.Hand;
                    modelItem.Location = new Point(leftMargin, startY + (panelHeight + panelSpacing) * i);
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
                    lblModel.ForeColor = Color.White;
                    lblModel.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                    lblModel.AutoSize = true; // Let label size itself based on text
                    lblModel.Location = new Point(pic.Right + 10, 15); // 10 pixels right of picture box
                    lblModel.BackColor = Color.Transparent;
                    lblModel.Cursor = Cursors.Hand;
                    lblModel.Click += carClickHandler; // Make label clickable

                    // Hover effect for the model name label
                    lblModel.MouseEnter += (s, args) => { lblModel.ForeColor = Color.FromArgb(255, 50, 100); };
                    lblModel.MouseLeave += (s, args) => { lblModel.ForeColor = Color.White; };

                    modelItem.Controls.Add(lblModel);

                    // Create and add Label for car price
                    Label lblPrice = new Label();
                    lblPrice.Text = $"${car.Price:#,##0.00} ETB"; // Format price with currency and comma separator
                    lblPrice.ForeColor = Color.LimeGreen;
                    lblPrice.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                    lblPrice.AutoSize = true; // Let label size itself
                    // Position to the right of the model label, with some spacing
                    lblPrice.Location = new Point(lblModel.Right + 20, 15); // Adjust spacing as needed

                    lblPrice.BackColor = Color.Transparent;
                    lblPrice.Cursor = Cursors.Hand;
                    lblPrice.Click += carClickHandler; // Make price label clickable

                    modelItem.Controls.Add(lblPrice);

                    // Add the dynamically created car item panel to the right container panel (panel2)
                    panel2.SuspendLayout(); // Use SuspendLayout/ResumeLayout for better performance when adding many controls
                    panel2.Controls.Add(modelItem);
                    panel2.ResumeLayout();


                    // Also make the main item panel itself clickable
                    modelItem.Click += carClickHandler;
                }
                panel2.VerticalScroll.Maximum = startY + (panelHeight + panelSpacing) * carList.Count + 10; // Enable scrolling if needed
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


        // Method to display a specific car's details and image in the main area
        private void DisplayCar(int index)
        {
            // Ensure the index is within the valid range of the carList
            if (carList != null && index >= 0 && index < carList.Count)
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

                        // Apply the adjusted size
                        int baseImageWidth = 1300; // Use a base size for calculation
                        int baseImageHeight = 550;
                        int availableWidth = this.ClientSize.Width - panel1.Width - panel2.Width;
                        int adjustedWidth = (int)(availableWidth * 0.6); // Use a percentage of available width
                        int adjustedHeight = (int)(baseImageHeight * (adjustedWidth / (float)baseImageWidth)); // Maintain aspect ratio

                        pictureBox1.Size = new Size(adjustedWidth, adjustedHeight);


                        // Center the PictureBox between the panels (panel1 as left, panel2 as right)
                        // Adjust these panel references if they are different in your designer
                        int leftPanelWidth = panel1.Width;
                        int rightPanelWidth = panel2.Width;

                        availableWidth = this.ClientSize.Width - (leftPanelWidth + rightPanelWidth);
                        int availableHeight = this.ClientSize.Height;

                        // Centering the PictureBox with an offset
                        int offsetX = 0; // Adjust horizontal offset
                        int offsetY = -50; // Adjust vertical offset upwards to make space for the button
                        pictureBox1.Left = panel1.Width + (availableWidth - pictureBox1.Width) / 2 + offsetX;
                        pictureBox1.Top = (availableHeight - pictureBox1.Height) / 2 + offsetY;

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
                }


                // Update the car info panel (panel10)
                UpdateCarInfo(
                    $"{car.Brand} {car.Model} {car.Year}", // Example car name + year
                    $"${car.Price:#,##0.00} ETB", // Formatted price
                    car.Description // Use description from DB
                                    // You could format the description into a specs list here if needed
                );
            }
            else if (carList != null && carList.Count > 0)
            {
                // Handle wrapping if index is out of bounds - this case should ideally be handled by the caller
                // but adding robust handling here is also fine.
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
                // Optionally disable navigation arrows if no cars
            }
        }


        // This method updates the text of the labels in panel10
        private void UpdateCarInfo(string carName, string price, string specs)
        {
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


        // Remove Paint event handlers as controls are added in the constructor
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void panel2_Paint(object sender, PaintEventArgs e) { }
        private void panel3_Paint(object sender, PaintEventArgs e) { }
        private void panel4_Paint(object sender, PaintEventArgs e) { }
        private void panel5_Paint(object sender, PaintEventArgs e) { }
        private void panel6_Paint(object sender, PaintEventArgs e) { }
        private void panel7_Paint(object sender, PaintEventArgs e) { }
        private void panel8_Paint(object sender, PaintEventArgs e) { }
        private void panel9_Paint(object sender, PaintEventArgs e) { }
        private void panel10_Paint(object sender, PaintEventArgs e) { }
        private void panel11_Paint_2(object sender, PaintEventArgs e) { } // Assuming this was for Next arrow
        private void panel12_Paint(object sender, PaintEventArgs e) { } // Assuming this was for Previous arrow
        private void panel13_Paint_1(object sender, PaintEventArgs e) { } // Assuming this was for Buy button
        private void panel11_Paint(object sender, PaintEventArgs e) { } // Keep if it exists in designer
        private void panel11_Paint_1(object sender, PaintEventArgs e) { } // Keep if it exists in designer


        // Add a Form Resize event handler to adjust layout when the form is resized
        private void Form1_Resize(object sender, EventArgs e)
        {
            AdjustPanelLayout(); // Call the layout adjustment method on resize
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint_2(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void panel8_Paint_1(object sender, PaintEventArgs e)
        {

        }
    }
}