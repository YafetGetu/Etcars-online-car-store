using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using carstore; 

namespace carstore
{
    public partial class order : Form
    {
        private readonly Color primaryColor = Color.FromArgb(0, 120, 215); 
        private readonly Color secondaryColor = Color.FromArgb(50, 50, 80); 
        private readonly Color textColor = Color.White; 
        private readonly Color lightTextColor = Color.LightGray; 
        private readonly Font titleFont = new Font("Segoe UI", 18, FontStyle.Bold);
        private readonly Font labelFont = new Font("Segoe UI", 10, FontStyle.Regular);
        private readonly Font buttonFont = new Font("Segoe UI", 10, FontStyle.Bold);

        private User currentUser; 
        private DataGridView dgvOrders; 

        public order(User user)
        {
            InitializeComponent();
            currentUser = user;
            InitializeCustomComponents();
            LoadUserOrders(); 
        }

        public order()
        {
            InitializeComponent();
            InitializeCustomComponents();
            LoadUserOrders(); 
        }


        private void InitializeCustomComponents()
        {
            this.Text = "My Luxury Car Orders"; 
            this.Size = new Size(700, 500); 
            this.MinimumSize = new Size(600, 400); 
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(0, 40, 85); 

            panel1.Dock = DockStyle.Fill;
            panel1.BackColor = Color.FromArgb(0, 40, 85); 
            panel1.Padding = new Padding(20);
            panel1.BorderStyle = BorderStyle.FixedSingle;

            Label titleLabel = new Label();
            titleLabel.Text = "MY ORDERS"; 
            titleLabel.Font = titleFont;
            titleLabel.ForeColor = textColor; 
            titleLabel.AutoSize = true;
            titleLabel.Location = new Point((panel1.ClientSize.Width - titleLabel.Width) / 2, 20);
            panel1.Controls.Add(titleLabel);

            Panel titleLine = new Panel();
            titleLine.BackColor = primaryColor;
            titleLine.Size = new Size(panel1.ClientSize.Width - 40, 2); 
            titleLine.Location = new Point(20, 60); 
            panel1.Controls.Add(titleLine);


            dgvOrders = new DataGridView();
            dgvOrders.Name = "dgvOrders";
            dgvOrders.Location = new Point(20, 80); 
            dgvOrders.Size = new Size(panel1.ClientSize.Width - 40, panel1.ClientSize.Height - 100); 
            dgvOrders.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right; 

            dgvOrders.AutoGenerateColumns = true; 
            dgvOrders.ReadOnly = true; 
            dgvOrders.AllowUserToAddRows = false; 
            dgvOrders.AllowUserToDeleteRows = false; 
            dgvOrders.AllowUserToResizeRows = false; 
            dgvOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; 

            
            dgvOrders.BackgroundColor = Color.FromArgb(50, 50, 80); 
            dgvOrders.DefaultCellStyle.BackColor = Color.FromArgb(70, 70, 100); 
            dgvOrders.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(80, 80, 110); 
            dgvOrders.DefaultCellStyle.ForeColor = textColor; 
            dgvOrders.ColumnHeadersDefaultCellStyle.BackColor = primaryColor; 
            dgvOrders.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; 
            dgvOrders.EnableHeadersVisualStyles = false; 
            dgvOrders.CellBorderStyle = DataGridViewCellBorderStyle.None; 
            dgvOrders.RowHeadersVisible = false; 
            dgvOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect; 
            dgvOrders.AllowUserToOrderColumns = true; 

            panel1.Controls.Add(dgvOrders);

            
        }

        private void LoadUserOrders()
        {
            if (currentUser != null)
            {
                List<OrderDisplayItem> userOrders = DatabaseConnection.GetUserOrders(currentUser.UserID);

                if (userOrders != null && userOrders.Count > 0)
                {
                    dgvOrders.DataSource = userOrders;

                    dgvOrders.Columns["TotalAmount"].DefaultCellStyle.Format = "C2"; 
                    dgvOrders.Columns["OrderDate"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm"; 
                }
                else
                {
                    Label noOrdersLabel = new Label();
                    noOrdersLabel.Text = "No orders found for this user.";
                    noOrdersLabel.Font = labelFont;
                    noOrdersLabel.ForeColor = lightTextColor;
                    noOrdersLabel.AutoSize = true;
                    noOrdersLabel.Location = new Point(20, 80); 
                    panel1.Controls.Add(noOrdersLabel);

                    dgvOrders.Visible = false;
                }
            }
            else
            {
                Label loginRequiredLabel = new Label();
                loginRequiredLabel.Text = "Please log in to view your orders.";
                loginRequiredLabel.Font = titleFont;
                loginRequiredLabel.ForeColor = Color.Red;
                loginRequiredLabel.AutoSize = true;
                loginRequiredLabel.Location = new Point((panel1.ClientSize.Width - loginRequiredLabel.Width) / 2, panel1.ClientSize.Height / 2 - 20); // Center message
                panel1.Controls.Add(loginRequiredLabel);

                dgvOrders.Visible = false;
            }
        }

        

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, panel1.ClientRectangle,
                                  primaryColor, 1, ButtonBorderStyle.Solid,
                                  primaryColor, 1, ButtonBorderStyle.Solid,
                                  primaryColor, 1, ButtonBorderStyle.Solid,
                                  primaryColor, 1, ButtonBorderStyle.Solid);
        }

        
    }
}