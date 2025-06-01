using System;
using System.Drawing;
using System.Windows.Forms;
using carstore; 
using System.Linq; 
using System.Collections.Generic; 

namespace carstore
{
    public partial class setting : Form
    {
        private User currentUser; 
        public List<CartItem> UserCartItems { get; private set; }

        public setting(User user, List<CartItem> cartItems)
        {
            InitializeComponent();
            this.currentUser = user; 
            this.UserCartItems = cartItems; 
            InitializeSettingsUI();
            this.StartPosition = FormStartPosition.CenterScreen;

            this.Owner = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            if (this.Owner != null)
            {
                this.Owner.AddOwnedForm(this);
            }

            this.DialogResult = DialogResult.Cancel;
        }

        private void InitializeSettingsUI()
        {
            this.BackColor = Color.FromArgb(0, 40, 85);
            this.Size = new Size(500, 500);
            this.Text = "Settings";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            Label lblTitle = new Label()
            {
                Text = "Settings",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true,
                ForeColor = Color.White
            };

            Label lblEditProfile = new Label()
            {
                Text = "Edit Profile",
                Location = new Point(40, 80),
                AutoSize = true,
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.White
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

            btnEditProfile.Click += (s, args) =>
            {
                if (currentUser != null)
                {
                    profile profileForm = new profile(currentUser);
                    DialogResult result = profileForm.ShowDialog();



                }
                else
                {
                    MessageBox.Show("You must be logged in to edit your profile.", "Login Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            };


            Label lblPrivacy = new Label()
            {
                Text = "Privacy Policy",
                Location = new Point(40, 130),
                AutoSize = true,
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.White
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
                privacy privacyForm = new privacy();
                privacyForm.ShowDialog();
            };


            Label lblFeedback = new Label()
            {
                Text = "Feedback & Support",
                Location = new Point(40, 180),
                AutoSize = true,
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.White
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

            Label lblClearData = new Label()
            {
                Text = "Clear Cart", 
                Location = new Point(40, 230),
                AutoSize = true,
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.White
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
                var confirm = MessageBox.Show("Clear all items from your cart?",
                    "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm == DialogResult.Yes)
                {
                    if (UserCartItems != null) 
                    {
                        UserCartItems.Clear();
                        MessageBox.Show("Your cart has been cleared.", "Success");
                        
                    }
                    else
                    {
                        MessageBox.Show("Cart is already empty or not accessible.", "Information");
                    }
                }
            };

            Label lblLogout = new Label()
            {
                Text = "Logout",
                Location = new Point(40, 280),
                AutoSize = true,
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.White
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
                    this.DialogResult = DialogResult.OK; 
                    this.Close(); 
                    
                }
            };


            Label lblVersion = new Label()
            {
                Text = $"Et Cars Version: {Application.ProductVersion}",
                Location = new Point(20, this.ClientSize.Height - 40), 
                AutoSize = true,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.LightGray
            };
            lblVersion.Anchor = AnchorStyles.Bottom | AnchorStyles.Left; 


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

        private void setting_Load(object sender, EventArgs e)
        {
            
            Control[] foundVersionLabel = this.Controls.Find("lblVersion", false);
            if (foundVersionLabel.Length > 0 && foundVersionLabel[0] is Label lblVersion)
            {
                lblVersion.Location = new Point(20, this.ClientSize.Height - lblVersion.Height - 10);
            }
        }

        
    }
}