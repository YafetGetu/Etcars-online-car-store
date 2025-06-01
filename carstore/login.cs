using System;
using System.Drawing;
using System.Windows.Forms;
using carstore; 

namespace carstore
{
    public partial class login : Form
    {

        public string LoggedInUsername { get; private set; }

        
        private Panel pnlLogin;

        private Label labelLoginName;
        private TextBox textBoxLoginName;
        private Label labelLoginPassword;
        private TextBox textBoxLoginPassword;
        private Button buttonPerformLogin; 

        public login()
        {
            
            InitializeComponent();

            SetupLoginPanel();

            ApplyDesignerControlStyling();

            
            LoggedInUsername = string.Empty;
        }

        
        private void ApplyDesignerControlStyling()
        {
            this.Text = "Car Store - User Authentication";
            this.Size = new Size(1000, 800); 
            this.MinimumSize = new Size(500, 600); 
            this.BackColor = Color.FromArgb(0, 40, 85); 
            this.FormBorderStyle = FormBorderStyle.FixedSingle; 
            this.MaximizeBox = true; 
            this.StartPosition = FormStartPosition.CenterScreen; 

            panel1.Size = new Size(600, 550); 
            panel1.Location = new Point((this.ClientSize.Width - panel1.Width) / 2 - 8, 80);
            panel1.BackColor = Color.White; 
            panel1.BorderStyle = BorderStyle.FixedSingle; 
            panel1.Padding = new Padding(20); 
            panel1.BringToFront(); 

            button2.Text = "Login"; 
            button2.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            button2.Size = new Size(180, 45);
            button2.Location = new Point(215, 400);
            button2.BackColor = Color.FromArgb(70, 130, 180); 
            button2.ForeColor = Color.White;
            button2.FlatStyle = FlatStyle.Flat; 
            button2.FlatAppearance.BorderSize = 0; 


            int yPos = 30; 
            int labelWidth = 150;
            int textBoxWidth = 280;
            int controlHeight = 30;
            int spacing = 45; 

            label1.Text = "Full Name:";
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            label1.Size = new Size(labelWidth, controlHeight);
            label1.Location = new Point(20, yPos);

            textBox1.PlaceholderText = "Enter your full name"; 
            textBox1.Font = new Font("Segoe UI", 10F);
            textBox1.Size = new Size(textBoxWidth, controlHeight);
            textBox1.Location = new Point(labelWidth + 30, yPos);
            textBox1.BorderStyle = BorderStyle.FixedSingle;
            yPos += spacing;

            label2.Text = "Phone Number:";
            label2.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            label2.Size = new Size(labelWidth, controlHeight);
            label2.Location = new Point(20, yPos);

            textBox2.PlaceholderText = "e.g., +2518985959898";
            textBox2.Font = new Font("Segoe UI", 10F);
            textBox2.Size = new Size(textBoxWidth, controlHeight);
            textBox2.Location = new Point(labelWidth + 30, yPos);
            textBox2.BorderStyle = BorderStyle.FixedSingle;
            yPos += spacing;

            label3.Text = "Email:";
            label3.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            label3.Size = new Size(labelWidth, controlHeight);
            label3.Location = new Point(20, yPos);

            textBox3.PlaceholderText = "your.email@example.com";
            textBox3.Font = new Font("Segoe UI", 10F);
            textBox3.Size = new Size(textBoxWidth, controlHeight);
            textBox3.Location = new Point(labelWidth + 30, yPos);
            textBox3.BorderStyle = BorderStyle.FixedSingle;
            yPos += spacing;

            label4.Text = "Password:";
            label4.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            label4.Size = new Size(labelWidth, controlHeight);
            label4.Location = new Point(20, yPos);

            textBox4.PlaceholderText = "Enter your password";
            textBox4.Font = new Font("Segoe UI", 10F);
            textBox4.Size = new Size(textBoxWidth, controlHeight);
            textBox4.Location = new Point(labelWidth + 30, yPos);
            textBox4.UseSystemPasswordChar = true; 
            textBox4.BorderStyle = BorderStyle.FixedSingle;
            yPos += spacing;

            label5.Text = "Gender:";
            label5.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            label5.Size = new Size(labelWidth, controlHeight);
            label5.Location = new Point(20, yPos);

            groupBox1.Text = ""; 
            groupBox1.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            groupBox1.Size = new Size(textBoxWidth, 100); 
            
            groupBox1.Location = new Point(labelWidth + 30, yPos - 10);

            radioButton1.Text = "Male";
            radioButton1.Font = new Font("Segoe UI", 9F);
            radioButton1.Size = new Size(100, 25);
            radioButton1.Location = new Point(10, 20); 

            radioButton2.Text = "Female";
            radioButton2.Font = new Font("Segoe UI", 9F);
            radioButton2.Size = new Size(100, 25);
            radioButton2.Location = new Point(10, 45);

            radioButton3.Text = "I prefer not to say";
            radioButton3.Font = new Font("Segoe UI", 9F);
            radioButton3.Size = new Size(150, 25);
            radioButton3.Location = new Point(10, 70);


            if (panel1.Controls.Contains(radioButton1)) panel1.Controls.Remove(radioButton1);
            if (panel1.Controls.Contains(radioButton2)) panel1.Controls.Remove(radioButton2);
            if (panel1.Controls.Contains(radioButton3)) panel1.Controls.Remove(radioButton3);

            if (!groupBox1.Controls.Contains(radioButton1)) groupBox1.Controls.Add(radioButton1);
            if (!groupBox1.Controls.Contains(radioButton2)) groupBox1.Controls.Add(radioButton2);
            if (!groupBox1.Controls.Contains(radioButton3)) groupBox1.Controls.Add(radioButton3);


            yPos += groupBox1.Height + 20; 

            button1.Text = "Register";
            button1.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            button1.Size = new Size(150, 45);
            button1.Location = new Point((panel1.Width - button1.Width) / 2, yPos);
            button1.BackColor = Color.FromArgb(60, 179, 113); 
            button1.ForeColor = Color.White;
            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderSize = 0;
        }


