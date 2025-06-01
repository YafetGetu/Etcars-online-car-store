using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms; // Added for MessageBox
using System.Collections.Generic; // Added for List

namespace carstore
{
    // Make sure you have the 'Car' class defined within this namespace or in a separate file.
    // public class Car { ... } // If you define it here


    public class DatabaseConnection
    {
        // !! IMPORTANT !!
        // Replace "Your_Connection_String_Here" with the actual connection string
        // you copied from the Server Explorer properties.
        private static string connectionString = "Data Source=.;Initial Catalog=CarStore1;Integrated Security=True;Encrypt=False;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public static (bool success, bool isAdmin) ValidateUser(string username, string password)
        {
            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    // Use the actual column names from your database (fullName, password)
                    string query = "SELECT IsAdmin FROM Users WHERE fullName = @fullName AND password = @password";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@fullName", username); // Parameter name matches query placeholder
                        cmd.Parameters.AddWithValue("@password", password); // Parameter name matches query placeholder
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            bool isAdmin = Convert.ToBoolean(result);
                            return (true, isAdmin);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // In a real app, log this properly instead of just showing a message box
                    MessageBox.Show("Database validation error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return (false, false);
        }

        public static bool RegisterUser(string fullName, string email, string password, string phoneNumber, string gender)
        {
            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    // This query and parameters already match your database schema
                    string query = @"INSERT INTO Users (fullName, email, password, phoneNumber, gender, IsAdmin)
                                   VALUES (@fullName, @email, @password, @phoneNumber, @gender, 0)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@fullName", fullName);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@password", password); // Remember to hash passwords in a real app
                        cmd.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                        cmd.Parameters.AddWithValue("@gender", gender);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception
                    MessageBox.Show("Registration database error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        // --- Method to Fetch Cars ---
        public static List<Car> GetAllCars()
        {
            List<Car> cars = new List<Car>();
            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    // Ensure column names match your 'Cars' table exactly
                    string query = "SELECT CarID, Brand, Model, Year, Price, Description, ImagePath, IsAvailable FROM Cars WHERE IsAvailable = 1"; // Only get available cars
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cars.Add(new Car
                                {
                                    CarID = reader.GetInt32(reader.GetOrdinal("CarID")),
                                    Brand = reader.GetString(reader.GetOrdinal("Brand")),
                                    Model = reader.GetString(reader.GetOrdinal("Model")),
                                    Year = reader.GetInt32(reader.GetOrdinal("Year")),
                                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                    // Handle potential NULLs for Description and ImagePath (Base64)
                                    Description = reader["Description"] == DBNull.Value ? string.Empty : reader.GetString(reader.GetOrdinal("Description")),
                                    ImageBase64 = reader["ImagePath"] == DBNull.Value ? null : reader.GetString(reader.GetOrdinal("ImagePath")),
                                    IsAvailable = reader.GetBoolean(reader.GetOrdinal("IsAvailable"))
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading cars from database: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // In a real app, log this exception
                }
            }
            return cars;
        }
    }
}