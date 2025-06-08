using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace carstore
{
    public partial class AdminPage : Form
    {
        private string base64ImageString = null;

        // Declare the UI controls at the class level
        private DataGridView dataGridViewCars;
        private TextBox txtBrand;
        private TextBox txtModel;
        private TextBox txtYear;
        private TextBox txtPrice;
        private TextBox txtDescription;
        private PictureBox pictureBoxCar;
        private Button btnBrowseImage;
        private Button btnAddCar;
        private Button btnEditCar;
        private Button btnDeleteCar;

        // Order management controls
        private DataGridView dataGridViewOrders;
        private TextBox txtOrderId;
        private TextBox txtCustomerName;
        private TextBox txtCarDetails;
        private DateTimePicker dtpOrderDate;
        private ComboBox cmbOrderStatus;
        private TextBox txtTotalAmount;
        private Button btnUpdateOrder;
        private Button btnCancelOrder;

        private TabControl tabControlAdmin;
        private TabPage tabPageCars;
        private TabPage tabPageOrders;

        public AdminPage()
        {
            // Set up the form properties
            this.Text = "Car Store - Admin Panel";
            this.Size = new System.Drawing.Size(900, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(0, 40, 85);
            this.ForeColor = Color.White;

            // Create the TabControl and add it to the form
            tabControlAdmin = new TabControl();
            tabControlAdmin.Dock = DockStyle.Fill;
            tabControlAdmin.BackColor = Color.FromArgb(0, 40, 85);
            tabControlAdmin.ForeColor = Color.White;
            this.Controls.Add(tabControlAdmin);

            // Create the TabPages and add them to the TabControl
            tabPageCars = new TabPage("Manage Cars");
            tabPageCars.Name = "tabPageCars";
            tabPageCars.BackColor = Color.FromArgb(0, 40, 85);
            tabPageCars.ForeColor = Color.White;

            tabPageOrders = new TabPage("View Orders");
            tabPageOrders.Name = "tabPageOrders";
            tabPageOrders.BackColor = Color.FromArgb(0, 40, 85);
            tabPageOrders.ForeColor = Color.White;

            tabControlAdmin.TabPages.Add(tabPageCars);
            tabControlAdmin.TabPages.Add(tabPageOrders);

            // Set up UI
            SetupCarManagementTab(tabPageCars);
            SetupOrderViewingTab(tabPageOrders);

            // Load initial data
            LoadCars();
            LoadOrders();
        }

        private void SetupCarManagementTab(TabPage tab)
        {
            // Add DataGridView for cars
            dataGridViewCars = new DataGridView();
            dataGridViewCars.Dock = DockStyle.Top;
            dataGridViewCars.Height = 200;
            dataGridViewCars.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewCars.CellClick += dataGridViewCars_CellClick;
            dataGridViewCars.BackgroundColor = Color.FromArgb(0, 40, 85);
            dataGridViewCars.ForeColor = Color.White;
            dataGridViewCars.DefaultCellStyle.BackColor = Color.FromArgb(0, 40, 85);
            dataGridViewCars.DefaultCellStyle.ForeColor = Color.White;
            dataGridViewCars.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 40, 85);
            dataGridViewCars.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridViewCars.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 40, 85);
            dataGridViewCars.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            tab.Controls.Add(dataGridViewCars);

            // Add controls for car details
            int yPos = dataGridViewCars.Bottom + 20;
            int labelWidth = 100;
            int controlHeight = 25;
            int spacing = 10;
            int leftMargin = 20;
            int textBoxWidth = 200;

            // Brand
            Label labelBrand = new Label()
            {
                Text = "Brand:",
                Location = new Point(leftMargin, yPos),
                Width = labelWidth,
                Height = controlHeight,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(0, 40, 85)
            };
            txtBrand = new TextBox()
            {
                Location = new Point(leftMargin + labelWidth + spacing, yPos),
                Width = textBoxWidth,
                Height = controlHeight,
                BackColor = Color.FromArgb(0, 40, 85),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            tab.Controls.Add(labelBrand);
            tab.Controls.Add(txtBrand);
            yPos += controlHeight + spacing;

            // Model
            Label labelModel = new Label()
            {
                Text = "Model:",
                Location = new Point(leftMargin, yPos),
                Width = labelWidth,
                Height = controlHeight,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(0, 40, 85)
            };
            txtModel = new TextBox()
            {
                Location = new Point(leftMargin + labelWidth + spacing, yPos),
                Width = textBoxWidth,
                Height = controlHeight,
                BackColor = Color.FromArgb(0, 40, 85),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            tab.Controls.Add(labelModel);
            tab.Controls.Add(txtModel);
            yPos += controlHeight + spacing;

            // Year
            Label labelYear = new Label()
            {
                Text = "Year:",
                Location = new Point(leftMargin, yPos),
                Width = labelWidth,
                Height = controlHeight,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(0, 40, 85)
            };
            txtYear = new TextBox()
            {
                Location = new Point(leftMargin + labelWidth + spacing, yPos),
                Width = textBoxWidth,
                Height = controlHeight,
                BackColor = Color.FromArgb(0, 40, 85),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            tab.Controls.Add(labelYear);
            tab.Controls.Add(txtYear);
            yPos += controlHeight + spacing;

            // Price
            Label labelPrice = new Label()
            {
                Text = "Price:",
                Location = new Point(leftMargin, yPos),
                Width = labelWidth,
                Height = controlHeight,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(0, 40, 85)
            };
            txtPrice = new TextBox()
            {
                Location = new Point(leftMargin + labelWidth + spacing, yPos),
                Width = textBoxWidth,
                Height = controlHeight,
                BackColor = Color.FromArgb(0, 40, 85),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            tab.Controls.Add(labelPrice);
            tab.Controls.Add(txtPrice);
            yPos += controlHeight + spacing;

            // Description
            Label labelDescription = new Label()
            {
                Text = "Description:",
                Location = new Point(leftMargin, yPos),
                Width = labelWidth,
                Height = controlHeight,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(0, 40, 85)
            };
            txtDescription = new TextBox()
            {
                Location = new Point(leftMargin + labelWidth + spacing, yPos),
                Width = textBoxWidth * 2,
                Height = controlHeight * 3,
                Multiline = true,
                BackColor = Color.FromArgb(0, 40, 85),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            tab.Controls.Add(labelDescription);
            tab.Controls.Add(txtDescription);
            yPos += controlHeight * 3 + spacing;

            // Image
            Label labelImage = new Label()
            {
                Text = "Image:",
                Location = new Point(leftMargin, yPos),
                Width = labelWidth,
                Height = controlHeight,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(0, 40, 85)
            };
            pictureBoxCar = new PictureBox()
            {
                Location = new Point(leftMargin + labelWidth + spacing, yPos),
                Size = new Size(100, 100),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.White
            };
            btnBrowseImage = new Button()
            {
                Text = "Browse Image",
                Location = new Point(leftMargin + labelWidth + spacing + pictureBoxCar.Width + spacing, yPos),
                Width = 120,
                Height = controlHeight,
                BackColor = Color.FromArgb(0, 60, 120),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnBrowseImage.FlatAppearance.BorderColor = Color.White;
            btnBrowseImage.Click += btnBrowseImage_Click;
            tab.Controls.Add(labelImage);
            tab.Controls.Add(pictureBoxCar);
            tab.Controls.Add(btnBrowseImage);
            yPos += pictureBoxCar.Height + spacing;

            // Buttons
            btnAddCar = new Button()
            {
                Text = "Add Car",
                Location = new Point(leftMargin, yPos),
                Width = 100,
                Height = 30,
                BackColor = Color.FromArgb(0, 60, 120),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAddCar.FlatAppearance.BorderColor = Color.White;

            btnEditCar = new Button()
            {
                Text = "Edit Car",
                Location = new Point(leftMargin + btnAddCar.Width + spacing, yPos),
                Width = 100,
                Height = 30,
                BackColor = Color.FromArgb(0, 60, 120),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnEditCar.FlatAppearance.BorderColor = Color.White;

            btnDeleteCar = new Button()
            {
                Text = "Delete Car",
                Location = new Point(leftMargin + btnAddCar.Width + spacing + btnEditCar.Width + spacing, yPos),
                Width = 100,
                Height = 30,
                BackColor = Color.FromArgb(0, 60, 120),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnDeleteCar.FlatAppearance.BorderColor = Color.White;

            btnAddCar.Click += btnAddCar_Click;
            btnEditCar.Click += btnEditCar_Click;
            btnDeleteCar.Click += btnDeleteCar_Click;

            tab.Controls.Add(btnAddCar);
            tab.Controls.Add(btnEditCar);
            tab.Controls.Add(btnDeleteCar);
        }

        private void SetupOrderViewingTab(TabPage tab)
        {
            // Create a split container to divide the tab into two parts
            SplitContainer splitContainer = new SplitContainer();
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Orientation = Orientation.Horizontal;
            splitContainer.SplitterDistance = 300; // DataGridView will take 300 pixels
            tab.Controls.Add(splitContainer);

            // Add DataGridView for orders to the top panel
            dataGridViewOrders = new DataGridView();
            dataGridViewOrders.Dock = DockStyle.Fill;
            dataGridViewOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewOrders.CellClick += dataGridViewOrders_CellClick;
            dataGridViewOrders.BackgroundColor = Color.FromArgb(0, 40, 85);
            dataGridViewOrders.ForeColor = Color.White;
            dataGridViewOrders.DefaultCellStyle.BackColor = Color.FromArgb(0, 40, 85);
            dataGridViewOrders.DefaultCellStyle.ForeColor = Color.White;
            dataGridViewOrders.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 40, 85);
            dataGridViewOrders.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridViewOrders.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 40, 85);
            dataGridViewOrders.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            splitContainer.Panel1.Controls.Add(dataGridViewOrders);

            // Create a panel for order details in the bottom panel
            Panel orderDetailsPanel = new Panel();
            orderDetailsPanel.Dock = DockStyle.Fill;
            orderDetailsPanel.BackColor = Color.FromArgb(0, 40, 85);
            splitContainer.Panel2.Controls.Add(orderDetailsPanel);

            // Add controls for order details
            int yPos = 20;
            int labelWidth = 120;
            int controlHeight = 25;
            int spacing = 10;
            int leftMargin = 20;
            int textBoxWidth = 250;

            // Order ID
            Label labelOrderId = new Label()
            {
                Text = "Order ID:",
                Location = new Point(leftMargin, yPos),
                Width = labelWidth,
                Height = controlHeight,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(0, 40, 85)
            };
            txtOrderId = new TextBox()
            {
                Location = new Point(leftMargin + labelWidth + spacing, yPos),
                Width = textBoxWidth,
                Height = controlHeight,
                BackColor = Color.FromArgb(0, 40, 85),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                ReadOnly = true
            };
            orderDetailsPanel.Controls.Add(labelOrderId);
            orderDetailsPanel.Controls.Add(txtOrderId);
            yPos += controlHeight + spacing;

            // Customer Name
            Label labelCustomerName = new Label()
            {
                Text = "Customer Name:",
                Location = new Point(leftMargin, yPos),
                Width = labelWidth,
                Height = controlHeight,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(0, 40, 85)
            };
            txtCustomerName = new TextBox()
            {
                Location = new Point(leftMargin + labelWidth + spacing, yPos),
                Width = textBoxWidth,
                Height = controlHeight,
                BackColor = Color.FromArgb(0, 40, 85),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                ReadOnly = true
            };
            orderDetailsPanel.Controls.Add(labelCustomerName);
            orderDetailsPanel.Controls.Add(txtCustomerName);
            yPos += controlHeight + spacing;

            // Car Details
            Label labelCarDetails = new Label()
            {
                Text = "Car Details:",
                Location = new Point(leftMargin, yPos),
                Width = labelWidth,
                Height = controlHeight,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(0, 40, 85)
            };
            txtCarDetails = new TextBox()
            {
                Location = new Point(leftMargin + labelWidth + spacing, yPos),
                Width = textBoxWidth,
                Height = controlHeight,
                BackColor = Color.FromArgb(0, 40, 85),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                ReadOnly = true
            };
            orderDetailsPanel.Controls.Add(labelCarDetails);
            orderDetailsPanel.Controls.Add(txtCarDetails);
            yPos += controlHeight + spacing;

            // Order Date
            Label labelOrderDate = new Label()
            {
                Text = "Order Date:",
                Location = new Point(leftMargin, yPos),
                Width = labelWidth,
                Height = controlHeight,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(0, 40, 85)
            };
            dtpOrderDate = new DateTimePicker()
            {
                Location = new Point(leftMargin + labelWidth + spacing, yPos),
                Width = textBoxWidth,
                Height = controlHeight,
                Format = DateTimePickerFormat.Short,
                BackColor = Color.FromArgb(0, 40, 85),
                ForeColor = Color.White
            };
            orderDetailsPanel.Controls.Add(labelOrderDate);
            orderDetailsPanel.Controls.Add(dtpOrderDate);
            yPos += controlHeight + spacing;

            // Order Status
            Label labelOrderStatus = new Label()
            {
                Text = "Order Status:",
                Location = new Point(leftMargin, yPos),
                Width = labelWidth,
                Height = controlHeight,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(0, 40, 85)
            };
            cmbOrderStatus = new ComboBox()
            {
                Location = new Point(leftMargin + labelWidth + spacing, yPos),
                Width = textBoxWidth,
                Height = controlHeight,
                BackColor = Color.FromArgb(0, 40, 85),
                ForeColor = Color.White,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbOrderStatus.Items.AddRange(new object[] { "Pending", "Processing", "Shipped", "Delivered", "Cancelled" });
            orderDetailsPanel.Controls.Add(labelOrderStatus);
            orderDetailsPanel.Controls.Add(cmbOrderStatus);
            yPos += controlHeight + spacing;

            // Total Amount
            Label labelTotalAmount = new Label()
            {
                Text = "Total Amount:",
                Location = new Point(leftMargin, yPos),
                Width = labelWidth,
                Height = controlHeight,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(0, 40, 85)
            };
            txtTotalAmount = new TextBox()
            {
                Location = new Point(leftMargin + labelWidth + spacing, yPos),
                Width = textBoxWidth,
                Height = controlHeight,
                BackColor = Color.FromArgb(0, 40, 85),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                ReadOnly = true
            };
            orderDetailsPanel.Controls.Add(labelTotalAmount);
            orderDetailsPanel.Controls.Add(txtTotalAmount);
            yPos += controlHeight + spacing * 2;

            // Update Order button
            btnUpdateOrder = new Button()
            {
                Text = "Update Order",
                Location = new Point(leftMargin, yPos),
                Width = 120,
                Height = 30,
                BackColor = Color.FromArgb(0, 60, 120),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnUpdateOrder.FlatAppearance.BorderColor = Color.White;
            btnUpdateOrder.Click += btnUpdateOrder_Click;
            orderDetailsPanel.Controls.Add(btnUpdateOrder);

            // Cancel Order button
            btnCancelOrder = new Button()
            {
                Text = "Cancel Order",
                Location = new Point(leftMargin + btnUpdateOrder.Width + spacing, yPos),
                Width = 120,
                Height = 30,
                BackColor = Color.FromArgb(120, 0, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancelOrder.FlatAppearance.BorderColor = Color.White;
            btnCancelOrder.Click += btnCancelOrder_Click;
            orderDetailsPanel.Controls.Add(btnCancelOrder);

            // Refresh button at the bottom
            Button btnRefreshOrders = new Button()
            {
                Text = "Refresh Orders",
                Dock = DockStyle.Bottom,
                Height = 30,
                BackColor = Color.FromArgb(0, 60, 120),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRefreshOrders.FlatAppearance.BorderColor = Color.White;
            btnRefreshOrders.Click += (s, e) => LoadOrders();
            tab.Controls.Add(btnRefreshOrders);
        }

        private void LoadCars()
        {
            using (MySqlConnection conn = DatabaseConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT CarID, Brand, Model, Year, Price, Description, ImagePath FROM Cars";
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridViewCars.DataSource = dt;

                        if (dataGridViewCars.Columns.Contains("ImagePath"))
                        {
                            dataGridViewCars.Columns["ImagePath"].Visible = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading cars: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnBrowseImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg; *.jpeg; *.png; *.gif; *.bmp)|*.jpg; *.jpeg; *.png; *.gif; *.bmp|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Image selectedImage = Image.FromFile(openFileDialog.FileName);
                    pictureBoxCar.Image = selectedImage;

                    using (MemoryStream ms = new MemoryStream())
                    {
                        selectedImage.Save(ms, selectedImage.RawFormat);
                        byte[] imageBytes = ms.ToArray();
                        base64ImageString = Convert.ToBase64String(imageBytes);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading or converting image: " + ex.Message, "Image Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    base64ImageString = null;
                    pictureBoxCar.Image = null;
                }
            }
        }

        private void btnAddCar_Click(object sender, EventArgs e)
        {
            string brand = txtBrand.Text.Trim();
            string model = txtModel.Text.Trim();
            int year = 0;
            decimal price = 0;
            string description = txtDescription.Text.Trim();

            if (string.IsNullOrEmpty(brand) || string.IsNullOrEmpty(model) || string.IsNullOrEmpty(txtYear.Text) ||
                string.IsNullOrEmpty(txtPrice.Text) || base64ImageString == null)
            {
                MessageBox.Show("Please fill in all fields and select an image.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtYear.Text, out year))
            {
                MessageBox.Show("Please enter a valid Year.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out price))
            {
                MessageBox.Show("Please enter a valid Price.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection conn = DatabaseConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = @"INSERT INTO Cars (Brand, Model, Year, Price, Description, ImagePath, IsAvailable)
                                   VALUES (@Brand, @Model, @Year, @Price, @Description, @ImagePath, 1)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Brand", brand);
                        cmd.Parameters.AddWithValue("@Model", model);
                        cmd.Parameters.AddWithValue("@Year", year);
                        cmd.Parameters.AddWithValue("@Price", price);
                        cmd.Parameters.AddWithValue("@Description", description);
                        cmd.Parameters.AddWithValue("@ImagePath", base64ImageString);
                        cmd.ExecuteNonQuery();
                    }
                    LoadCars();
                    ClearCarInputFields();
                    MessageBox.Show("Car added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding car: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridViewCars_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridViewCars.Rows.Count)
            {
                if (!dataGridViewCars.Rows[e.RowIndex].IsNewRow)
                {
                    DataGridViewRow selectedRow = dataGridViewCars.Rows[e.RowIndex];

                    txtBrand.Text = selectedRow.Cells["Brand"].Value?.ToString() ?? string.Empty;
                    txtModel.Text = selectedRow.Cells["Model"].Value?.ToString() ?? string.Empty;
                    txtYear.Text = selectedRow.Cells["Year"].Value?.ToString() ?? string.Empty;
                    txtPrice.Text = selectedRow.Cells["Price"].Value?.ToString() ?? string.Empty;
                    txtDescription.Text = selectedRow.Cells["Description"].Value?.ToString() ?? string.Empty;

                    object imagePathValue = selectedRow.Cells["ImagePath"].Value;
                    if (imagePathValue != null && imagePathValue != DBNull.Value)
                    {
                        base64ImageString = imagePathValue.ToString();
                        if (!string.IsNullOrEmpty(base64ImageString))
                        {
                            try
                            {
                                byte[] imageBytes = Convert.FromBase64String(base64ImageString);
                                using (MemoryStream ms = new MemoryStream(imageBytes))
                                {
                                    pictureBoxCar.Image = Image.FromStream(ms);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error loading image from database: " + ex.Message, "Image Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                pictureBoxCar.Image = null;
                                base64ImageString = null;
                            }
                        }
                        else
                        {
                            pictureBoxCar.Image = null;
                            base64ImageString = null;
                        }
                    }
                    else
                    {
                        pictureBoxCar.Image = null;
                        base64ImageString = null;
                    }
                }
            }
        }

        private void btnEditCar_Click(object sender, EventArgs e)
        {
            if (dataGridViewCars.SelectedRows.Count > 0)
            {
                object carIdValue = dataGridViewCars.SelectedRows[0].Cells["CarID"].Value;
                if (carIdValue == null || carIdValue == DBNull.Value)
                {
                    MessageBox.Show("Cannot edit: Selected row does not have a valid Car ID.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int carId = Convert.ToInt32(carIdValue);

                string brand = txtBrand.Text.Trim();
                string model = txtModel.Text.Trim();
                int year = 0;
                decimal price = 0;
                string description = txtDescription.Text.Trim();

                if (string.IsNullOrEmpty(brand) || string.IsNullOrEmpty(model) || string.IsNullOrEmpty(txtYear.Text) ||
                    string.IsNullOrEmpty(txtPrice.Text) || base64ImageString == null)
                {
                    MessageBox.Show("Please fill in all fields and select an image.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(txtYear.Text, out year))
                {
                    MessageBox.Show("Please enter a valid Year.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txtPrice.Text, out price))
                {
                    MessageBox.Show("Please enter a valid Price.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (MySqlConnection conn = DatabaseConnection.GetConnection())
                {
                    try
                    {
                        conn.Open();
                        string query = @"UPDATE Cars SET Brand=@Brand, Model=@Model, Year=@Year, Price=@Price, 
                                         Description=@Description, ImagePath=@ImagePath WHERE CarID=@CarID";
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Brand", brand);
                            cmd.Parameters.AddWithValue("@Model", model);
                            cmd.Parameters.AddWithValue("@Year", year);
                            cmd.Parameters.AddWithValue("@Price", price);
                            cmd.Parameters.AddWithValue("@Description", description);
                            cmd.Parameters.AddWithValue("@ImagePath", base64ImageString);
                            cmd.Parameters.AddWithValue("@CarID", carId);
                            cmd.ExecuteNonQuery();
                        }
                        LoadCars();
                        ClearCarInputFields();
                        MessageBox.Show("Car updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error updating car: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a car to edit.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnDeleteCar_Click(object sender, EventArgs e)
        {
            if (dataGridViewCars.SelectedRows.Count > 0)
            {
                object carIdValue = dataGridViewCars.SelectedRows[0].Cells["CarID"].Value;
                if (carIdValue == null || carIdValue == DBNull.Value)
                {
                    MessageBox.Show("Cannot delete: Selected row does not have a valid Car ID.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int carId = Convert.ToInt32(carIdValue);

                DialogResult result = MessageBox.Show("Are you sure you want to delete this car?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    using (MySqlConnection conn = DatabaseConnection.GetConnection())
                    {
                        try
                        {
                            conn.Open();
                            string query = "DELETE FROM Cars WHERE CarID=@CarID";
                            using (MySqlCommand cmd = new MySqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@CarID", carId);
                                cmd.ExecuteNonQuery();
                            }
                            LoadCars();
                            ClearCarInputFields();
                            MessageBox.Show("Car deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error deleting car: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a car to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void LoadOrders()
        {
            using (MySqlConnection conn = DatabaseConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = @"SELECT
                                        o.OrderID,
                                        u.fullName AS CustomerName,
                                        CONCAT(c.Brand, ' ', c.Model) AS CarDetails,
                                        o.OrderDate,
                                        o.Status,
                                        o.TotalAmount
                                    FROM Orders o
                                    JOIN Users u ON o.UserID = u.UserID
                                    JOIN Cars c ON o.CarID = c.CarID";
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridViewOrders.DataSource = dt;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading orders: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridViewOrders_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridViewOrders.Rows.Count)
            {
                DataGridViewRow selectedRow = dataGridViewOrders.Rows[e.RowIndex];

                txtOrderId.Text = selectedRow.Cells["OrderID"].Value?.ToString() ?? string.Empty;
                txtCustomerName.Text = selectedRow.Cells["CustomerName"].Value?.ToString() ?? string.Empty;
                txtCarDetails.Text = selectedRow.Cells["CarDetails"].Value?.ToString() ?? string.Empty;

                if (DateTime.TryParse(selectedRow.Cells["OrderDate"].Value?.ToString(), out DateTime orderDate))
                {
                    dtpOrderDate.Value = orderDate;
                }
                else
                {
                    dtpOrderDate.Value = DateTime.Now;
                }

                cmbOrderStatus.SelectedItem = selectedRow.Cells["Status"].Value?.ToString() ?? "Pending";
                txtTotalAmount.Text = selectedRow.Cells["TotalAmount"].Value?.ToString() ?? string.Empty;
            }
        }

        private void btnUpdateOrder_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtOrderId.Text))
            {
                MessageBox.Show("Please select an order to update.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int orderId = Convert.ToInt32(txtOrderId.Text);
            DateTime orderDate = dtpOrderDate.Value;
            string status = cmbOrderStatus.SelectedItem?.ToString() ?? "Pending";

            using (MySqlConnection conn = DatabaseConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = @"UPDATE Orders SET 
                                    OrderDate=@OrderDate, 
                                    Status=@Status 
                                    WHERE OrderID=@OrderID";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@OrderDate", orderDate);
                        cmd.Parameters.AddWithValue("@Status", status);
                        cmd.Parameters.AddWithValue("@OrderID", orderId);
                        cmd.ExecuteNonQuery();
                    }
                    LoadOrders();
                    MessageBox.Show("Order updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating order: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancelOrder_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtOrderId.Text))
            {
                MessageBox.Show("Please select an order to cancel.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to cancel this order?", "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                int orderId = Convert.ToInt32(txtOrderId.Text);

                using (MySqlConnection conn = DatabaseConnection.GetConnection())
                {
                    try
                    {
                        conn.Open();
                        string query = "UPDATE Orders SET Status='Cancelled' WHERE OrderID=@OrderID";
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@OrderID", orderId);
                            cmd.ExecuteNonQuery();
                        }
                        LoadOrders();
                        MessageBox.Show("Order cancelled successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error cancelling order: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ClearCarInputFields()
        {
            txtBrand.Clear();
            txtModel.Clear();
            txtYear.Clear();
            txtPrice.Clear();
            txtDescription.Clear();
            pictureBoxCar.Image = null;
            base64ImageString = null;
            dataGridViewCars.ClearSelection();
        }

        private void AdminPage_Load(object sender, EventArgs e)
        {
            // Optional: Any initialization code when the form loads
        }
    }
}