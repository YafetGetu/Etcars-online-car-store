using System.Diagnostics;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using carstore;

namespace carstore
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Set background image
            string imgPath = Application.StartupPath + @"\asset\bg\bg1.jpg";
            this.BackgroundImage = Image.FromFile(imgPath);
            this.BackgroundImageLayout = ImageLayout.Stretch;

            // Load the initial image for pictureBox1
            LoadInitialCarImage();
        }

        private void LoadInitialCarImage()
        {
            string imagePath = Application.StartupPath + @"\asset\models\car1.png";

            if (File.Exists(imagePath))
            {
                Image image = Image.FromFile(imagePath);
                pictureBox1.Image = image;
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox1.BackColor = Color.Transparent;

                // Set the size
                int initialWidth = 1000;
                int initialHeight = 550;
                int increasedWidth = (int)(initialWidth * 1.2);
                int increasedHeight = (int)(initialHeight * 1.2);
                pictureBox1.Size = new Size(increasedWidth, increasedHeight);

                // Center the PictureBox
                int leftPanelWidth = panel1.Width;
                int rightPanelWidth = panel1.Width;
                int availableWidth = this.ClientSize.Width - (leftPanelWidth + rightPanelWidth);
                int availableHeight = this.ClientSize.Height;

                int offset = 65;
                pictureBox1.Left = leftPanelWidth + (availableWidth - pictureBox1.Width) / 2 + offset;
                pictureBox1.Top = (availableHeight - pictureBox1.Height) / 2;
            }
            else
            {
                MessageBox.Show("Initial image not found. Please check the path.");
            }
        }



        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (panel1.Controls.Find("btnDashboard", false).Length == 0)
            {
                // Configure the panel
                panel1.BackColor = Color.FromArgb(100, 60, 60, 80); // Semi-transparent gray


                // Add Logo at the top center of the panel using resize logic
                PictureBox logo = new PictureBox();
                logo.Name = "logo";
                string logoPath = Application.StartupPath + @"\asset\bg\logo.jpg";  // Path to your logo

                if (File.Exists(logoPath))
                {
                    Image originalLogo = Image.FromFile(logoPath);
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


                // Create Model button
                Button btnProfile = new Button();
                btnProfile.Name = "btnProfile";
                btnProfile.Text = "  Profile";
                btnProfile.TextAlign = ContentAlignment.MiddleLeft;
                btnProfile.ImageAlign = ContentAlignment.MiddleLeft;
                btnProfile.TextImageRelation = TextImageRelation.ImageBeforeText;
                btnProfile.FlatStyle = FlatStyle.Flat;
                btnProfile.FlatAppearance.BorderSize = 0;
                btnProfile.BackColor = Color.Transparent;
                btnProfile.ForeColor = Color.White;
                btnProfile.Font = new Font("Segoe UI", 13, FontStyle.Bold);

                // Size and positioning
                btnProfile.Size = new Size(panel1.Width, 50);   // Full width
                btnProfile.Location = new Point(0, 170);         // Flush left
                btnProfile.Margin = new Padding(10);
                btnProfile.Padding = new Padding(10, 0, 0, 0);  // Space between icon and text

                // Load and resize icon
                string iconPath = Application.StartupPath + @"\asset\icon\model.png";
                if (File.Exists(iconPath))
                {
                    Image originalIcon = Image.FromFile(iconPath);
                    Image resizedIcon = new Bitmap(originalIcon, new Size(35, 35));
                    btnProfile.Image = resizedIcon;
                }

                // Hover effect
                btnProfile.MouseEnter += (s, args) =>
                {
                    btnProfile.BackColor = Color.FromArgb(100, 65, 65, 100);  // Hover color
                };

                btnProfile.MouseLeave += (s, args) =>
                {
                    btnProfile.BackColor = Color.Transparent;
                };

                btnProfile.Click += (s, args) =>
                {
                    // Create and show the About form
                    profile profileForm = new profile();
                    profileForm.ShowDialog(); // Show as profile dialog
                                           
                };

                // Add to panel
                panel1.Controls.Add(btnProfile);



                // Create Buy button
                Button btnBuy = new Button();
                btnBuy.Name = "btnBuy";
                btnBuy.Text = "  Buy";
                btnBuy.TextAlign = ContentAlignment.MiddleLeft;
                btnBuy.ImageAlign = ContentAlignment.MiddleLeft;
                btnBuy.TextImageRelation = TextImageRelation.ImageBeforeText;
                btnBuy.FlatStyle = FlatStyle.Flat;
                btnBuy.FlatAppearance.BorderSize = 0;
                btnBuy.BackColor = Color.Transparent;
                btnBuy.ForeColor = Color.White;
                btnBuy.Font = new Font("Segoe UI", 13, FontStyle.Bold);

                // Size and positioning
                btnBuy.Size = new Size(panel1.Width, 50);
                btnBuy.Location = new Point(0, 230); // Below Dashboard (170 + 50 + 10 spacing)
                btnBuy.Margin = new Padding(10);
                btnBuy.Padding = new Padding(10, 0, 0, 0);

                // Load and resize icon
                string buyIconPath = Application.StartupPath + @"\asset\bg\d.jpg";
                if (File.Exists(buyIconPath))
                {
                    Image originalBuyIcon = Image.FromFile(buyIconPath);
                    Image resizedBuyIcon = new Bitmap(originalBuyIcon, new Size(35, 35));
                    btnBuy.Image = resizedBuyIcon;
                }

                // Hover effect
                btnBuy.MouseEnter += (s, args) =>
                {
                    btnBuy.BackColor = Color.FromArgb(255, 10, 30, 32);
                };

                btnBuy.MouseLeave += (s, args) =>
                {
                    btnBuy.BackColor = Color.Transparent;
                };
                btnBuy.Click += (s, args) =>
                {
                    payment payForm = new payment();
                    payForm.StartPosition = FormStartPosition.CenterScreen;
                    payForm.ShowDialog();
                };

                // Add to panel
                panel1.Controls.Add(btnBuy);


                // Create Order button
                Button btnOrder = new Button();
                btnOrder.Name = "btnOrder";
                btnOrder.Text = "  Order";
                btnOrder.TextAlign = ContentAlignment.MiddleLeft;
                btnOrder.ImageAlign = ContentAlignment.MiddleLeft;
                btnOrder.TextImageRelation = TextImageRelation.ImageBeforeText;
                btnOrder.FlatStyle = FlatStyle.Flat;
                btnOrder.FlatAppearance.BorderSize = 0;
                btnOrder.BackColor = Color.Transparent;
                btnOrder.ForeColor = Color.White;
                btnOrder.Font = new Font("Segoe UI", 13, FontStyle.Bold);

                // Size and positioning
                btnOrder.Size = new Size(panel1.Width, 50);
                btnOrder.Location = new Point(0, 290); // 10 pixels below Buy button
                btnOrder.Margin = new Padding(10);
                btnOrder.Padding = new Padding(10, 0, 0, 0);

                // Load and resize icon
                string orderIconPath = Application.StartupPath + @"\asset\icon\order.png"; // Adjust path as needed
                if (File.Exists(orderIconPath))
                {
                    Image originalOrderIcon = Image.FromFile(orderIconPath);
                    Image resizedOrderIcon = new Bitmap(originalOrderIcon, new Size(35, 35));
                    btnOrder.Image = resizedOrderIcon;
                }

                // Hover effect with different color
                btnOrder.MouseEnter += (s, args) =>
                {
                    btnOrder.BackColor = Color.FromArgb(255, 30, 30, 90); // Dark blue hover
                };

                btnOrder.MouseLeave += (s, args) =>
                {
                    btnOrder.BackColor = Color.Transparent;
                };

                btnOrder.Click += (s, e) =>
                {
                    order orderForm = new order();  // Use the login form from your carstore namespace
                    orderForm.ShowDialog();         // Show it as a modal dialog
                };

                // Add to panel
                panel1.Controls.Add(btnOrder);



                // Create Login/Signup button
                Button btnLoginSignup = new Button();
                btnLoginSignup.Name = "btnLoginSignup";
                btnLoginSignup.Text = "  Login/Signup";
                btnLoginSignup.TextAlign = ContentAlignment.MiddleLeft;
                btnLoginSignup.ImageAlign = ContentAlignment.MiddleLeft;
                btnLoginSignup.TextImageRelation = TextImageRelation.ImageBeforeText;
                btnLoginSignup.FlatStyle = FlatStyle.Flat;
                btnLoginSignup.FlatAppearance.BorderSize = 0;
                btnLoginSignup.BackColor = Color.Transparent;
                btnLoginSignup.ForeColor = Color.White;
                btnLoginSignup.Font = new Font("Segoe UI", 13, FontStyle.Bold);

                // Size and positioning
                btnLoginSignup.Size = new Size(panel1.Width, 50);
                btnLoginSignup.Location = new Point(0, 700); // Below Buy (230 + 50 + 10 spacing)
                btnLoginSignup.Margin = new Padding(10);
                btnLoginSignup.Padding = new Padding(10, 0, 0, 0);

                // Load and resize icon
                string loginIconPath = Application.StartupPath + @"\asset\icon\login.png";
                if (File.Exists(loginIconPath))
                {
                    Image originalLoginIcon = Image.FromFile(loginIconPath);
                    Image resizedLoginIcon = new Bitmap(originalLoginIcon, new Size(35, 35));
                    btnLoginSignup.Image = resizedLoginIcon;
                }

                // Hover effect
                btnLoginSignup.MouseEnter += (s, args) =>
                {
                    btnLoginSignup.BackColor = Color.FromArgb(255, 30, 50, 70);
                };

                btnLoginSignup.MouseLeave += (s, args) =>
                {
                    btnLoginSignup.BackColor = Color.Transparent;
                };

                btnLoginSignup.Click += (s, e) =>
                {
                    login loginForm = new login();  // Use the login form from your carstore namespace
                    loginForm.ShowDialog();         // Show it as a modal dialog
                };


                // Add to panel
                panel1.Controls.Add(btnLoginSignup);


                // Create About button

                Button btnAbout = new Button();
                btnAbout.Name = "btnAbout";
                btnAbout.Text = "  About";
                btnAbout.TextAlign = ContentAlignment.MiddleLeft;
                btnAbout.ImageAlign = ContentAlignment.MiddleLeft;
                btnAbout.TextImageRelation = TextImageRelation.ImageBeforeText;
                btnAbout.FlatStyle = FlatStyle.Flat;
                btnAbout.FlatAppearance.BorderSize = 0;
                btnAbout.BackColor = Color.Transparent;
                btnAbout.ForeColor = Color.White;
                btnAbout.Font = new Font("Segoe UI", 13, FontStyle.Bold);

                // Position below Contact
                btnAbout.Size = new Size(panel1.Width, 50);
                btnAbout.Location = new Point(0, 760);
                btnAbout.Margin = new Padding(10);
                btnAbout.Padding = new Padding(10, 0, 0, 0);

                // Icon
                string aboutIconPath = Application.StartupPath + @"\asset\icon\about.png"; // Adjust as needed
                if (File.Exists(aboutIconPath))
                {
                    Image original = Image.FromFile(aboutIconPath);
                    btnAbout.Image = new Bitmap(original, new Size(35, 35));
                }

                // Hover color: Dark violet
                btnAbout.MouseEnter += (s, args) =>
                {
                    btnAbout.BackColor = Color.FromArgb(255, 60, 30, 60);
                };
                btnAbout.MouseLeave += (s, args) =>
                {
                    btnAbout.BackColor = Color.Transparent;
                };
                btnAbout.Click += (s, args) =>
                {
                    // Create and show the About form
                    about aboutForm = new about();
                    aboutForm.ShowDialog(); // Show as modal dialog
                                            // OR use Show() for non-modal:
                                            // aboutForm.Show();
                };


                panel1.Controls.Add(btnAbout);

                // Create Settings button
                Button btnSettings = new Button();
                btnSettings.Name = "btnSettings";
                btnSettings.Text = "  Settings";
                btnSettings.TextAlign = ContentAlignment.MiddleLeft;
                btnSettings.ImageAlign = ContentAlignment.MiddleLeft;
                btnSettings.TextImageRelation = TextImageRelation.ImageBeforeText;
                btnSettings.FlatStyle = FlatStyle.Flat;
                btnSettings.FlatAppearance.BorderSize = 0;
                btnSettings.BackColor = Color.Transparent;
                btnSettings.ForeColor = Color.White;
                btnSettings.Font = new Font("Segoe UI", 13, FontStyle.Bold);

                // Position below Inventory
                btnSettings.Size = new Size(panel1.Width, 50);
                btnSettings.Location = new Point(0, 820);
                btnSettings.Margin = new Padding(10);
                btnSettings.Padding = new Padding(10, 0, 0, 0);

                // Icon
                string settingsIconPath = Application.StartupPath + @"\asset\icon\setting.png"; // Adjust as needed
                if (File.Exists(settingsIconPath))
                {
                    Image original = Image.FromFile(settingsIconPath);
                    btnSettings.Image = new Bitmap(original, new Size(35, 35));
                }

                // Hover color: Dark gray-blue
                btnSettings.MouseEnter += (s, args) =>
                {
                    btnSettings.BackColor = Color.FromArgb(255, 40, 40, 80);
                };
                btnSettings.MouseLeave += (s, args) =>
                {
                    btnSettings.BackColor = Color.Transparent;
                };

                btnSettings.Click += (s, args) =>
                {
                    // Create and show the Settings form
                    setting settingsForm = new setting();
                    settingsForm.ShowDialog(); // Show as a modal dialog
                                               // OR use: settingsForm.Show(); // for non-modal window
                };


                panel1.Controls.Add(btnSettings);


                // Create Contact Us button
                Button btnContact = new Button();
                btnContact.Name = "btnContact";
                btnContact.Text = "  Contact Us";
                btnContact.TextAlign = ContentAlignment.MiddleLeft;
                btnContact.ImageAlign = ContentAlignment.MiddleLeft;
                btnContact.TextImageRelation = TextImageRelation.ImageBeforeText;
                btnContact.FlatStyle = FlatStyle.Flat;
                btnContact.FlatAppearance.BorderSize = 0;
                btnContact.BackColor = Color.Transparent;
                btnContact.ForeColor = Color.White;
                btnContact.Font = new Font("Segoe UI", 13, FontStyle.Bold);

                // Position below Settings
                btnContact.Size = new Size(panel1.Width, 50);
                btnContact.Location = new Point(0, 880);
                btnContact.Margin = new Padding(10);
                btnContact.Padding = new Padding(10, 0, 0, 0);

                // Icon
                string contactIconPath = Application.StartupPath + @"\asset\icon\contact.png"; // Adjust as needed
                if (File.Exists(contactIconPath))
                {
                    Image original = Image.FromFile(contactIconPath);
                    btnContact.Image = new Bitmap(original, new Size(35, 35));
                }

                // Hover color: Deep greenish-blue
                btnContact.MouseEnter += (s, args) =>
                {
                    btnContact.BackColor = Color.FromArgb(255, 30, 60, 60);
                };
                btnContact.MouseLeave += (s, args) =>
                {
                    btnContact.BackColor = Color.Transparent;
                };
                btnContact.Click += (s, e) =>
                {
                    MessageBox.Show("Please contact us support@carstore.com ", "Contact");
                };

                panel1.Controls.Add(btnContact);



            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Ensure the path is correct and points to your asset folder
            string imagePath = Application.StartupPath + @"\asset\models\car1.png";

            // Check if the file exists
            if (System.IO.File.Exists(imagePath))
            {
                // Load the image
                Image image = Image.FromFile(imagePath);

                // Set the PictureBox image
                pictureBox1.Image = image;
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox1.BackColor = Color.Transparent; // Transparent background

                // Set the initial size (1000x550)
                int initialWidth = 1000;
                int initialHeight = 550;

                // Increase the size (20% larger)
                int increasedWidth = (int)(initialWidth * 1.2);
                int increasedHeight = (int)(initialHeight * 1.2);

                // Apply the increased size
                pictureBox1.Size = new Size(increasedWidth, increasedHeight);

                // Center the PictureBox between the panels (panel1 as left, panel2 as right)
                int leftPanelWidth = panel1.Width;   // Adjust this to your left panel
                int rightPanelWidth = panel1.Width;  // Adjust this to your right panel

                int availableWidth = this.ClientSize.Width - (leftPanelWidth + rightPanelWidth);
                int availableHeight = this.ClientSize.Height;

                // Centering the PictureBox
                int offset = 65; // Adjust this value to move more/less to the right
                pictureBox1.Left = leftPanelWidth + (availableWidth - pictureBox1.Width) / 2 + offset;
                pictureBox1.Top = (availableHeight - pictureBox1.Height) / 2;
            }
            else
            {
                MessageBox.Show("Image not found. Please check the path.");
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            if (panel2.Controls.Find("modelItem", false).Length == 0)
            {
                Panel modelItem = new Panel();
                modelItem.Name = "modelItem";
                modelItem.Size = new Size(480, 50);
                modelItem.BackColor = Color.FromArgb(100, 60, 60, 80);
                modelItem.Cursor = Cursors.Hand;
                modelItem.Location = new Point(10, 5);

                string imagePath = Application.StartupPath + @"\asset\models\hilux.png"; // Changed to Hilux image

                EventHandler clickHandler = (s, args) =>
                {
                    if (File.Exists(imagePath))
                    {
                        Image image = Image.FromFile(imagePath);
                        pictureBox1.Image = image;
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBox1.BackColor = Color.Transparent;
                        pictureBox1.Size = new Size(1300, 600);
                        pictureBox1.Left = 300;
                        pictureBox1.Top = 300;

                        // Update panel10 with Toyota Hilux information
                        UpdateCarInfo(
                            "Toyota Hilux 2023",
                            "$3,850,000 ETB",
                            "Model:         Toyota Hilux 4x4 Diesel\n" +
                            "Engine:        2.8L Turbo Diesel\n" +
                            "Horsepower:    201 HP @ 3,400 RPM\n" +
                            "Torque:        369 lb-ft @ 1,600-2,800 RPM\n" +
                            "Transmission:  6-speed Automatic\n" +
                            "Drive Type:    Four-wheel drive\n" +
                            "Payload:       1,000 kg\n" +
                            "Towing:        3,500 kg"
                        );
                    }
                    else
                    {
                        MessageBox.Show("Image not found. Please check the path.");
                    }
                };

                if (File.Exists(imagePath))
                {
                    PictureBox pic = new PictureBox();
                    pic.Image = new Bitmap(Image.FromFile(imagePath), new Size(50, 40));
                    pic.SizeMode = PictureBoxSizeMode.Zoom;
                    pic.Size = new Size(55, 45);
                    pic.Location = new Point(5, 5);
                    pic.BackColor = Color.Transparent;
                    pic.Click += clickHandler;
                    modelItem.Controls.Add(pic);
                }

                Label lblModel = new Label();
                lblModel.Text = "  Toyota Hilux"; // Changed to Hilux
                lblModel.ForeColor = Color.White;
                lblModel.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                lblModel.AutoSize = true;
                lblModel.Location = new Point(65, 15);
                lblModel.BackColor = Color.Transparent;
                lblModel.Click += clickHandler;

                lblModel.MouseEnter += (s, args) =>
                {
                    lblModel.ForeColor = Color.FromArgb(255, 50, 100);
                };

                lblModel.MouseLeave += (s, args) =>
                {
                    lblModel.ForeColor = Color.White;
                };

                modelItem.Controls.Add(lblModel);

                Label lblPrice = new Label();
                lblPrice.Text = "        $3,850,000 ETB"; // Same price format
                lblPrice.ForeColor = Color.Green;
                lblPrice.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                lblPrice.AutoSize = true;
                lblPrice.Location = new Point(lblModel.Right, 15);
                lblPrice.BackColor = Color.Transparent;
                lblPrice.Click += clickHandler;
                modelItem.Controls.Add(lblPrice);

                modelItem.Click += clickHandler;
                panel2.Controls.Add(modelItem);
            }
        }



        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            if (panel3.Controls.Find("modelItem3", false).Length == 0)
            {
                Panel modelItem3 = new Panel();
                modelItem3.Name = "modelItem3";
                modelItem3.Size = new Size(480, 50);
                modelItem3.BackColor = Color.FromArgb(100, 60, 60, 80);
                modelItem3.Cursor = Cursors.Hand;
                modelItem3.Location = new Point(10, 5);

                EventHandler clickHandler = (s, args) =>
                {
                    string newImagePath = Application.StartupPath + @"\asset\models\lc1.png";
                    if (File.Exists(newImagePath))
                    {
                        Image image = Image.FromFile(newImagePath);
                        pictureBox1.Image = image;
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBox1.BackColor = Color.Transparent;
                        pictureBox1.Size = new Size(1300, 700);
                        pictureBox1.Left = 300;
                        pictureBox1.Top = 300;

                        // Update panel10 with Land Cruiser information
                        UpdateCarInfo(
                            "Toyota Land Cruiser 2023",
                            "$8,500,000 ETB",
                            "Model:         Toyota Land Cruiser V8\n" +
                            "Engine:        5.7L V8 Gasoline\n" +
                            "Horsepower:    381 HP @ 5,600 RPM\n" +
                            "Torque:        401 lb-ft @ 3,600 RPM\n" +
                            "Transmission:  8-speed Automatic\n" +
                            "Drive Type:    Full-time 4WD\n" +
                            "Seating:       8 passengers\n" +
                            "Towing:        8,100 lbs\n" +
                            "Fuel Economy:  13 MPG city / 18 MPG highway"
                        );
                    }
                    else
                    {
                        MessageBox.Show("Image not found. Please check the path.");
                    }
                };

                // Load image
                string imagePath = Application.StartupPath + @"\asset\models\lc1.png";
                if (File.Exists(imagePath))
                {
                    PictureBox pic = new PictureBox();
                    pic.Image = new Bitmap(Image.FromFile(imagePath), new Size(50, 40));
                    pic.SizeMode = PictureBoxSizeMode.Zoom;
                    pic.Size = new Size(55, 45);
                    pic.Location = new Point(5, 5);
                    pic.BackColor = Color.Transparent;
                    pic.Click += clickHandler;
                    modelItem3.Controls.Add(pic);
                }

                // Model label
                Label lblModel = new Label();
                lblModel.Text = "  Land Cruiser"; // Updated model name
                lblModel.ForeColor = Color.White;
                lblModel.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                lblModel.AutoSize = true;
                lblModel.Location = new Point(65, 15);
                lblModel.BackColor = Color.Transparent;
                lblModel.Click += clickHandler;

                lblModel.MouseEnter += (s, args) =>
                {
                    lblModel.ForeColor = Color.FromArgb(255, 50, 100);
                };

                lblModel.MouseLeave += (s, args) =>
                {
                    lblModel.ForeColor = Color.White;
                };

                modelItem3.Controls.Add(lblModel);

                // Price label (updated for Land Cruiser)
                Label lblPrice = new Label();
                lblPrice.Text = "       $8,500,000 ETB"; // Updated price
                lblPrice.ForeColor = Color.Green;
                lblPrice.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                lblPrice.AutoSize = true;
                lblPrice.Location = new Point(lblModel.Right, 15);
                lblPrice.BackColor = Color.Transparent;
                lblPrice.Click += clickHandler;
                modelItem3.Controls.Add(lblPrice);

                modelItem3.Click += clickHandler;
                panel3.Controls.Add(modelItem3);
            }
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            if (panel4.Controls.Find("modelItem4", false).Length == 0)
            {
                Panel modelItem = new Panel();
                modelItem.Name = "modelItem4";
                modelItem.Size = new Size(480, 50);
                modelItem.BackColor = Color.FromArgb(100, 60, 60, 80);
                modelItem.Cursor = Cursors.Hand;
                modelItem.Location = new Point(10, 5);

                string imagePath = Application.StartupPath + @"\asset\models\ferari.png";

                EventHandler clickHandler = (s, args) =>
                {
                    if (File.Exists(imagePath))
                    {
                        Image image = Image.FromFile(imagePath);
                        pictureBox1.Image = image;
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBox1.BackColor = Color.Transparent;
                        pictureBox1.Size = new Size(1300, 700);
                        pictureBox1.Left = 280;
                        pictureBox1.Top = 200;
                    }
                    else
                    {
                        MessageBox.Show("Image not found. Please check the path.");
                    }
                };

                if (File.Exists(imagePath))
                {
                    PictureBox pic = new PictureBox();
                    pic.Image = new Bitmap(Image.FromFile(imagePath), new Size(50, 40));
                    pic.SizeMode = PictureBoxSizeMode.Zoom;
                    pic.Size = new Size(55, 45);
                    pic.Location = new Point(5, 5);
                    pic.BackColor = Color.Transparent;
                    pic.Click += clickHandler;
                    modelItem.Controls.Add(pic);
                }

                // Model label
                Label lblModel = new Label();
                lblModel.Text = "  Ferrari  ";
                lblModel.ForeColor = Color.White;
                lblModel.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                lblModel.AutoSize = true;
                lblModel.Location = new Point(65, 15);
                lblModel.BackColor = Color.Transparent;
                lblModel.Click += clickHandler;

                // Hover effect for the model name label
                lblModel.MouseEnter += (s, args) =>
                {
                    lblModel.ForeColor = Color.FromArgb(255, 50, 100); // Change model name color on hover
                };

                lblModel.MouseLeave += (s, args) =>
                {
                    lblModel.ForeColor = Color.White; // Reset model name color
                };

                modelItem.Controls.Add(lblModel);

                // Price label
                Label lblPrice = new Label();
                lblPrice.Text = "                  $1,747,555 ETB";
                lblPrice.ForeColor = Color.Green;
                lblPrice.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                lblPrice.AutoSize = true;
                lblPrice.Location = new Point(lblModel.Right, 15);
                lblPrice.BackColor = Color.Transparent;
                lblPrice.Click += clickHandler;
                modelItem.Controls.Add(lblPrice);

                // Assign the click event to the main panel too
                modelItem.Click += clickHandler;

                panel4.Controls.Add(modelItem);

            }
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {
            if (panel5.Controls.Find("modelItem5", false).Length == 0)
            {
                Panel modelItem = new Panel();
                modelItem.Name = "modelItem5";
                modelItem.Size = new Size(480, 50);
                modelItem.BackColor = Color.FromArgb(100, 60, 60, 80);
                modelItem.Cursor = Cursors.Hand;
                modelItem.Location = new Point(10, 5);

                string imagePath = Application.StartupPath + @"\asset\models\lambsvj.png";

                EventHandler clickHandler = (s, args) =>
                {
                    if (File.Exists(imagePath))
                    {
                        // Load and display the car image
                        Image image = Image.FromFile(imagePath);
                        pictureBox1.Image = image;
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBox1.BackColor = Color.Transparent;
                        pictureBox1.Size = new Size(1100, 500);
                        pictureBox1.Left = 400;
                        pictureBox1.Top = 350;

                        // Update panel10 with Lamborghini information (formatted exactly like BMW)
                        UpdateCarInfo(
                            "Lamborghini Aventador SVJ",
                            "$3,500,000 ETB",
                            "Model:         Lamborghini Aventador SVJ\n" +
                            "Engine:        6.5L V12\n" +
                            "Horsepower:    759 HP @ 8,500 RPM\n" +
                            "Torque:        531 lb-ft @ 6,750 RPM\n" +
                            "0-60 mph:      2.8 seconds\n" +
                            "Top Speed:     217 mph\n" +
                            "Transmission:  7-speed ISR\n" +
                            "Drivetrain:    All-wheel drive"
                        );
                    }
                    else
                    {
                        MessageBox.Show("Image not found. Please check the path.");
                    }
                };

                if (File.Exists(imagePath))
                {
                    PictureBox pic = new PictureBox();
                    pic.Image = new Bitmap(Image.FromFile(imagePath), new Size(50, 40));
                    pic.SizeMode = PictureBoxSizeMode.Zoom;
                    pic.Size = new Size(55, 45);
                    pic.Location = new Point(5, 5);
                    pic.BackColor = Color.Transparent;
                    pic.Click += clickHandler;
                    modelItem.Controls.Add(pic);
                }

                Label lblModel = new Label();
                lblModel.Text = "  Lamborghini      ";
                lblModel.ForeColor = Color.White;
                lblModel.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                lblModel.AutoSize = true;
                lblModel.Location = new Point(65, 15);
                lblModel.BackColor = Color.Transparent;
                lblModel.Click += clickHandler;

                lblModel.MouseEnter += (s, args) =>
                {
                    lblModel.ForeColor = Color.FromArgb(255, 50, 100);
                };

                lblModel.MouseLeave += (s, args) =>
                {
                    lblModel.ForeColor = Color.White;
                };

                modelItem.Controls.Add(lblModel);

                Label lblPrice = new Label();
                lblPrice.Text = "  $3,500,000 ETB";
                lblPrice.ForeColor = Color.Green;
                lblPrice.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                lblPrice.AutoSize = true;
                lblPrice.Location = new Point(lblModel.Right, 15);
                lblPrice.BackColor = Color.Transparent;
                lblPrice.Click += clickHandler;
                modelItem.Controls.Add(lblPrice);

                modelItem.Click += clickHandler;
                panel5.Controls.Add(modelItem);
            }
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {
            if (panel6.Controls.Find("modelItem6", false).Length == 0)
            {
                Panel modelItem = new Panel();
                modelItem.Name = "modelItem6";
                modelItem.Size = new Size(480, 50);
                modelItem.BackColor = Color.FromArgb(100, 60, 60, 80);
                modelItem.Cursor = Cursors.Hand;
                modelItem.Location = new Point(10, 5);

                string imagePath = Application.StartupPath + @"\asset\models\car4.png";

                EventHandler clickHandler = (s, args) =>
                {
                    if (File.Exists(imagePath))
                    {
                        Image image = Image.FromFile(imagePath);
                        pictureBox1.Image = image;
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBox1.BackColor = Color.Transparent;
                        pictureBox1.Size = new Size(1500, 900);
                        pictureBox1.Left = 300;
                        pictureBox1.Top = 100;
                    }
                    else
                    {
                        MessageBox.Show("Image not found. Please check the path.");
                    }
                };

                if (File.Exists(imagePath))
                {
                    PictureBox pic = new PictureBox();
                    pic.Image = new Bitmap(Image.FromFile(imagePath), new Size(50, 40));
                    pic.SizeMode = PictureBoxSizeMode.Zoom;
                    pic.Size = new Size(55, 45);
                    pic.Location = new Point(5, 5);
                    pic.BackColor = Color.Transparent;
                    pic.Click += clickHandler;
                    modelItem.Controls.Add(pic);
                }

                Label lblModel = new Label();
                lblModel.Text = "  Tesla Model S  ";
                lblModel.ForeColor = Color.White;
                lblModel.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                lblModel.AutoSize = true;
                lblModel.Location = new Point(65, 15);
                lblModel.BackColor = Color.Transparent;
                lblModel.Click += clickHandler;

                lblModel.MouseEnter += (s, args) =>
                {
                    lblModel.ForeColor = Color.FromArgb(255, 50, 100);
                };

                lblModel.MouseLeave += (s, args) =>
                {
                    lblModel.ForeColor = Color.White;
                };

                modelItem.Controls.Add(lblModel);

                Label lblPrice = new Label();
                lblPrice.Text = "      $1,200,000 ETB";
                lblPrice.ForeColor = Color.Green;
                lblPrice.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                lblPrice.AutoSize = true;
                lblPrice.Location = new Point(lblModel.Right, 15);
                lblPrice.BackColor = Color.Transparent;
                lblPrice.Click += clickHandler;
                modelItem.Controls.Add(lblPrice);

                modelItem.Click += clickHandler;
                panel6.Controls.Add(modelItem);
            }
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {
            if (panel7.Controls.Find("modelItem7", false).Length == 0)
            {
                Panel modelItem = new Panel();
                modelItem.Name = "modelItem7";
                modelItem.Size = new Size(480, 50);
                modelItem.BackColor = Color.FromArgb(100, 60, 60, 80);
                modelItem.Cursor = Cursors.Hand;
                modelItem.Location = new Point(10, 5);

                string imagePath = Application.StartupPath + @"\asset\models\dodge.png";

                EventHandler clickHandler = (s, args) =>
                {
                    if (File.Exists(imagePath))
                    {
                        Image image = Image.FromFile(imagePath);
                        pictureBox1.Image = image;
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBox1.BackColor = Color.Transparent;
                        pictureBox1.Size = new Size(1500, 900);
                        pictureBox1.Left = 300;
                        pictureBox1.Top = 100;
                    }
                    else
                    {
                        MessageBox.Show("Image not found. Please check the path.");
                    }
                };

                if (File.Exists(imagePath))
                {
                    PictureBox pic = new PictureBox();
                    pic.Image = new Bitmap(Image.FromFile(imagePath), new Size(50, 40));
                    pic.SizeMode = PictureBoxSizeMode.Zoom;
                    pic.Size = new Size(55, 45);
                    pic.Location = new Point(5, 5);
                    pic.BackColor = Color.Transparent;
                    pic.Click += clickHandler;
                    modelItem.Controls.Add(pic);
                }

                Label lblModel = new Label();
                lblModel.Text = "  Porsche 911    ";
                lblModel.ForeColor = Color.White;
                lblModel.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                lblModel.AutoSize = true;
                lblModel.Location = new Point(65, 15);
                lblModel.BackColor = Color.Transparent;
                lblModel.Click += clickHandler;

                lblModel.MouseEnter += (s, args) =>
                {
                    lblModel.ForeColor = Color.FromArgb(255, 50, 100);
                };

                lblModel.MouseLeave += (s, args) =>
                {
                    lblModel.ForeColor = Color.White;
                };

                modelItem.Controls.Add(lblModel);

                Label lblPrice = new Label();
                lblPrice.Text = "       $2,800,000 ETB";
                lblPrice.ForeColor = Color.Green;
                lblPrice.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                lblPrice.AutoSize = true;
                lblPrice.Location = new Point(lblModel.Right, 15);
                lblPrice.BackColor = Color.Transparent;
                lblPrice.Click += clickHandler;
                modelItem.Controls.Add(lblPrice);

                modelItem.Click += clickHandler;
                panel7.Controls.Add(modelItem);
            }
        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {
            if (panel8.Controls.Find("modelItem8", false).Length == 0)
            {
                Panel modelItem = new Panel();
                modelItem.Name = "modelItem8";
                modelItem.Size = new Size(480, 50);
                modelItem.BackColor = Color.FromArgb(100, 60, 60, 80);
                modelItem.Cursor = Cursors.Hand;
                modelItem.Location = new Point(10, 5);

                string imagePath = Application.StartupPath + @"\asset\models\rr.png";

                EventHandler clickHandler = (s, args) =>
                {
                    if (File.Exists(imagePath))
                    {
                        Image image = Image.FromFile(imagePath);
                        pictureBox1.Image = image;
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBox1.BackColor = Color.Transparent;
                        pictureBox1.Size = new Size(1300, 700);
                        pictureBox1.Left = 300;
                        pictureBox1.Top = 250;

                        // Update panel10 with Range Rover information
                        UpdateCarInfo(
                            "Range Rover Autobiography",
                            "$3,200,000 ETB",
                            "Model:         Range Rover Autobiography\n" +
                            "Engine:        4.4L Twin-Turbo V8\n" +
                            "Horsepower:    523 HP @ 5,500-6,000 RPM\n" +
                            "Torque:        553 lb-ft @ 1,800-4,600 RPM\n" +
                            "0-60 mph:      4.6 seconds\n" +
                            "Top Speed:     155 mph (limited)\n" +
                            "Transmission:  8-speed Automatic\n" +
                            "Drive Type:    All-wheel drive\n" +
                            "Seating:       5 passengers\n" +
                            "Wading Depth:  35.4 inches"
                        );
                    }
                    else
                    {
                        MessageBox.Show("Image not found. Please check the path.");
                    }
                };

                if (File.Exists(imagePath))
                {
                    PictureBox pic = new PictureBox();
                    pic.Image = new Bitmap(Image.FromFile(imagePath), new Size(50, 40));
                    pic.SizeMode = PictureBoxSizeMode.Zoom;
                    pic.Size = new Size(55, 45);
                    pic.Location = new Point(5, 5);
                    pic.BackColor = Color.Transparent;
                    pic.Click += clickHandler;
                    modelItem.Controls.Add(pic);
                }

                Label lblModel = new Label();
                lblModel.Text = "  Range Rover      ";
                lblModel.ForeColor = Color.White;
                lblModel.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                lblModel.AutoSize = true;
                lblModel.Location = new Point(65, 15);
                lblModel.BackColor = Color.Transparent;
                lblModel.Click += clickHandler;

                lblModel.MouseEnter += (s, args) =>
                {
                    lblModel.ForeColor = Color.FromArgb(255, 50, 100);
                };

                lblModel.MouseLeave += (s, args) =>
                {
                    lblModel.ForeColor = Color.White;
                };

                modelItem.Controls.Add(lblModel);

                Label lblPrice = new Label();
                lblPrice.Text = "  $3,200,000 ETB";
                lblPrice.ForeColor = Color.Green;
                lblPrice.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                lblPrice.AutoSize = true;
                lblPrice.Location = new Point(lblModel.Right, 15);
                lblPrice.BackColor = Color.Transparent;
                lblPrice.Click += clickHandler;
                modelItem.Controls.Add(lblPrice);

                modelItem.Click += clickHandler;
                panel8.Controls.Add(modelItem);
            }
        }

        private void panel9_Paint(object sender, PaintEventArgs e)
        {
            if (panel9.Controls.Find("modelItem9", false).Length == 0)
            {
                Panel modelItem = new Panel();
                modelItem.Name = "modelItem9";
                modelItem.Size = new Size(480, 50);
                modelItem.BackColor = Color.FromArgb(100, 60, 60, 80);
                modelItem.Cursor = Cursors.Hand;
                modelItem.Location = new Point(10, 5);

                string imagePath = Application.StartupPath + @"\asset\models\bmw.png";

                EventHandler clickHandler = (s, args) =>
                {
                    if (File.Exists(imagePath))
                    {
                        // Load and display the car image
                        Image image = Image.FromFile(imagePath);
                        pictureBox1.Image = image;
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBox1.BackColor = Color.Transparent;
                        pictureBox1.Size = new Size(1250, 930);
                        pictureBox1.Left = 300;
                        pictureBox1.Top = 100;

                        // Update panel10 with BMW M5 information
                        UpdateCarInfo(
    "BMW M5 Competition 2023",
    "$2,500,000 ETB",
    "Model:         BMW M5 Competition 2023\n" +
    "Engine:        4.4L Twin-Turbo V8\n" +
    "Horsepower:    617 HP @ 6,000 RPM\n" +
    "Torque:        553 lb-ft @ 1,800–5,800 RPM\n" +
    "0-60 mph:      3.1 seconds\n" +
    "Top Speed:     190 mph (electronically limited)\n" +
    "Transmission:  8-speed M Steptronic\n" +
    "Drivetrain:    M xDrive AWD"
);

                    }
                    else
                    {
                        MessageBox.Show("Image not found. Please check the path.");
                    }
                };

                if (File.Exists(imagePath))
                {
                    PictureBox pic = new PictureBox();
                    pic.Image = new Bitmap(Image.FromFile(imagePath), new Size(50, 40));
                    pic.SizeMode = PictureBoxSizeMode.Zoom;
                    pic.Size = new Size(55, 45);
                    pic.Location = new Point(5, 5);
                    pic.BackColor = Color.Transparent;
                    pic.Click += clickHandler;
                    modelItem.Controls.Add(pic);
                }

                Label lblModel = new Label();
                lblModel.Text = "  BMW M5  ";
                lblModel.ForeColor = Color.White;
                lblModel.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                lblModel.AutoSize = true;
                lblModel.Location = new Point(65, 15);
                lblModel.BackColor = Color.Transparent;
                lblModel.Click += clickHandler;

                lblModel.MouseEnter += (s, args) =>
                {
                    lblModel.ForeColor = Color.FromArgb(255, 50, 100);
                };

                lblModel.MouseLeave += (s, args) =>
                {
                    lblModel.ForeColor = Color.White;
                };

                modelItem.Controls.Add(lblModel);

                Label lblPrice = new Label();
                lblPrice.Text = "             $2,500,000 ETB";
                lblPrice.ForeColor = Color.Green;
                lblPrice.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                lblPrice.AutoSize = true;
                lblPrice.Location = new Point(lblModel.Right, 15);
                lblPrice.BackColor = Color.Transparent;
                lblPrice.Click += clickHandler;
                modelItem.Controls.Add(lblPrice);

                modelItem.Click += clickHandler;
                panel9.Controls.Add(modelItem);
            }
        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {
            if (panel10.Controls.Count == 0) // Only create controls once
            {
                // Configure panel as fully transparent
                panel10.BackColor = Color.Transparent;
                panel10.AutoScroll = false;

                // Car name label (compact but visible)
                Label lblCarName = new Label();
                lblCarName.Name = "lblCarName";
                lblCarName.Text = "Select a car model";
                lblCarName.Font = new Font("Segoe UI", 14, FontStyle.Bold);
                lblCarName.ForeColor = Color.White;
                lblCarName.BackColor = Color.Transparent;
                lblCarName.AutoSize = true;
                lblCarName.Location = new Point(10, 10);
                panel10.Controls.Add(lblCarName);

                // Price label
                Label lblPrice = new Label();
                lblPrice.Name = "lblPrice";
                lblPrice.Text = "";
                lblPrice.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                lblPrice.ForeColor = Color.LimeGreen;
                lblPrice.BackColor = Color.Transparent;
                lblPrice.AutoSize = true;
                lblPrice.Location = new Point(10, 35);
                panel10.Controls.Add(lblPrice);

                // Compact specifications label
                Label lblSpecs = new Label();
                lblSpecs.Name = "lblSpecs";
                lblSpecs.Text = "";
                lblSpecs.Font = new Font("Segoe UI", 9);
                lblSpecs.ForeColor = Color.White;
                lblSpecs.BackColor = Color.Transparent;
                lblSpecs.AutoSize = false;
                lblSpecs.Size = new Size(panel10.Width - 20, panel10.Height - 50);
                lblSpecs.Location = new Point(10, 60);
                panel10.Controls.Add(lblSpecs);

                // Optional: Add a subtle separator line
                Panel separator = new Panel();
                separator.Height = 1;
                separator.Width = panel10.Width - 20;
                separator.BackColor = Color.FromArgb(50, 255, 255, 255);
                separator.Location = new Point(10, 55);
                panel10.Controls.Add(separator);
            }
        }

        // Add this method to update car info when a car is clicked
        private void UpdateCarInfo(string carName, string price, string specs)
        {
            if (panel10.Controls.Find("lblCarName", false).Length > 0)
            {
                Label lblCarName = (Label)panel10.Controls.Find("lblCarName", false)[0];
                lblCarName.Text = carName;

                Label lblPrice = (Label)panel10.Controls.Find("lblPrice", false)[0];
                lblPrice.Text = price;

                Label lblSpecs = (Label)panel10.Controls.Find("lblSpecs", false)[0];
                lblSpecs.Text = specs;
            }
        }

        // Modify your existing click handlers to update the info panel
        // For example, in your Toyota Corolla click handler:


        private void panel11_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel11_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void panel11_Paint_2(object sender, PaintEventArgs e)
        {
            if (panel11.Controls.Find("modelItem11", false).Length == 0)
            {
                // Create slightly larger panel and move it upward
                Panel modelItem = new Panel();
                modelItem.Name = "modelItem11";
                modelItem.Size = new Size(800, 800); // Increased size
                modelItem.BackColor = Color.Transparent;
                modelItem.Location = new Point(10, 0); // Moved slightly upward

                string imagePath = Path.Combine(Application.StartupPath, @"asset\icon\nextt.png");

                EventHandler clickHandler = (s, args) =>
                {
                    if (File.Exists(imagePath))
                    {
                        Image image = Image.FromFile(imagePath);
                        pictureBox1.Image = image;
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBox1.BackColor = Color.Transparent;
                        pictureBox1.Size = new Size(1500, 900);
                        pictureBox1.Left = 300;
                        pictureBox1.Top = 100;
                    }
                    else
                    {
                        MessageBox.Show("Next icon image not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };

                if (File.Exists(imagePath))
                {
                    // Create slightly larger icon
                    PictureBox pic = new PictureBox();
                    pic.Name = "nextIcon";
                    pic.Image = new Bitmap(Image.FromFile(imagePath), new Size(90, 50)); // Slightly larger
                    pic.SizeMode = PictureBoxSizeMode.Zoom;
                    pic.Size = new Size(150, 90); // Updated size
                    pic.Location = new Point(5, 2); // Adjusted location for alignment
                    pic.BackColor = Color.Transparent;
                    pic.Cursor = Cursors.Hand;
                    pic.Click += clickHandler;

                    Image normalImage = new Bitmap(Image.FromFile(imagePath), new Size(90, 50));
                    Image hoverImage = AdjustBrightness(normalImage, 1.3f);

                    pic.MouseEnter += (s, args) => pic.Image = hoverImage;
                    pic.MouseLeave += (s, args) => pic.Image = normalImage;

                    modelItem.Controls.Add(pic);
                }

                panel11.Controls.Add(modelItem);
            }
        }


        // Helper method to adjust image brightness
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

        private void panel12_Paint(object sender, PaintEventArgs e)
        {
            if (panel12.Controls.Find("modelItem12", false).Length == 0)
            {
                // Create panel
                Panel modelItem = new Panel();
                modelItem.Name = "modelItem12";
                modelItem.Size = new Size(800, 800); // Match size with panel11
                modelItem.BackColor = Color.Transparent;
                modelItem.Location = new Point(10, 0); // Same vertical position as panel11

                string imagePath = Path.Combine(Application.StartupPath, @"asset\icon\previ.png");

                EventHandler clickHandler = (s, args) =>
                {
                    if (File.Exists(imagePath))
                    {
                        Image image = Image.FromFile(imagePath);
                        pictureBox1.Image = image;
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBox1.BackColor = Color.Transparent;
                        pictureBox1.Size = new Size(1500, 900);
                        pictureBox1.Left = 300;
                        pictureBox1.Top = 100;
                    }
                    else
                    {
                        MessageBox.Show("Previous icon image not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };

                if (File.Exists(imagePath))
                {
                    // Create icon for panel12
                    PictureBox pic = new PictureBox();
                    pic.Name = "prevIcon";
                    pic.Image = new Bitmap(Image.FromFile(imagePath), new Size(90, 50));
                    pic.SizeMode = PictureBoxSizeMode.Zoom;
                    pic.Size = new Size(150, 90); // Match size with nextIcon
                    pic.Location = new Point(5, 2);
                    pic.BackColor = Color.Transparent;
                    pic.Cursor = Cursors.Hand;
                    pic.Click += clickHandler;

                    Image normalImage = new Bitmap(Image.FromFile(imagePath), new Size(90, 50));
                    Image hoverImage = AdjustBrightness(normalImage, 1.3f);

                    pic.MouseEnter += (s, args) => pic.Image = hoverImage;
                    pic.MouseLeave += (s, args) => pic.Image = normalImage;

                    modelItem.Controls.Add(pic);
                }

                panel12.Controls.Add(modelItem);
            }
        }

        private void panel13_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel13_Paint_1(object sender, PaintEventArgs e)
        {
            // Prevent duplicate button
            if (panel13.Controls.Find("btnBuyPayment", false).Length == 0)
            {
                Button btnBuyPayment = new Button();
                btnBuyPayment.Name = "btnBuyPayment";
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

                // Size and position with better spacing
                btnBuyPayment.Size = new Size(panel13.Width - 20, 48);
                btnBuyPayment.Location = new Point(10, 10);
                btnBuyPayment.Margin = new Padding(10);

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
                
                string buyIconPath = Application.StartupPath + @"\asset\icon\pay.png";
                if (File.Exists(buyIconPath))
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

                // On click → open payment page
                btnBuyPayment.Click += (s, args) =>
                {
                    payment payForm = new payment();
                    payForm.StartPosition = FormStartPosition.CenterScreen;
                    payForm.ShowDialog();
                };

                panel13.Controls.Add(btnBuyPayment);
            }
        }



    }
}
