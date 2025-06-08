-- Create the carstore database
CREATE DATABASE IF NOT EXISTS carstoreDB;
USE carstoreDB;

-- Create Users table
CREATE TABLE IF NOT EXISTS Users (
    UserID INT AUTO_INCREMENT PRIMARY KEY,
    fullName VARCHAR(50) NOT NULL UNIQUE,
    password VARCHAR(100) NOT NULL, -- Remember to hash passwords in a real app
    email VARCHAR(100) NOT NULL UNIQUE,
    phoneNumber VARCHAR(20),
    gender VARCHAR(20),
    IsAdmin BOOLEAN DEFAULT 0,
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Create Cars table
CREATE TABLE IF NOT EXISTS Cars (
    CarID INT AUTO_INCREMENT PRIMARY KEY,
    Brand VARCHAR(50) NOT NULL,
    Model VARCHAR(50) NOT NULL,
    Year INT NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    Description TEXT,
    ImagePath LONGTEXT, -- For Base64 or image URLs
    IsAvailable BOOLEAN DEFAULT 1,
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Create Orders table
CREATE TABLE IF NOT EXISTS Orders (
    OrderID INT AUTO_INCREMENT PRIMARY KEY,
    UserID INT,
    CarID INT,
    OrderDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    Status VARCHAR(20) DEFAULT 'Pending',
    TotalAmount DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (CarID) REFERENCES Cars(CarID)
);

-- Insert default admin user
INSERT INTO Users (fullName, password, email, phoneNumber, gender, IsAdmin)
VALUES ('admin', 'Admin@123', 'admin@carstore.com', NULL, 'Not specified', 1);

-- Insert sample car data
INSERT INTO Cars (Brand, Model, Year, Price, Description, ImagePath)
VALUES
    ('Toyota', 'Camry', 2022, 25000.00, 'Reliable and fuel-efficient sedan', NULL),
    ('Honda', 'Civic', 2021, 22000.00, 'Sporty and compact car', NULL),
    ('Ford', 'Mustang', 2020, 35000.00, 'Classic American muscle car', NULL),
    ('BMW', 'X5', 2019, 45000.00, 'Luxury SUV with advanced features', NULL),
    ('Tesla', 'Model 3', 2023, 40000.00, 'Electric car with modern technology', NULL),
    ('Mercedes-Benz', 'E-Class', 2018, 50000.00, 'Premium sedan with elegant design', NULL),
    ('Audi', 'A4', 2022, 38000.00, 'Sophisticated and stylish sedan', NULL),
    ('Chevrolet', 'Silverado', 2021, 33000.00, 'Durable pickup truck for heavy-duty tasks', NULL),
    ('Hyundai', 'Tucson', 2020, 26000.00, 'Versatile SUV with modern safety features', NULL),
    ('Kia', 'Soul', 2019, 19000.00, 'Compact and funky urban car', NULL);

-- Insert sample orders
INSERT INTO Orders (UserID, CarID, OrderDate, Status, TotalAmount)
VALUES
    (1, 2, '2024-10-01 12:00:00', 'Shipped', 22000.00),
    (1, 4, '2024-11-15 08:30:00', 'Delivered', 45000.00);
