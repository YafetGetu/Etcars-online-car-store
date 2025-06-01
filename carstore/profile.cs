using System;
using System.Drawing;
using System.Windows.Forms;
using carstore; // Make sure this is included to use the User class and DatabaseConnection

namespace carstore
{
    public partial class profile : Form
    {
        private bool isEditMode = false;
        private Button btnEditSave;
        private TextBox txtName;
        private TextBox txtPhone;
        private TextBox txtEmail;
        private ComboBox cmbGender;

        private User currentUser; // Field to store the passed user data

        // Modify the constructor to accept a User object
        public profile(User user)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(0, 40, 85);

            // Store the passed user object
            currentUser = user;

            // Set up the form
            this.Text = "User Profile";
            this.Size = new Size(650, 600);  // Adjusted form size
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            InitializeProfileComponents();
            LoadUserData(); // Load data from the passed user object
        }

        // Default constructor (keep it for designer compatibility, though not used when opened from Form1)
        public profile()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(0, 40, 85);

            // Set up the form
            this.Text = "User Profile";
            this.Size = new Size(650, 600);  // Adjusted form size
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            InitializeProfileComponents();
            // If this constructor is used, currentUser will be null, LoadUserData will handle this.
            LoadUserData();
        }


        private void InitializeProfileComponents()
        {
            // Main panel setup
            // Ensure panel1 is created by the designer or here if not using designer for panel1
            if (panel1 == null) // Check if panel1 is created by designer
            {
                panel1 = new Panel();
                this.Controls.Add(panel1);
            }

            panel1.BackColor = Color.White;
            panel1.Location = new Point(30, 30);
            panel1.Size = new Size(580, 450);
            panel1.BorderStyle = BorderStyle.FixedSingle;
            // panel1 is added to controls in the constructor

            // Font settings
            Font labelFont = new Font("Segoe UI", 11, FontStyle.Bold);
            Font valueFont = new Font("Segoe UI", 10);
            int labelWidth = 150;  // Fixed width for all labels
            int controlStartX = 180; // Starting X position for controls
            int verticalSpacing = 50; // Space between each field
            int currentY = 40; // Starting Y position

            // Full Name
            Label lblName = new Label();
            lblName.Text = "Full Name:";
            lblName.Font = labelFont;
            lblName.ForeColor = Color.FromArgb(0, 40, 85);
            lblName.Location = new Point(30, currentY);
            lblName.Size = new Size(labelWidth, 25);
            lblName.TextAlign = ContentAlignment.MiddleRight;
            panel1.Controls.Add(lblName);

            txtName = new TextBox();
            txtName.Name = "txtName"; // Give it a name
            txtName.Font = valueFont;
            txtName.Location = new Point(controlStartX, currentY);
            txtName.Size = new Size(350, 26);
            txtName.BorderStyle = BorderStyle.FixedSingle;
            txtName.ReadOnly = true;
            panel1.Controls.Add(txtName);

            currentY += verticalSpacing;

            // Phone Number
            Label lblPhone = new Label();
            lblPhone.Text = "Phone Number:";
            lblPhone.Font = labelFont;
            lblPhone.ForeColor = Color.FromArgb(0, 40, 85);
            lblPhone.Location = new Point(30, currentY);
            lblPhone.Size = new Size(labelWidth, 25);
            lblPhone.TextAlign = ContentAlignment.MiddleRight;
            panel1.Controls.Add(lblPhone);

            txtPhone = new TextBox();
            txtPhone.Name = "txtPhone"; // Give it a name
            txtPhone.Font = valueFont;
            txtPhone.Location = new Point(controlStartX, currentY);
            txtPhone.Size = new Size(350, 26);
            txtPhone.BorderStyle = BorderStyle.FixedSingle;
            txtPhone.ReadOnly = true;
            panel1.Controls.Add(txtPhone);

            currentY += verticalSpacing;

            // Email
            Label lblEmail = new Label();
            lblEmail.Text = "Email Address:";
            lblEmail.Font = labelFont;
            lblEmail.ForeColor = Color.FromArgb(0, 40, 85);
            lblEmail.Location = new Point(30, currentY);
            lblEmail.Size = new Size(labelWidth, 25);
            lblEmail.TextAlign = ContentAlignment.MiddleRight;
            panel1.Controls.Add(lblEmail);

            txtEmail = new TextBox();
            txtEmail.Name = "txtEmail"; // Give it a name
            txtEmail.Font = valueFont;
            txtEmail.Location = new Point(controlStartX, currentY);
            txtEmail.Size = new Size(350, 26);
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtEmail.ReadOnly = true;
            panel1.Controls.Add(txtEmail);

            currentY += verticalSpacing;

            // Gender
            Label lblGender = new Label();
            lblGender.Text = "Gender:";
            lblGender.Font = labelFont;
            lblGender.ForeColor = Color.FromArgb(0, 40, 85);
            lblGender.Location = new Point(30, currentY);
            lblGender.Size = new Size(labelWidth, 25);
            lblGender.TextAlign = ContentAlignment.MiddleRight;
            panel1.Controls.Add(lblGender);

            cmbGender = new ComboBox();
            cmbGender.Name = "cmbGender"; // Give it a name
            cmbGender.Font = valueFont;
            cmbGender.Location = new Point(controlStartX, currentY);
            cmbGender.Size = new Size(150, 26);
            cmbGender.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbGender.Enabled = false;
            cmbGender.Items.AddRange(new object[] { "Male", "Female", "Other", "Prefer not to say" });
            panel1.Controls.Add(cmbGender);

            // Edit/Save button
            btnEditSave = new Button();
            btnEditSave.Name = "btnEditSave"; // Give it a name
            btnEditSave.Text = "Edit";
            btnEditSave.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnEditSave.BackColor = Color.FromArgb(0, 120, 215);
            btnEditSave.ForeColor = Color.White;
            btnEditSave.Size = new Size(120, 35);
            btnEditSave.Location = new Point(
                panel1.Left + (panel1.Width - btnEditSave.Width) / 2,
                panel1.Bottom + 20);
            btnEditSave.Click += BtnEditSave_Click;
            this.Controls.Add(btnEditSave);
        }

        private void LoadUserData()
        {
            if (currentUser != null)
            {
                txtName.Text = currentUser.fullName;
                txtPhone.Text = currentUser.PhoneNumber;
                txtEmail.Text = currentUser.Email;

                // Select the correct gender in the ComboBox
                if (cmbGender.Items.Contains(currentUser.Gender))
                {
                    cmbGender.SelectedItem = currentUser.Gender;
                }
                else
                {
                    cmbGender.SelectedItem = "Prefer not to say"; // Default if gender is not in the list
                }
            }
            else
            {
                // Handle the case where currentUser is null (e.g., if the default constructor was used)
                txtName.Text = "N/A";
                txtPhone.Text = "N/A";
                txtEmail.Text = "N/A";
                cmbGender.SelectedItem = "Prefer not to say";
                // Optionally disable editing if no user data is loaded
                btnEditSave.Enabled = false;
            }
        }

        private void BtnEditSave_Click(object sender, EventArgs e)
        {
            isEditMode = !isEditMode;

            if (isEditMode)
            {
                // Switch to edit mode
                btnEditSave.Text = "Save";
                btnEditSave.BackColor = Color.FromArgb(0, 180, 80);

                txtName.ReadOnly = false;
                txtPhone.ReadOnly = false;
                txtEmail.ReadOnly = false;
                cmbGender.Enabled = true;
            }
            else
            {
                // Switch to view mode and save changes
                if (currentUser != null)
                {
                    // Update the currentUser object with the new data
                    currentUser.fullName = txtName.Text.Trim();
                    currentUser.PhoneNumber = txtPhone.Text.Trim();
                    currentUser.Email = txtEmail.Text.Trim();
                    currentUser.Gender = cmbGender.SelectedItem?.ToString() ?? "Prefer not to say"; // Handle null selection

                    // Save the data to the database
                    bool saveSuccess = DatabaseConnection.UpdateUser(currentUser);

                    if (saveSuccess)
                    {
                        MessageBox.Show("Changes saved successfully!", "Success",
                           MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Optionally refresh the displayed data from the updated currentUser object
                        LoadUserData(); // This will reload from the potentially updated currentUser object
                    }
                    else
                    {
                        MessageBox.Show("Failed to save changes. Please try again.", "Save Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        // Revert back to edit mode or reload original data if save failed
                        isEditMode = true; // Stay in edit mode or revert UI
                        btnEditSave.Text = "Save";
                        btnEditSave.BackColor = Color.FromArgb(0, 180, 80);
                        txtName.ReadOnly = false;
                        txtPhone.ReadOnly = false;
                        txtEmail.ReadOnly = false;
                        cmbGender.Enabled = true;
                    }
                }
                else
                {
                    MessageBox.Show("No user data to save.", "Save Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }


                // Always revert UI to view mode after attempting save
                btnEditSave.Text = "Edit";
                btnEditSave.BackColor = Color.FromArgb(0, 120, 215);

                txtName.ReadOnly = true;
                txtPhone.ReadOnly = true;
                txtEmail.ReadOnly = true;
                cmbGender.Enabled = false;

            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // Optional: Add custom panel painting if needed
        }
    }
}