using System;
using System.Drawing;
using System.Windows.Forms;

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
            LoadUserData();
        }

        private void InitializeProfileComponents()
        {
            // Main panel setup
            panel1.BackColor = Color.White;
            panel1.Location = new Point(30, 30);
            panel1.Size = new Size(580, 450);
            panel1.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(panel1);

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
            cmbGender.Font = valueFont;
            cmbGender.Location = new Point(controlStartX, currentY);
            cmbGender.Size = new Size(150, 26);
            cmbGender.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbGender.Enabled = false;
            cmbGender.Items.AddRange(new object[] { "Male", "Female", "Other", "Prefer not to say" });
            panel1.Controls.Add(cmbGender);

            // Edit/Save button
            btnEditSave = new Button();
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
            // Sample data
            txtName.Text = "ET cars  ";
            txtPhone.Text = "+2515984949+";
            txtEmail.Text = "etcars@gmail.com";
            cmbGender.SelectedItem = "Male";
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
                btnEditSave.Text = "Edit";
                btnEditSave.BackColor = Color.FromArgb(0, 120, 215);

                txtName.ReadOnly = true;
                txtPhone.ReadOnly = true;
                txtEmail.ReadOnly = true;
                cmbGender.Enabled = false;

                // Here you would typically save the data to a database or file
                MessageBox.Show("Changes saved successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // Optional: Add custom panel painting if needed
        }
    }
}