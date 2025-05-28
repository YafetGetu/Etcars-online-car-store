using System;
using System.Drawing;
using System.Windows.Forms;

namespace carstore
{
    public partial class setting : Form
    {
        public setting()
        {
            InitializeComponent();
            InitializeSettingsUI();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeSettingsUI()
        {
            // Form setup with dark blue background
            this.BackColor = Color.FromArgb(0, 40, 85);  // Dark blue background
            this.Size = new Size(500, 500);
            this.Text = "Settings";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Title label with white text
            Label lblTitle = new Label()
            {
                Text = "Settings",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true,
                ForeColor = Color.White  // White text
            };

            // Edit Profile with white text
            Label lblEditProfile = new Label()
            {
                Text = "Edit Profile",
                Location = new Point(40, 80),
                AutoSize = true,
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.White  // White text
            };

            Button btnEditProfile = new Button()
            {
                Text = "Edit",
                Location = new Point(250, 75),
                Size = new Size(120, 30),
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnEditProfile.FlatAppearance.BorderSize = 0;
            btnEditProfile.Click += (s, e) =>
            {
                MessageBox.Show("Open profile edit form.", "Info");
                // TODO: Open profile edit form
            };

            // Privacy Policy with white text
            Label lblPrivacy = new Label()
            {
                Text = "Privacy Policy",
                Location = new Point(40, 130),
                AutoSize = true,
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.White  // White text
            };

            Button btnPrivacy = new Button()
            {
                Text = "View",
                Location = new Point(250, 125),
                Size = new Size(120, 30),
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnPrivacy.FlatAppearance.BorderSize = 0;
            btnPrivacy.Click += (s, e) =>
            {
                MessageBox.Show("Show privacy policy content here.", "Privacy Policy");
            };

            // Feedback & Support with white text
            Label lblFeedback = new Label()
            {
                Text = "Feedback & Support",
                Location = new Point(40, 180),
                AutoSize = true,
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.White  // White text
            };

            Button btnFeedback = new Button()
            {
                Text = "Contact",
                Location = new Point(250, 175),
                Size = new Size(120, 30),
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnFeedback.FlatAppearance.BorderSize = 0;
            btnFeedback.Click += (s, e) =>
            {
                MessageBox.Show("Please email support@carstore.com with your feedback.", "Feedback");
            };

            // Clear All Data with white text
            Label lblClearData = new Label()
            {
                Text = "Clear All Data",
                Location = new Point(40, 230),
                AutoSize = true,
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.White  // White text
            };

            Button btnClearData = new Button()
            {
                Text = "Clear",
                Location = new Point(250, 225),
                Size = new Size(120, 30),
                BackColor = Color.FromArgb(220, 80, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnClearData.FlatAppearance.BorderSize = 0;
            btnClearData.Click += (s, e) =>
            {
                var confirm = MessageBox.Show("Clear all app data? This cannot be undone.",
                    "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm == DialogResult.Yes)
                {
                    // TODO: Clear data logic
                    MessageBox.Show("All data cleared.", "Success");
                }
            };

            // Logout with white text
            Label lblLogout = new Label()
            {
                Text = "Logout",
                Location = new Point(40, 280),
                AutoSize = true,
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.White  // White text
            };

            Button btnLogout = new Button()
            {
                Text = "Sign Out",
                Location = new Point(250, 275),
                Size = new Size(120, 30),
                BackColor = Color.FromArgb(220, 80, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Click += (s, e) =>
            {
                var confirm = MessageBox.Show("Are you sure you want to logout?",
                    "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    // TODO: Logout logic
                    MessageBox.Show("You have been logged out.", "Success");
                    this.Close();
                }
            };

            // App Version with light gray text
            Label lblVersion = new Label()
            {
                Text = $"Version: {Application.ProductVersion}",
                Location = new Point(20, 420),
                AutoSize = true,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.LightGray  // Light gray for version text
            };

            // Add all controls to form
            this.Controls.Add(lblTitle);

            this.Controls.Add(lblEditProfile);
            this.Controls.Add(btnEditProfile);

            this.Controls.Add(lblPrivacy);
            this.Controls.Add(btnPrivacy);

            this.Controls.Add(lblFeedback);
            this.Controls.Add(btnFeedback);

            this.Controls.Add(lblClearData);
            this.Controls.Add(btnClearData);

            this.Controls.Add(lblLogout);
            this.Controls.Add(btnLogout);

            this.Controls.Add(lblVersion);
        }
    }
}