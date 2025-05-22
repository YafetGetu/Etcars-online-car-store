using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace carstore
{
    public partial class login : Form
    {
        private bool isLoginActive = true;
        private TextBox txtUsername, txtPassword, txtFullName, txtEmail, txtConfirmPassword;
        private CheckBox chkRememberMe;

        public login()
        {
            InitializeComponent();
            InitializeControls();
            this.DoubleBuffered = true; // Reduce flickering
        }

        private void InitializeControls()
        {
            // Remove existing controls
            panel1.Controls.Clear();

            // Common properties
            Font inputFont = new Font("Segoe UI", 10);
            Color borderColor = Color.FromArgb(200, 200, 200);

            if (isLoginActive)
            {
                // Login controls
                txtUsername = new TextBox
                {
                    Font = inputFont,
                    BorderStyle = BorderStyle.None,
                    ForeColor = Color.FromArgb(70, 70, 70),
                    BackColor = Color.White,
                    Location = new Point(35, 155),
                    Size = new Size(280, 25),
                    PlaceholderText = "Enter your username"
                };

                txtPassword = new TextBox
                {
                    Font = inputFont,
                    BorderStyle = BorderStyle.None,
                    ForeColor = Color.FromArgb(70, 70, 70),
                    BackColor = Color.White,
                    Location = new Point(35, 225),
                    Size = new Size(280, 25),
                    PlaceholderText = "Enter your password",
                    PasswordChar = '•'
                };

                chkRememberMe = new CheckBox
                {
                    Text = "Remember me",
                    Font = new Font("Segoe UI", 8),
                    ForeColor = Color.FromArgb(100, 100, 100),
                    Location = new Point(35, 265),
                    AutoSize = true
                };

                Button btnLogin = new Button
                {
                    Text = "LOGIN",
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = Color.FromArgb(70, 100, 180),
                    FlatStyle = FlatStyle.Flat,
                    Location = new Point(35, 295),
                    Size = new Size(280, 40)
                };
                btnLogin.FlatAppearance.BorderSize = 0;
                btnLogin.Click += BtnLogin_Click;

                LinkLabel lnkForgotPassword = new LinkLabel
                {
                    Text = "Forgot password?",
                    Font = new Font("Segoe UI", 8),
                    Location = new Point(35, 345),
                    AutoSize = true,
                    LinkColor = Color.DodgerBlue
                };
                lnkForgotPassword.Click += LnkForgotPassword_Click;

                LinkLabel lnkSignup = new LinkLabel
                {
                    Text = "Don't have an account? Sign Up",
                    Font = new Font("Segoe UI", 8),
                    Location = new Point(70, 440),
                    AutoSize = true,
                    LinkColor = Color.FromArgb(70, 100, 180)
                };
                lnkSignup.Click += LnkSignup_Click;

                panel1.Controls.AddRange(new Control[] { txtUsername, txtPassword, chkRememberMe, btnLogin, lnkForgotPassword, lnkSignup });
            }
            else
            {
                // Signup controls
                txtFullName = new TextBox
                {
                    Font = inputFont,
                    BorderStyle = BorderStyle.None,
                    ForeColor = Color.FromArgb(70, 70, 70),
                    BackColor = Color.White,
                    Location = new Point(35, 155),
                    Size = new Size(280, 25),
                    PlaceholderText = "Full name"
                };

                txtEmail = new TextBox
                {
                    Font = inputFont,
                    BorderStyle = BorderStyle.None,
                    ForeColor = Color.FromArgb(70, 70, 70),
                    BackColor = Color.White,
                    Location = new Point(35, 225),
                    Size = new Size(280, 25),
                    PlaceholderText = "Email address"
                };

                txtPassword = new TextBox
                {
                    Font = inputFont,
                    BorderStyle = BorderStyle.None,
                    ForeColor = Color.FromArgb(70, 70, 70),
                    BackColor = Color.White,
                    Location = new Point(35, 295),
                    Size = new Size(280, 25),
                    PlaceholderText = "Create password",
                    PasswordChar = '•'
                };

                txtConfirmPassword = new TextBox
                {
                    Font = inputFont,
                    BorderStyle = BorderStyle.None,
                    ForeColor = Color.FromArgb(70, 70, 70),
                    BackColor = Color.White,
                    Location = new Point(35, 365),
                    Size = new Size(280, 25),
                    PlaceholderText = "Confirm password",
                    PasswordChar = '•'
                };

                Button btnSignup = new Button
                {
                    Text = "SIGN UP",
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = Color.FromArgb(70, 180, 100),
                    FlatStyle = FlatStyle.Flat,
                    Location = new Point(35, 405),
                    Size = new Size(280, 40)
                };
                btnSignup.FlatAppearance.BorderSize = 0;
                btnSignup.Click += BtnSignup_Click;

                LinkLabel lnkLogin = new LinkLabel
                {
                    Text = "Already have an account? Login",
                    Font = new Font("Segoe UI", 8),
                    Location = new Point(70, 460),
                    AutoSize = true,
                    LinkColor = Color.FromArgb(70, 100, 180)
                };
                lnkLogin.Click += LnkLogin_Click;

                panel1.Controls.AddRange(new Control[] { txtFullName, txtEmail, txtPassword, txtConfirmPassword, btnSignup, lnkLogin });
            }

            // Add hover effects to all controls
            foreach (Control control in panel1.Controls)
            {
                if (control is Button btn)
                {
                    btn.MouseEnter += (s, e) => btn.BackColor = ControlPaint.Light(btn.BackColor, 0.2f);
                    btn.MouseLeave += (s, e) => btn.BackColor = isLoginActive ?
                        Color.FromArgb(70, 100, 180) : Color.FromArgb(70, 180, 100);
                }
                else if (control is LinkLabel lnk)
                {
                    lnk.LinkBehavior = LinkBehavior.HoverUnderline;
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // Set up graphics for smooth rendering
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Background gradient
            Rectangle panelRect = panel1.ClientRectangle;
            using (LinearGradientBrush bgBrush = new LinearGradientBrush(
                panelRect, Color.FromArgb(240, 240, 240), Color.FromArgb(200, 220, 255), 120f))
            {
                e.Graphics.FillRectangle(bgBrush, panelRect);
            }

            // Form container
            Rectangle formRect = new Rectangle(
                panelRect.Width / 2 - 175,
                panelRect.Height / 2 - (isLoginActive ? 200 : 250),
                350,
                isLoginActive ? 400 : 500);

            // Draw form container with shadow
            using (var shadowPath = new GraphicsPath())
            {
                shadowPath.AddRectangle(new Rectangle(formRect.X + 5, formRect.Y + 5, formRect.Width, formRect.Height));
                using (var shadowBrush = new SolidBrush(Color.FromArgb(50, Color.Black)))
                {
                    e.Graphics.FillPath(shadowBrush, shadowPath);
                }
            }

            // Form background
            using (var formBrush = new SolidBrush(Color.White))
            {
                e.Graphics.FillRectangle(formBrush, formRect);
            }
            e.Graphics.DrawRectangle(Pens.LightGray, formRect);

            // Title
            using (Font titleFont = new Font("Segoe UI", 20, FontStyle.Bold))
            using (SolidBrush titleBrush = new SolidBrush(Color.FromArgb(70, 100, 180)))
            {
                e.Graphics.DrawString("Welcome", titleFont, titleBrush,
                    new PointF(formRect.X + 20, formRect.Y + 20));
            }

            // Tab selection (Login/Signup)
            Rectangle loginTab = new Rectangle(formRect.X + 20, formRect.Y + 70, 100, 40);
            Rectangle signupTab = new Rectangle(formRect.X + 130, formRect.Y + 70, 100, 40);

            // Draw tabs
            using (var tabBrush = new SolidBrush(Color.FromArgb(70, 100, 180)))
            using (var inactiveTabBrush = new SolidBrush(Color.LightGray))
            using (var tabFont = new Font("Segoe UI", 11, FontStyle.Regular))
            {
                // Login tab
                e.Graphics.FillRectangle(isLoginActive ? tabBrush : inactiveTabBrush, loginTab);
                e.Graphics.DrawString("Login", tabFont, Brushes.White,
                    new PointF(loginTab.X + 30, loginTab.Y + 10));

                // Signup tab
                e.Graphics.FillRectangle(!isLoginActive ? tabBrush : inactiveTabBrush, signupTab);
                e.Graphics.DrawString("Sign Up", tabFont, Brushes.White,
                    new PointF(signupTab.X + 20, signupTab.Y + 10));
            }

            // Draw input field backgrounds
            int fieldY = formRect.Y + 130;
            int fieldHeight = isLoginActive ? 400 : 500;

            if (isLoginActive)
            {
                // Login field backgrounds
                e.Graphics.DrawString("Username", new Font("Segoe UI", 9), Brushes.DimGray,
                    new PointF(formRect.X + 30, fieldY));
                e.Graphics.DrawRectangle(Pens.LightGray, new Rectangle(formRect.X + 30, fieldY + 20, 290, 35));

                e.Graphics.DrawString("Password", new Font("Segoe UI", 9), Brushes.DimGray,
                    new PointF(formRect.X + 30, fieldY + 70));
                e.Graphics.DrawRectangle(Pens.LightGray, new Rectangle(formRect.X + 30, fieldY + 90, 290, 35));

                // Remember me checkbox
                e.Graphics.DrawRectangle(Pens.LightGray, new Rectangle(formRect.X + 30, fieldY + 140, 15, 15));
                e.Graphics.DrawString("Remember me", new Font("Segoe UI", 8), Brushes.DimGray,
                    new PointF(formRect.X + 50, fieldY + 140));

                // Forgot password link
                e.Graphics.DrawString("Forgot password?", new Font("Segoe UI", 8, FontStyle.Underline),
                    Brushes.DodgerBlue, new PointF(formRect.X + 30, fieldY + 220));
            }
            else
            {
                // Signup field backgrounds
                e.Graphics.DrawString("Full Name", new Font("Segoe UI", 9), Brushes.DimGray,
                    new PointF(formRect.X + 30, fieldY));
                e.Graphics.DrawRectangle(Pens.LightGray, new Rectangle(formRect.X + 30, fieldY + 20, 290, 35));

                e.Graphics.DrawString("Email", new Font("Segoe UI", 9), Brushes.DimGray,
                    new PointF(formRect.X + 30, fieldY + 70));
                e.Graphics.DrawRectangle(Pens.LightGray, new Rectangle(formRect.X + 30, fieldY + 90, 290, 35));

                e.Graphics.DrawString("Password", new Font("Segoe UI", 9), Brushes.DimGray,
                    new PointF(formRect.X + 30, fieldY + 140));
                e.Graphics.DrawRectangle(Pens.LightGray, new Rectangle(formRect.X + 30, fieldY + 160, 290, 35));

                e.Graphics.DrawString("Confirm Password", new Font("Segoe UI", 9), Brushes.DimGray,
                    new PointF(formRect.X + 30, fieldY + 210));
                e.Graphics.DrawRectangle(Pens.LightGray, new Rectangle(formRect.X + 30, fieldY + 230, 290, 35));
            }
        }

        private void LnkSignup_Click(object sender, EventArgs e)
        {
            isLoginActive = false;
            InitializeControls();
            panel1.Invalidate();
        }

        private void LnkLogin_Click(object sender, EventArgs e)
        {
            isLoginActive = true;
            InitializeControls();
            panel1.Invalidate();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            // Validate and process login
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter both username and password", "Login Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // TODO: Add your authentication logic here
            MessageBox.Show("Login successful!", "Welcome",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnSignup_Click(object sender, EventArgs e)
        {
            // Validate and process signup
            if (string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
            {
                MessageBox.Show("Please fill in all fields", "Signup Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Passwords do not match", "Signup Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // TODO: Add your registration logic here
            MessageBox.Show("Account created successfully!", "Welcome",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LnkForgotPassword_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Password reset instructions will be sent to your email", "Forgot Password",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}