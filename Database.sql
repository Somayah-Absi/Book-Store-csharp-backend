-- Category
CREATE TABLE Category(
CategoryID SERIAL PRIMARY KEY,
CategoryName VARCHAR(50) UNIQUE NOT NULL
);

INSERT INTO Category (CategoryName)
VALUES 
    ('Clothing'),
    ('Beauty & Personal Care'),
    ('Shoes & Accessories'),
    ('Sports & Outdoors'),
    ('Toys & Games'),
    ('Arts & Crafts'),
    ('Home & Kitchen'),
    ('Electronics');

SELECT * FROM Category;
DELETE FROM Category
WHERE CategoryName = 'Sports & Outdoors'
OR CategoryName = 'Home & Kitchen';
UPDATE Category 
SET CategoryName = 'Beauty Products' 
WHERE CategoryID = 2;

-- Customer
CREATE TABLE Customer( 
CustomerID SERIAL PRIMARY KEY, 
CustomerFirstName VARCHAR(50) NOT NULL, 
CustomerLastName VARCHAR(50) NOT NULL,
CustomerEmail VARCHAR(100) UNIQUE NOT NULL, 
CustomerPassword VARCHAR(100) NOT NULL,  
CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

INSERT INTO Customer (CustomerFirstName, CustomerLastName, CustomerEmail, CustomerPassword)
VALUES
    ('Raghad', 'Alotaibi', 'Raghad@gmail.com', '11112'),
    ('Somayah', 'Absi', 'somayah@gmail.com', '222221'),
    ('Nada', 'Yhaya', 'Nada@gmail.com', '333311'),
    ('Albandri', 'Alotaibi', 'Albandri@gmail.com', '11442');


SELECT * FROM Customer; 
SELECT CustomerID, CustomerFirstName, CustomerLastName FROM Customer;
SELECT * FROM Customer WHERE CustomerID = 1;
UPDATE Customer SET CustomerFirstName='sadeem' WHERE CustomerID = 2;
DELETE FROM Customer WHERE CustomerID = 1;


-- Product 
CREATE TABLE Product(
ProductID SERIAL PRIMARY KEY,
ProductName VARCHAR(50) NOT NULL,
ProductDescription TEXT NOT NULL,
ProductPrice NUMERIC(10, 2) NOT NULL,
ProductQuantityInStock INTEGER NOT NULL, 
CategoryID INTEGER,
FOREIGN KEY (CategoryID) REFERENCES Category(CategoryID)
);

INSERT INTO Product (ProductName, ProductDescription, ProductPrice, ProductQuantityInStock, CategoryID)
VALUES
('Perfume', 'Elegant fragrance for all occasions', 59.99, 50, 2),
 ('Sunscreen', 'Protect your skin from harmful UV rays', 100.98, 89, 2),
('Lipstick', 'Add a pop of color to your lips with our creamy lipstick.', 25.55, 15, 2),
('Sunglasses', 'Stay stylish and protected from the sun with our fashionable sunglasses.', 45.75, 25, 3);

SELECT * FROM Product;
SELECT ProductID, ProductName, ProductPrice FROM Product;
SELECT * FROM Product WHERE CategoryID = 2;
SELECT * FROM Product WHERE ProductName = 'Sunscreen';
UPDATE Product 
SET ProductPrice = 90.00 
WHERE ProductID = 2;
UPDATE Product 
SET ProductPrice = 70.99, ProductQuantityInStock = 150 
WHERE ProductID = 4;
SELECT * FROM Product;
DELETE FROM Product WHERE ProductID = 2;




