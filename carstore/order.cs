using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using carstore; // Make sure this is included to use User, DatabaseConnection, and OrderDisplayItem

namespace carstore
{
    public partial class order : Form
    {
        // Custom colors and fonts - updated for dark theme
        private readonly Color primaryColor = Color.FromArgb(0, 120, 215); // Blue accent
        private readonly Color secondaryColor = Color.FromArgb(50, 50, 80); // Darker blue-gray
        private readonly Color textColor = Color.White; // White text for visibility
        private readonly Color lightTextColor = Color.LightGray; // For less important text
        private readonly Font titleFont = new Font("Segoe UI", 18, FontStyle.Bold);
        private readonly Font labelFont = new Font("Segoe UI", 10, FontStyle.Regular);
        private readonly Font buttonFont = new Font("Segoe UI", 10, FontStyle.Bold);

        private User currentUser; // Field to hold the logged-in user
        private DataGridView dgvOrders; // DataGridView to display orders

        // Modify the constructor to accept a User object
        public order(User user)
        {
            InitializeComponent();
            // Store the passed user object
            currentUser = user;
            InitializeCustomComponents();
            LoadUserOrders(); // Load and display orders for this user
        }

        // Default constructor (keep for designer, handle null user in LoadUserOrders)
        public order()
        {
            InitializeComponent();
            // currentUser will be null initially if this constructor is used
            InitializeCustomComponents();
            LoadUserOrders(); // Attempt to load orders (will handle null user)
        }


        private void InitializeCustomComponents()
        {
            // Form setup with dark background
            this.Text = "My Luxury Car Orders"; // Updated title
            this.Size = new Size(700, 500); // Adjusted size for displaying list
            this.MinimumSize = new Size(600, 400); // Allow some resizing
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(0, 40, 85); // Dark blue background

            // Main panel setup
            panel1.Dock = DockStyle.Fill;
            panel1.BackColor = Color.FromArgb(0, 40, 85); // Match form background
            panel1.Padding = new Padding(20);
            panel1.BorderStyle = BorderStyle.FixedSingle;

            // Title label (white text)
            Label titleLabel = new Label();
            titleLabel.Text = "MY ORDERS"; // Updated title text
            titleLabel.Font = titleFont;
            titleLabel.ForeColor = textColor; // White text
            titleLabel.AutoSize = true;
            // Center title horizontally
            titleLabel.Location = new Point((panel1.ClientSize.Width - titleLabel.Width) / 2, 20);
            panel1.Controls.Add(titleLabel);

            // Horizontal line under title (light blue)
            Panel titleLine = new Panel();
            titleLine.BackColor = primaryColor;
            titleLine.Size = new Size(panel1.ClientSize.Width - 40, 2); // Adjust width to panel padding
            titleLine.Location = new Point(20, 60); // Position below title
            panel1.Controls.Add(titleLine);


            // DataGridView to display orders
            dgvOrders = new DataGridView();
            dgvOrders.Name = "dgvOrders";
            dgvOrders.Location = new Point(20, 80); // Position below title line
            dgvOrders.Size = new Size(panel1.ClientSize.Width - 40, panel1.ClientSize.Height - 100); // Fill remaining space
            dgvOrders.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right; // Anchor to resize with panel

            dgvOrders.AutoGenerateColumns = true; // Automatically create columns from data source
            dgvOrders.ReadOnly = true; // Make it read-only
            dgvOrders.AllowUserToAddRows = false; // Prevent adding new rows via UI
            dgvOrders.AllowUserToDeleteRows = false; // Prevent deleting rows via UI
            dgvOrders.AllowUserToResizeRows = false; // Prevent resizing rows
            dgvOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Auto-size columns to fill width

            // Styling for DataGridView (optional but recommended for dark theme)
            dgvOrders.BackgroundColor = Color.FromArgb(50, 50, 80); // Dark background for the grid
            dgvOrders.DefaultCellStyle.BackColor = Color.FromArgb(70, 70, 100); // Alternate row color 1
            dgvOrders.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(80, 80, 110); // Alternate row color 2
            dgvOrders.DefaultCellStyle.ForeColor = textColor; // White text for data cells
            dgvOrders.ColumnHeadersDefaultCellStyle.BackColor = primaryColor; // Blue header background
            dgvOrders.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; // White header text
            dgvOrders.EnableHeadersVisualStyles = false; // Allow custom header style
            dgvOrders.CellBorderStyle = DataGridViewCellBorderStyle.None; // No cell borders
            dgvOrders.RowHeadersVisible = false; // Hide row headers
            dgvOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // Select full row on click
            dgvOrders.AllowUserToOrderColumns = true; // Allow reordering columns

            panel1.Controls.Add(dgvOrders);

            // Remove all the old controls related to placing an order
            // (You would need to list them by name or clear panel1 if all were added programmatically to panel1)
            // For simplicity, if all previous controls were added to panel1:
            // panel1.Controls.Clear(); // Use with caution if panel1 contains other design-time controls
            // A safer way is to remove specific controls if you named them
            // e.g., panel1.Controls.Remove(modelComboBox);
            // However, since they were created programmatically in InitializeCustomComponents,
            // we just won't create them in the new version of this method.

        }

