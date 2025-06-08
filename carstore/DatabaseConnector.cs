using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace carstore
{
    public class DatabaseConnection
    {
        private static string connectionString = "server=localhost;database=carstoreDB;uid=root;pwd=6240;SslMode=none;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        public static (bool success, bool isAdmin) ValidateUser(string username, string password)
        {
            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT IsAdmin FROM Users WHERE fullName = @fullName AND password = @password";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@fullName", username);
                        cmd.Parameters.AddWithValue("@password", password);
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
                    MessageBox.Show("Database validation error: " + ex.Message);
                }
            }
            return (false, false);
        }

        public static bool RegisterUser(string fullName, string email, string password, string phoneNumber, string gender)
        {
            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = @"INSERT INTO Users (fullName, email, password, phoneNumber, gender, IsAdmin)
                                     VALUES (@fullName, @email, @password, @phoneNumber, @gender, 0)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@fullName", fullName);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@password", password);
                        cmd.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                        cmd.Parameters.AddWithValue("@gender", gender);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Registration error: " + ex.Message);
                    return false;
                }
            }
        }

        public static List<Car> GetAllCars()
        {
            List<Car> cars = new List<Car>();
            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT CarID, Brand, Model, Year, Price, Description, ImagePath, IsAvailable FROM Cars WHERE IsAvailable = 1";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cars.Add(new Car
                            {
                                CarID = reader.GetInt32("CarID"),
                                Brand = reader.GetString("Brand"),
                                Model = reader.GetString("Model"),
                                Year = reader.GetInt32("Year"),
                                Price = reader.GetDecimal("Price"),
                                Description = reader["Description"] == DBNull.Value ? string.Empty : reader.GetString("Description"),
                                ImageBase64 = reader["ImagePath"] == DBNull.Value ? null : reader.GetString("ImagePath"),
                                IsAvailable = reader.GetBoolean("IsAvailable")
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading cars: " + ex.Message);
                }
            }
            return cars;
        }

        public static User GetUserData(string username)
        {
            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT UserID, fullName, email, phoneNumber, gender, IsAdmin FROM Users WHERE fullName = @fullName";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@fullName", username);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new User
                                {
                                    UserID = reader.GetInt32("UserID"),
                                    fullName = reader.GetString("fullName"),
                                    Email = reader.GetString("email"),
                                    PhoneNumber = reader.GetString("phoneNumber"),
                                    Gender = reader.GetString("gender"),
                                    IsAdmin = reader.GetBoolean("IsAdmin")
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving user: " + ex.Message);
                }
            }
            return null;
        }

        public static bool UpdateUser(User user)
        {
            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = @"UPDATE Users
                                     SET fullName = @fullName,
                                         email = @email,
                                         phoneNumber = @phoneNumber,
                                         gender = @gender
                                     WHERE UserID = @userID";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@fullName", user.fullName);
                        cmd.Parameters.AddWithValue("@email", user.Email);
                        cmd.Parameters.AddWithValue("@phoneNumber", user.PhoneNumber);
                        cmd.Parameters.AddWithValue("@gender", user.Gender);
                        cmd.Parameters.AddWithValue("@userID", user.UserID);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating user: " + ex.Message);
                    return false;
                }
            }
        }

        public static bool CreateOrder(int userId, int carId, decimal totalAmount)
        {
            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Orders (UserID, CarID, TotalAmount) VALUES (@userId, @carId, @totalAmount)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@carId", carId);
                        cmd.Parameters.AddWithValue("@totalAmount", totalAmount);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error creating order: " + ex.Message);
                    return false;
                }
            }
        }

        public static List<OrderDisplayItem> GetUserOrders(int userId)
        {
            List<OrderDisplayItem> orders = new List<OrderDisplayItem>();
            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = @"SELECT o.OrderID, c.Brand, c.Model, c.Year, o.TotalAmount, o.OrderDate, o.Status
                                     FROM Orders o
                                     JOIN Cars c ON o.CarID = c.CarID
                                     WHERE o.UserID = @userId
                                     ORDER BY o.OrderDate DESC";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                orders.Add(new OrderDisplayItem
                                {
                                    OrderID = reader.GetInt32("OrderID"),
                                    Car = $"{reader.GetString("Brand")} {reader.GetString("Model")} ({reader.GetInt32("Year")})",
                                    TotalAmount = reader.GetDecimal("TotalAmount"),
                                    OrderDate = reader.GetDateTime("OrderDate"),
                                    Status = reader.GetString("Status")
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading orders: " + ex.Message);
                }
            }
            return orders;
        }

        public static bool MarkCarAsUnavailable(int carId)
        {
            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE Cars SET IsAvailable = 0 WHERE CarID = @carId";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@carId", carId);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error marking car unavailable: " + ex.Message);
                    return false;
                }
            }
        }
    }
}
