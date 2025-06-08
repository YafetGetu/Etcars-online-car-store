using System;
using System.Drawing;
using System.Windows.Forms;
using carstore; // Make sure to include the carstore namespace
using System.Linq; // Required for OfType<T>() and FirstOrDefault()
using System.Collections.Generic; // Required for List<CartItem>

namespace carstore
{
    public partial class setting : Form
    {
        private User currentUser; // Member variable to hold the current user
        // Changed to a property to be accessible if needed, though primarily passed for reference
        public List<CartItem> UserCartItems { get; private set; }

        // MODIFIED Constructor to accept the current User and the cart items list
        public setting(User user, List<CartItem> cartItems)
        {
            InitializeComponent();
            this.currentUser = user; // Store the current user
            this.UserCartItems = cartItems; // Store the reference to the cart items list
            InitializeSettingsUI();
            this.StartPosition = FormStartPosition.CenterScreen;

            // Ensure the form stays on top of the main form while open
            // Setting the Owner helps with taskbar grouping and z-order
            this.Owner = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            if (this.Owner != null)
            {
                this.Owner.AddOwnedForm(this);
            }

            // Set initial DialogResult to Cancel in case the form is just closed
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

            // --- Edit Profile ---
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
                    // Pass the current user object to the profile form
                    profile profileForm = new profile(currentUser);
                    // Show the profile form as a dialog
                    DialogResult result = profileForm.ShowDialog();

                    // After the profile form closes, you might want to check if user data was updated
                    // and potentially refresh the currentUser object in THIS settings form,
                    // or rely on Form1 to re-fetch when settings form closes.
                    // For now, leaving as is, assuming Form1 handles post-settings-form refresh if needed.

                }
                else
                {
                    MessageBox.Show("You must be logged in to edit your profile.", "Login Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            };


            // --- Privacy Policy ---
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
                // Assuming you have a Privacy form named 'privacy'
                privacy privacyForm = new privacy();
                privacyForm.ShowDialog();
            };


            // --- Feedback & Support ---
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

            // --- Clear Cart (Interpreting as Clear Cart for now) ---
            Label lblClearData = new Label()
            {
                Text = "Clear Cart", // Changed text for clarity
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
                    // Clear the cart items list passed from Form1
                    if (UserCartItems != null) // Use the public property
                    {
                        UserCartItems.Clear();
                        MessageBox.Show("Your cart has been cleared.", "Success");
                        // No need to signal Form1 to refresh the cart display here,
                        // as Form1 will clear its list on successful payment in the Cart form.
                        // If you had a cart count label in Form1, you'd need a way to update it.
                    }
                    else
                    {
                        MessageBox.Show("Cart is already empty or not accessible.", "Information");
                    }
                }
            };

            // --- Logout ---
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
                    // *** CORRECTED LOGOUT LOGIC: Signal to Form1 and close ***
                    this.DialogResult = DialogResult.OK; // Set DialogResult to OK
                    // The message "You have been logged out." will now be shown in Form1
                    this.Close(); // Close the settings form
                    // *** Removed the direct logout actions from here ***
                }
                // If user clicks No, DialogResult remains Cancel and form stays open.
            };


            // --- Version Label ---
            Label lblVersion = new Label()
            {
                Text = $"Et Cars Version: {Application.ProductVersion}",
                Location = new Point(20, this.ClientSize.Height - 40), // Position near the bottom
                AutoSize = true,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.LightGray
            };
            lblVersion.Anchor = AnchorStyles.Bottom | AnchorStyles.Left; // Anchor to bottom left


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

            // Adjust the vertical position of the version label after adding other controls
            // This is better handled after the form has its final size in the Load event.
        }

        private void setting_Load(object sender, EventArgs e)
        {
            // Adjust the version label position here to be more reliable
            Control[] foundVersionLabel = this.Controls.Find("lblVersion", false);
            if (foundVersionLabel.Length > 0 && foundVersionLabel[0] is Label lblVersion)
            {
                lblVersion.Location = new Point(20, this.ClientSize.Height - lblVersion.Height - 10);
            }
        }

        // If you are using a designer, the Dispose method will be in Cart.Designer.cs.
        // If not using a designer, you might need a Dispose method here to clean up resources.
        // protected override void Dispose(bool disposing) { ... }
    }
}