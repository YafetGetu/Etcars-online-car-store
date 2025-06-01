using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace carstore
{
    // Ensure this is 'partial' if you intend to use a designer later, 
    // but for programmatic UI, it's not strictly necessary.
    public partial class AdminPage : Form
    {
        private string base64ImageString = null; // To store the Base64 string of the selected image

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
        private DataGridView dataGridViewOrders;
        private TabControl tabControlAdmin;
        private TabPage tabPageCars;
        private TabPage tabPageOrders;


        public AdminPage()
        {
            // InitializeComponent(); // Remove or comment out this line if building UI programmatically

            // Set up the form properties
            this.Text = "Car Store - Admin Panel";
            this.Size = new System.Drawing.Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen; // Center the form

            // Create the TabControl and add it to the form
            tabControlAdmin = new TabControl();
            tabControlAdmin.Dock = DockStyle.Fill; // Make the TabControl fill the form
            this.Controls.Add(tabControlAdmin);

            // Create the TabPages and add them to the TabControl
            tabPageCars = new TabPage("Manage Cars");
            tabPageCars.Name = "tabPageCars"; // Give it a name
            tabPageOrders = new TabPage("View Orders");
            tabPageOrders.Name = "tabPageOrders"; // Give it a name
            tabControlAdmin.TabPages.Add(tabPageCars);
            tabControlAdmin.TabPages.Add(tabPageOrders);

            // Call methods to set up UI within the created TabPages, passing the pages
            SetupCarManagementTab(tabPageCars);
            SetupOrderViewingTab(tabPageOrders);

            // Load initial data
            LoadCars();
            LoadOrders();
        }

        // --- Setup UI Elements Programmatically ---
        // This method takes a TabPage and adds the car management controls to it
        private void SetupCarManagementTab(TabPage tab)
        {
            // Add DataGridView for cars
            dataGridViewCars = new DataGridView();
            dataGridViewCars.Dock = DockStyle.Top;
            dataGridViewCars.Height = 200;
            dataGridViewCars.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewCars.CellClick += dataGridViewCars_CellClick; // Event handler for selecting a car
            tab.Controls.Add(dataGridViewCars); // Add to the passed-in TabPage

            // Add controls for car details
            int yPos = dataGridViewCars.Bottom + 20;
            int labelWidth = 100;
            int controlHeight = 25;
            int spacing = 10;
            int leftMargin = 20;
            int textBoxWidth = 200;

            // Brand
            Label labelBrand = new Label() { Text = "Brand:", Location = new Point(leftMargin, yPos), Width = labelWidth, Height = controlHeight };
            txtBrand = new TextBox() { Location = new Point(leftMargin + labelWidth + spacing, yPos), Width = textBoxWidth, Height = controlHeight };
            tab.Controls.Add(labelBrand);
            tab.Controls.Add(txtBrand);
            yPos += controlHeight + spacing;

            // Model
            Label labelModel = new Label() { Text = "Model:", Location = new Point(leftMargin, yPos), Width = labelWidth, Height = controlHeight };
            txtModel = new TextBox() { Location = new Point(leftMargin + labelWidth + spacing, yPos), Width = textBoxWidth, Height = controlHeight };
            tab.Controls.Add(labelModel);
            tab.Controls.Add(txtModel);
            yPos += controlHeight + spacing;

            // Year
            Label labelYear = new Label() { Text = "Year:", Location = new Point(leftMargin, yPos), Width = labelWidth, Height = controlHeight };
            txtYear = new TextBox() { Location = new Point(leftMargin + labelWidth + spacing, yPos), Width = textBoxWidth, Height = controlHeight };
            tab.Controls.Add(labelYear);
            tab.Controls.Add(txtYear);
            yPos += controlHeight + spacing;

            // Price
            Label labelPrice = new Label() { Text = "Price:", Location = new Point(leftMargin, yPos), Width = labelWidth, Height = controlHeight };
            txtPrice = new TextBox() { Location = new Point(leftMargin + labelWidth + spacing, yPos), Width = textBoxWidth, Height = controlHeight };
            tab.Controls.Add(labelPrice);
            tab.Controls.Add(txtPrice);
            yPos += controlHeight + spacing;

            // Description
            Label labelDescription = new Label() { Text = "Description:", Location = new Point(leftMargin, yPos), Width = labelWidth, Height = controlHeight };
            txtDescription = new TextBox() { Location = new Point(leftMargin + labelWidth + spacing, yPos), Width = textBoxWidth * 2, Height = controlHeight * 3, Multiline = true };
            tab.Controls.Add(labelDescription);
            tab.Controls.Add(txtDescription);
            yPos += controlHeight * 3 + spacing;


            // Image
            Label labelImage = new Label() { Text = "Image:", Location = new Point(leftMargin, yPos), Width = labelWidth, Height = controlHeight };
            pictureBoxCar = new PictureBox() { Location = new Point(leftMargin + labelWidth + spacing, yPos), Size = new Size(100, 100), BorderStyle = BorderStyle.FixedSingle, SizeMode = PictureBoxSizeMode.Zoom };
            btnBrowseImage = new Button() { Text = "Browse Image", Location = new Point(leftMargin + labelWidth + spacing + pictureBoxCar.Width + spacing, yPos), Width = 120, Height = controlHeight };
            btnBrowseImage.Click += btnBrowseImage_Click;
            tab.Controls.Add(labelImage);
            tab.Controls.Add(pictureBoxCar);
            tab.Controls.Add(btnBrowseImage);
            yPos += pictureBoxCar.Height + spacing;

            // Buttons
            btnAddCar = new Button() { Text = "Add Car", Location = new Point(leftMargin, yPos), Width = 100, Height = 30 };
            btnEditCar = new Button() { Text = "Edit Car", Location = new Point(leftMargin + btnAddCar.Width + spacing, yPos), Width = 100, Height = 30 };
            btnDeleteCar = new Button() { Text = "Delete Car", Location = new Point(leftMargin + btnAddCar.Width + spacing + btnEditCar.Width + spacing, yPos), Width = 100, Height = 30 };

            btnAddCar.Click += btnAddCar_Click;
            btnEditCar.Click += btnEditCar_Click;
            btnDeleteCar.Click += btnDeleteCar_Click;

            tab.Controls.Add(btnAddCar);
            tab.Controls.Add(btnEditCar);
            tab.Controls.Add(btnDeleteCar);
        }

        // This method takes a TabPage and adds the order viewing controls to it
        private void SetupOrderViewingTab(TabPage tab)
        {
            // Add DataGridView for orders
            dataGridViewOrders = new DataGridView();
            dataGridViewOrders.Dock = DockStyle.Fill;
            dataGridViewOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tab.Controls.Add(dataGridViewOrders); // Add to the passed-in TabPage

            // You might want a button to refresh the order list
            Button btnRefreshOrders = new Button() { Text = "Refresh Orders", Dock = DockStyle.Bottom, Height = 30 };
            btnRefreshOrders.Click += (s, e) => LoadOrders();
            tab.Controls.Add(btnRefreshOrders); // Add to the passed-in TabPage

            // Make sure DataGridView is above the button
            dataGridViewOrders.BringToFront();
        }


        // --- Database Interaction Methods ---

        private void LoadCars()
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // Select the Base64 string stored in the ImagePath column
                    string query = "SELECT CarID, Brand, Model, Year, Price, Description, ImagePath FROM Cars";
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridViewCars.DataSource = dt;

                        // Hide the Base64 column in the grid for better readability
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
                    // Load the image into the PictureBox
                    Image selectedImage = Image.FromFile(openFileDialog.FileName);
                    pictureBoxCar.Image = selectedImage;

                    // Convert the image to Base64 string
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
                    base64ImageString = null; // Clear the stored Base64 string on error
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

            // Basic validation
            if (string.IsNullOrEmpty(brand) || string.IsNullOrEmpty(model) || string.IsNullOrEmpty(txtYear.Text) || string.IsNullOrEmpty(txtPrice.Text) || base64ImageString == null)
            {
                MessageBox.Show("Please fill in Brand, Model, Year, Price, select an Image, and provide a Description.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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


            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = @"INSERT INTO Cars (Brand, Model, Year, Price, Description, ImagePath, IsAvailable)
                                   VALUES (@Brand, @Model, @Year, @Price, @Description, @ImagePath, 1)"; // Store Base64 in ImagePath
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Brand", brand);
                        cmd.Parameters.AddWithValue("@Model", model);
                        cmd.Parameters.AddWithValue("@Year", year);
                        cmd.Parameters.AddWithValue("@Price", price);
                        cmd.Parameters.AddWithValue("@Description", description);
                        cmd.Parameters.AddWithValue("@ImagePath", base64ImageString); // Store the Base64 string
                        cmd.ExecuteNonQuery();
                    }
                    LoadCars(); // Refresh the list
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
            if (e.RowIndex >= 0 && e.RowIndex < dataGridViewCars.Rows.Count) // Add bounds check
            {
                // Check if the clicked row is not the new row placeholder
                if (!dataGridViewCars.Rows[e.RowIndex].IsNewRow)
                {
                    DataGridViewRow selectedRow = dataGridViewCars.Rows[e.RowIndex];

                    // Populate the input fields
                    // Use TryGetValue to handle potential DBNull values gracefully
                    txtBrand.Text = selectedRow.Cells["Brand"].Value?.ToString() ?? string.Empty;
                    txtModel.Text = selectedRow.Cells["Model"].Value?.ToString() ?? string.Empty;
                    txtYear.Text = selectedRow.Cells["Year"].Value?.ToString() ?? string.Empty;
                    txtPrice.Text = selectedRow.Cells["Price"].Value?.ToString() ?? string.Empty;
                    txtDescription.Text = selectedRow.Cells["Description"].Value?.ToString() ?? string.Empty;

                    // Load and display the image from the Base64 string
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
                                // Log or show a more specific error if Base64 conversion fails
                                MessageBox.Show("Error loading image from database: " + ex.Message, "Image Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                pictureBoxCar.Image = null; // Clear picture box on error
                                base64ImageString = null; // Clear string if it was invalid Base64
                            }
                        }
                        else
                        {
                            pictureBoxCar.Image = null; // Clear picture box if Base64 string is empty
                            base64ImageString = null;
                        }
                    }
                    else
                    {
                        pictureBoxCar.Image = null; // Clear picture box if no image data in DB
                        base64ImageString = null;
                    }
                }
            }
        }


        private void btnEditCar_Click(object sender, EventArgs e)
        {
            if (dataGridViewCars.SelectedRows.Count > 0)
            {
                // Get the CarID from the selected row
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

                // Basic validation
                if (string.IsNullOrEmpty(brand) || string.IsNullOrEmpty(model) || string.IsNullOrEmpty(txtYear.Text) || string.IsNullOrEmpty(txtPrice.Text) || base64ImageString == null)
                {
                    MessageBox.Show("Please fill in Brand, Model, Year, Price, select an Image, and provide a Description.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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


                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    try
                    {
                        conn.Open();
                        string query = @"UPDATE Cars SET Brand=@Brand, Model=@Model, Year=@Year, Price=@Price, Description=@Description, ImagePath=@ImagePath
                                         WHERE CarID=@CarID";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Brand", brand);
                            cmd.Parameters.AddWithValue("@Model", model);
                            cmd.Parameters.AddWithValue("@Year", year);
                            cmd.Parameters.AddWithValue("@Price", price);
                            cmd.Parameters.AddWithValue("@Description", description);
                            cmd.Parameters.AddWithValue("@ImagePath", base64ImageString); // Update with the current Base64 string
                            cmd.Parameters.AddWithValue("@CarID", carId);
                            cmd.ExecuteNonQuery();
                        }
                        LoadCars(); // Refresh the list
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
                // Get the CarID from the selected row
                object carIdValue = dataGridViewCars.SelectedRows[0].Cells["CarID"].Value;
                if (carIdValue == null || carIdValue == DBNull.Value)
                {
                    MessageBox.Show("Cannot delete: Selected row does not have a valid Car ID.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int carId = Convert.ToInt32(carIdValue);

                // Confirm deletion
                DialogResult result = MessageBox.Show("Are you sure you want to delete this car?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    using (SqlConnection conn = DatabaseConnection.GetConnection())
                    {
                        try
                        {
                            conn.Open();
                            string query = "DELETE FROM Cars WHERE CarID=@CarID";
                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@CarID", carId);
                                cmd.ExecuteNonQuery();
                            }
                            LoadCars(); // Refresh the list
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
            // Assuming you have a DataGridView named 'dataGridViewOrders' on the 'View Orders' tab
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = @"SELECT
                                        o.OrderID,
                                        u.fullName,
                                        c.Brand + ' ' + c.Model AS Car, -- Concatenate Brand and Model for display
                                        o.OrderDate,
                                        o.Status,
                                        o.TotalAmount
                                    FROM Orders o
                                    JOIN Users u ON o.UserID = u.UserID
                                    JOIN Cars c ON o.CarID = c.CarID";
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
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


        // --- Helper Method ---
        private void ClearCarInputFields()
        {
            txtBrand.Clear();
            txtModel.Clear();
            txtYear.Clear();
            txtPrice.Clear();
            txtDescription.Clear();
            pictureBoxCar.Image = null;
            base64ImageString = null; // Clear the Base64 string
            // Optional: Clear DataGridView selection
            dataGridViewCars.ClearSelection();
        }

        // If you were using the designer, these would be in AdminPage.Designer.cs
        // private void InitializeComponent()
        // {
        //    this.SuspendLayout();
        //    //
        //    // AdminPage
        //    //
        //    this.ClientSize = new System.Drawing.Size(284, 261);
        //    this.Name = "AdminPage";
        //    this.ResumeLayout(false);
        // }
    }
}