        private void LoadUserOrders()
        {
            if (currentUser != null)
            {
                List<OrderDisplayItem> userOrders = DatabaseConnection.GetUserOrders(currentUser.UserID);

                if (userOrders != null && userOrders.Count > 0)
                {
                    // Bind the list of orders to the DataGridView
                    dgvOrders.DataSource = userOrders;

                    // Optional: Customize column headers if AutoGenerateColumns is true
                    // dgvOrders.Columns["OrderID"].HeaderText = "Order ID";
                    // dgvOrders.Columns["Car"].HeaderText = "Car Ordered";
                    // dgvOrders.Columns["TotalAmount"].HeaderText = "Total Amount";
                    // dgvOrders.Columns["OrderDate"].HeaderText = "Order Date";
                    // dgvOrders.Columns["Status"].HeaderText = "Status";

                    // Optional: Format currency and date columns
                    dgvOrders.Columns["TotalAmount"].DefaultCellStyle.Format = "C2"; // Currency format (adjust culture if needed)
                    dgvOrders.Columns["OrderDate"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm"; // Date and time format
                }
                else
                {
                    // Display a message if no orders are found
                    Label noOrdersLabel = new Label();
                    noOrdersLabel.Text = "No orders found for this user.";
                    noOrdersLabel.Font = labelFont;
                    noOrdersLabel.ForeColor = lightTextColor;
                    noOrdersLabel.AutoSize = true;
                    noOrdersLabel.Location = new Point(20, 80); // Position below the title line
                    panel1.Controls.Add(noOrdersLabel);

                    // Hide the DataGridView if no orders
                    dgvOrders.Visible = false;
                }
            }
            else
            {
                // Handle case where user is not logged in (should be prevented by Form1, but good for robustness)
                Label loginRequiredLabel = new Label();
                loginRequiredLabel.Text = "Please log in to view your orders.";
                loginRequiredLabel.Font = titleFont;
                loginRequiredLabel.ForeColor = Color.Red;
                loginRequiredLabel.AutoSize = true;
                loginRequiredLabel.Location = new Point((panel1.ClientSize.Width - loginRequiredLabel.Width) / 2, panel1.ClientSize.Height / 2 - 20); // Center message
                panel1.Controls.Add(loginRequiredLabel);

                // Hide the DataGridView if no user
                dgvOrders.Visible = false;
            }
        }

        // Remove all helper methods related to creating input controls
        // (CreateLabel, CreateComboBox, CreateStyledCheckBox, ShowError, ShowConfirmation)

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // Custom border in accent color
            ControlPaint.DrawBorder(e.Graphics, panel1.ClientRectangle,
                                  primaryColor, 1, ButtonBorderStyle.Solid,
                                  primaryColor, 1, ButtonBorderStyle.Solid,
                                  primaryColor, 1, ButtonBorderStyle.Solid,
                                  primaryColor, 1, ButtonBorderStyle.Solid);
        }

        // Keep any other event handlers generated by the designer if they exist (e.g., order_Load)
        // private void order_Load(object sender, EventArgs e) { }
    }
}