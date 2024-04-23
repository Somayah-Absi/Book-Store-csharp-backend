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