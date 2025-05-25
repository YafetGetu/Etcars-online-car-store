using System;
using System.Drawing;
using System.Windows.Forms;

namespace carstore
{
    public partial class order : Form
    {
        // Custom colors and fonts
        private readonly Color primaryColor = Color.FromArgb(0, 122, 204); // Blue accent
        private readonly Color secondaryColor = Color.FromArgb(240, 240, 240); // Light gray
        private readonly Font titleFont = new Font("Segoe UI", 18, FontStyle.Bold);
        private readonly Font labelFont = new Font("Segoe UI", 10, FontStyle.Regular);
        private readonly Font buttonFont = new Font("Segoe UI", 10, FontStyle.Bold);

        public order()
        {
            InitializeComponent();
            InitializeCustomComponents();
            //this.Icon = SystemIcons.Car;
        }

        private void InitializeCustomComponents()
        {
            // Form setup
            this.Text = "Luxury Car Order Form";
            this.Size = new Size(600, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            // Main panel setup
            panel1.Dock = DockStyle.Fill;
            panel1.BackColor = Color.White;
            panel1.Padding = new Padding(20);
            panel1.BorderStyle = BorderStyle.FixedSingle;

            // Title label
            Label titleLabel = new Label();
            titleLabel.Text = "ORDER YOUR DREAM CAR";
            titleLabel.Font = titleFont;
            titleLabel.ForeColor = primaryColor;
            titleLabel.AutoSize = true;
            titleLabel.Location = new Point(120, 20);
            panel1.Controls.Add(titleLabel);

            // Horizontal line under title
            Panel titleLine = new Panel();
            titleLine.BackColor = primaryColor;
            titleLine.Size = new Size(450, 2);
            titleLine.Location = new Point(50, 60);
            panel1.Controls.Add(titleLine);

            // Model selection
            CreateLabel("Car Model:", 50, 80);
            ComboBox modelComboBox = CreateComboBox(200, 80);
            modelComboBox.Items.AddRange(new string[] { "Toyota Hilux", "Land Cruiser", "Ferrari", "Lamborghini", "Tesla Model 5" , "Porche 911" ,"Range Rover" ,"BMW M5" });

            // Color selection
            CreateLabel("Exterior Color:", 50, 130);
            ComboBox colorComboBox = CreateComboBox(200, 130);
            colorComboBox.Items.AddRange(new string[] { "Midnight Black", "Arctic White", "Royal Blue", "Ruby Red", "Silver Metallic", "Emerald Green" });

            // Engine type
            CreateLabel("Engine Type:", 50, 180);
            ComboBox engineComboBox = CreateComboBox(200, 180);
            engineComboBox.Items.AddRange(new string[] { "V6 Twin-Turbo", "V8 Performance", "Hybrid Premium", "Electric Performance", "Diesel Elite" });

            // Transmission
            CreateLabel("Transmission:", 50, 230);
            ComboBox transmissionComboBox = CreateComboBox(200, 230);
            transmissionComboBox.Items.AddRange(new string[] { "8-Speed Automatic", "7-Speed Dual-Clutch", "10-Speed Automatic", "Manual Sport" });

            // Interior package
            CreateLabel("Interior Package:", 50, 280);
            ComboBox interiorComboBox = CreateComboBox(200, 280);
            interiorComboBox.Items.AddRange(new string[] { "Standard Luxury", "Premium Leather", "Executive Lounge", "Sport Carbon" });

            // Additional features
            Label featuresLabel = CreateLabel("Additional Features:", 50, 330);
            featuresLabel.Font = new Font(labelFont, FontStyle.Bold);

            // Feature checkboxes with stylish appearance
            CheckBox leatherSeatsCheck = CreateStyledCheckBox("Nappa Leather Seats (+$50,500 ETB", 200, 330);
            CheckBox sunroofCheck = CreateStyledCheckBox("Panoramic Sunroof (+$20,800ETB)", 200, 360);
            CheckBox navigationCheck = CreateStyledCheckBox("Premium Navigation (+$14,200ETB)", 200, 390);
            CheckBox soundSystemCheck = CreateStyledCheckBox("Bespoke Audio System (+$35,500ETB)", 200, 420);

            // Customer name
            CreateLabel("Your Name:", 50, 460);
            TextBox nameTextBox = new TextBox();
            nameTextBox.Location = new Point(200, 460);
            nameTextBox.Size = new Size(250, 25);
            nameTextBox.Font = labelFont;
            nameTextBox.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(nameTextBox);

            // Order button with gradient background
            Button orderButton = new Button();
            orderButton.Text = "CONFIRM ORDER";
            orderButton.Location = new Point(200, 510);
            orderButton.Size = new Size(150, 45);
            orderButton.Font = buttonFont;
            orderButton.ForeColor = Color.White;
            orderButton.FlatStyle = FlatStyle.Flat;
            orderButton.FlatAppearance.BorderSize = 0;
            orderButton.BackColor = primaryColor;
            orderButton.Cursor = Cursors.Hand;

            // Hover effects for button
            orderButton.MouseEnter += (sender, e) => orderButton.BackColor = Color.FromArgb(0, 96, 160);
            orderButton.MouseLeave += (sender, e) => orderButton.BackColor = primaryColor;

            orderButton.Click += (sender, e) => 
            {
                if (string.IsNullOrWhiteSpace(nameTextBox.Text))
                {
                    ShowError("Please enter your name to proceed with the order.");
                    return;
                }

                if (modelComboBox.SelectedItem == null)
                {
                    ShowError("Please select your desired car model.");
                    return;
                }

                string features = "";
                if (leatherSeatsCheck.Checked) features += "Nappa Leather Seats, ";
                if (sunroofCheck.Checked) features += "Panoramic Sunroof, ";
                if (navigationCheck.Checked) features += "Premium Navigation, ";
                if (soundSystemCheck.Checked) features += "Bespoke Audio System, ";
                if (features.Length > 0) features = features.Remove(features.Length - 2);

                string message = $"THANK YOU, {nameTextBox.Text.ToUpper()}!\n\n" +
                                "YOUR LUXURY CAR ORDER DETAILS:\n\n" +
                                $"▪ Model: {modelComboBox.SelectedItem}\n" +
                                $"▪ Color: {(colorComboBox.SelectedItem ?? "To be selected")}\n" +
                                $"▪ Engine: {(engineComboBox.SelectedItem ?? "To be selected")}\n" +
                                $"▪ Transmission: {(transmissionComboBox.SelectedItem ?? "To be selected")}\n" +
                                $"▪ Interior: {(interiorComboBox.SelectedItem ?? "To be selected")}\n" +
                                $"▪ Features: {(string.IsNullOrEmpty(features) ? "None selected" : features)}\n\n" +
                                "Our sales representative will contact you shortly to finalize your order.";

                ShowConfirmation(message, "Order Confirmation");
            };
            panel1.Controls.Add(orderButton);
        }

        private Label CreateLabel(string text, int x, int y)
        {
            Label label = new Label();
            label.Text = text;
            label.Font = labelFont;
            label.ForeColor = Color.DimGray;
            label.Location = new Point(x, y);
            label.AutoSize = true;
            panel1.Controls.Add(label);
            return label;
        }

        private ComboBox CreateComboBox(int x, int y)
        {
            ComboBox comboBox = new ComboBox();
            comboBox.Location = new Point(x, y);
            comboBox.Size = new Size(250, 25);
            comboBox.Font = labelFont;
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox.FlatStyle = FlatStyle.Flat;
            comboBox.BackColor = secondaryColor;
            panel1.Controls.Add(comboBox);
            return comboBox;
        }

        private CheckBox CreateStyledCheckBox(string text, int x, int y)
        {
            CheckBox checkBox = new CheckBox();
            checkBox.Text = text;
            checkBox.Location = new Point(x, y);
            checkBox.Font = labelFont;
            checkBox.ForeColor = Color.DimGray;
            checkBox.AutoSize = true;
            checkBox.FlatStyle = FlatStyle.Flat;
            panel1.Controls.Add(checkBox);
            return checkBox;
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Order Information Needed", 
                          MessageBoxButtons.OK, 
                          MessageBoxIcon.Exclamation);
        }

        private void ShowConfirmation(string message, string title)
        {
            MessageBox.Show(message, title, 
                          MessageBoxButtons.OK, 
                          MessageBoxIcon.Information);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // Optional: Add custom border or other painting
            ControlPaint.DrawBorder(e.Graphics, panel1.ClientRectangle, 
                                  primaryColor, 1, ButtonBorderStyle.Solid,
                                  primaryColor, 1, ButtonBorderStyle.Solid,
                                  primaryColor, 1, ButtonBorderStyle.Solid,
                                  primaryColor, 1, ButtonBorderStyle.Solid);
        }
    }
}