        private void SetupLoginPanel()
        {
            pnlLogin = new Panel();
            pnlLogin.Name = "pnlLogin"; 
            pnlLogin.Size = new Size(600, 550); 
                                                
            pnlLogin.Location = new Point((this.ClientSize.Width - panel1.Width) / 2, 80);

            pnlLogin.BackColor = Color.White;
            pnlLogin.BorderStyle = BorderStyle.FixedSingle;
            pnlLogin.Padding = new Padding(20);
            pnlLogin.Visible = false; 
            this.Controls.Add(pnlLogin); 

            int loginYPos = 50; 
            int loginLabelWidth = 100;
            int loginTextBoxWidth = 250;
            int loginControlHeight = 30;
            int loginSpacing = 60; 

            labelLoginName = new Label();
            labelLoginName.Name = "labelLoginName"; 
            labelLoginName.Text = "Name:";
            labelLoginName.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            labelLoginName.Size = new Size(loginLabelWidth, loginControlHeight);
            labelLoginName.Location = new Point(20, loginYPos);
            pnlLogin.Controls.Add(labelLoginName);

            textBoxLoginName = new TextBox();
            textBoxLoginName.Name = "textBoxLoginName"; 
            textBoxLoginName.PlaceholderText = "Enter your username or full name";
            textBoxLoginName.Font = new Font("Segoe UI", 10F);
            textBoxLoginName.Size = new Size(loginTextBoxWidth, loginControlHeight);
            textBoxLoginName.Location = new Point(loginLabelWidth + 30, loginYPos);
            textBoxLoginName.BorderStyle = BorderStyle.FixedSingle;
            pnlLogin.Controls.Add(textBoxLoginName);

            loginYPos += loginSpacing;

            labelLoginPassword = new Label();
            labelLoginPassword.Name = "labelLoginPassword"; 
            labelLoginPassword.Text = "Password:";
            labelLoginPassword.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            labelLoginPassword.Size = new Size(loginLabelWidth, loginControlHeight);
            labelLoginPassword.Location = new Point(20, loginYPos);
            pnlLogin.Controls.Add(labelLoginPassword);

            textBoxLoginPassword = new TextBox();
            textBoxLoginPassword.Name = "textBoxLoginPassword"; 
            textBoxLoginPassword.PlaceholderText = "Enter your password";
            textBoxLoginPassword.Font = new Font("Segoe UI", 10F);
            textBoxLoginPassword.Size = new Size(loginTextBoxWidth, loginControlHeight);
            textBoxLoginPassword.Location = new Point(loginLabelWidth + 30, loginYPos);
            textBoxLoginPassword.UseSystemPasswordChar = true; 
            textBoxLoginPassword.BorderStyle = BorderStyle.FixedSingle;
            pnlLogin.Controls.Add(textBoxLoginPassword);

            loginYPos += loginSpacing;

            buttonPerformLogin = new Button();
            buttonPerformLogin.Name = "buttonPerformLogin"; 
            buttonPerformLogin.Text = "Login";
            buttonPerformLogin.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            buttonPerformLogin.Size = new Size(150, 45);
            buttonPerformLogin.Location = new Point(150, loginYPos);
            buttonPerformLogin.BackColor = Color.FromArgb(255, 140, 0); 
            buttonPerformLogin.ForeColor = Color.White;
            buttonPerformLogin.FlatStyle = FlatStyle.Flat;
            buttonPerformLogin.FlatAppearance.BorderSize = 0;
            buttonPerformLogin.Click += new EventHandler(this.buttonPerformLogin_Click);
            pnlLogin.Controls.Add(buttonPerformLogin);


            Button buttonSwitchToRegister = new Button();
            buttonSwitchToRegister.Name = "buttonSwitchToRegister"; 
            buttonSwitchToRegister.Text = "Don't have an account? Register";
            buttonSwitchToRegister.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            buttonSwitchToRegister.Size = new Size(250, 35);
            buttonSwitchToRegister.Location = new Point(110, loginYPos);
            buttonSwitchToRegister.BackColor = Color.Transparent;
            buttonSwitchToRegister.ForeColor = Color.FromArgb(70, 130, 180); 
            buttonSwitchToRegister.FlatStyle = FlatStyle.Flat;
            buttonSwitchToRegister.FlatAppearance.BorderSize = 0;
            buttonSwitchToRegister.Cursor = Cursors.Hand; 
            buttonSwitchToRegister.Click += (s, e) =>
            {
                pnlLogin.Visible = false;
                panel1.Visible = true;
                button2.Text = "Login"; 
                this.Text = "Car Store - User Registration"; 
            };
            pnlLogin.Controls.Add(buttonSwitchToRegister);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void label3_Click(object sender, EventArgs e)
        {
        }

        private void label4_Click(object sender, EventArgs e)
        {
        }

        private void label5_Click(object sender, EventArgs e)
        {
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
        }

        private void login_Load(object sender, EventArgs e)
        {
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string fullName = textBox1.Text.Trim();
            string phoneNumber = textBox2.Text.Trim();
            string email = textBox3.Text.Trim();
            string password = textBox4.Text; 

            string gender = "Not specified";
            if (radioButton1.Checked) gender = "Male";
            else if (radioButton2.Checked) gender = "Female";
            else if (radioButton3.Checked) gender = "I prefer not to say";

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please fill in all required fields (Full Name, Email, Password).", "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool registrationSuccess = DatabaseConnection.RegisterUser(fullName, email, password, phoneNumber, gender);

            if (registrationSuccess)
            {
                MessageBox.Show("Registration Successful! You can now login.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                radioButton3.Checked = false;

                panel1.Visible = false;
                pnlLogin.Visible = true;
                button2.Text = "Switch to Register";
                this.Text = "Car Store - User Login";
            }
            else
            {
                MessageBox.Show("Registration failed. Please try again. (Username or Email might already exist)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (panel1.Visible)
            {
                panel1.Visible = false; 
                pnlLogin.Visible = true; 
                button2.Text = "Switch to Register"; 
                this.Text = "Car Store - User Login"; 
            }
            else
            {
                panel1.Visible = true; 
                pnlLogin.Visible = false; 
                button2.Text = "Switch to Login"; 
                this.Text = "Car Store - User Registration"; 
            }
        }


        private void buttonPerformLogin_Click(object sender, EventArgs e)
        {
            string loginName = textBoxLoginName.Text.Trim();
            string loginPassword = textBoxLoginPassword.Text; 

            if (string.IsNullOrEmpty(loginName) || string.IsNullOrEmpty(loginPassword))
            {
                MessageBox.Show("Please enter both Name and Password to log in.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var (success, isAdmin) = DatabaseConnection.ValidateUser(loginName, loginPassword);

            if (success)
            {
                LoggedInUsername = loginName;
                DialogResult = DialogResult.OK;

                if (isAdmin)
                {
                    AdminPage adminForm = new AdminPage();
                    this.Hide(); 
                    adminForm.ShowDialog(); 
                    this.Close(); 
                }
                else
                {
                    MessageBox.Show("Login Successful! Welcome to Car Store.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Invalid Name or Password. Please try again.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                textBoxLoginName.Clear();
                textBoxLoginPassword.Clear();
            }
        }
    }
}