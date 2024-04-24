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

-- Add columns to Category table & update them
ALTER TABLE Category
ADD COLUMN CategorySlug VARCHAR(100) UNIQUE,
ADD COLUMN CategoryDescription TEXT,
ADD COLUMN CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP;

UPDATE Category
SET
    CategorySlug = 
        CASE
            WHEN CategoryName = 'Clothing' THEN 'clothing'
            WHEN CategoryName = 'Beauty Products' THEN 'beauty-products'
            WHEN CategoryName = 'Shoes & Accessories' THEN 'shoes-and-accessories'
            WHEN CategoryName = 'Toys & Games' THEN 'toys-and-games'
            WHEN CategoryName = 'Arts & Crafts' THEN 'arts-and-crafts'
            WHEN CategoryName = 'Electronics' THEN 'electronics'
        END,
    CategoryDescription =
        CASE
            WHEN CategoryName = 'Clothing' THEN 'Discover the latest trends in fashion with our Clothing category. From casual wear to elegant attire, find the perfect outfit for any occasion.'
            WHEN CategoryName = 'Beauty Products' THEN 'Enhance your natural beauty and pamper yourself with our Beauty & Personal Care products. Explore a range of skincare, cosmetics, and grooming essentials.'
            WHEN CategoryName = 'Shoes & Accessories' THEN 'Step out in style with our Shoes & Accessories collection. Whether you''re looking for trendy footwear or statement accessories, we''ve got you covered.'
            WHEN CategoryName = 'Toys & Games' THEN 'Spark imagination and endless fun with our Toys & Games selection. From educational toys to exciting games, there''s something for every age and interest.'
            WHEN CategoryName = 'Arts & Crafts' THEN 'Unleash your creativity with our Arts & Crafts supplies. Dive into a world of colors, textures, and possibilities to bring your artistic visions to life.'
            WHEN CategoryName = 'Electronics' THEN 'Stay connected and up-to-date with our Electronics category. Discover the latest gadgets, devices, and tech innovations to elevate your digital lifestyle.'
        END;

\x
SELECT * FROM Category;

ALTER TABLE Category
ALTER COLUMN CategorySlug SET NOT NULL;

-- User
CREATE TABLE users( 
UserID SERIAL PRIMARY KEY, 
FirstName VARCHAR(50) NOT NULL, 
LastName VARCHAR(50) NOT NULL,
Email VARCHAR(100) UNIQUE NOT NULL, 
Password VARCHAR(100) NOT NULL,  
CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
Role VARCHAR(10) NOT NULL CHECK (Role IN ('User', 'Admin'))
);

INSERT INTO users (FirstName, LastName, Email, Password , Role)
VALUES
    ('Raghad', 'Alotaibi', 'Raghad@gmail.com', '11112','Admin'),
    ('Somayah', 'Absi', 'somayah@gmail.com', '222221','User'),
    ('Nada', 'Yhaya', 'Nada@gmail.com', '333311','User'),
    ('Albandri', 'Alotaibi', 'Albandri@gmail.com', '11442','User');

SELECT * FROM users; 
SELECT UserID, FirstName, LastName FROM users;
SELECT * FROM users WHERE UserID = 1;
UPDATE users SET FirstName='sadeem' WHERE UserID = 2;
DELETE FROM users WHERE UserID = 1;

-- Add columns to User table & update them
ALTER TABLE users ADD COLUMN Mobile varchar(50) UNIQUE ;
ALTER TABLE users
RENAME COLUMN Password TO UserPassword;
ALTER TABLE users
RENAME COLUMN Role TO UserRole;

UPDATE users
SET Mobile= '0539482044'
WHERE UserID=3;

UPDATE users
SET Mobile= '0539444478'
WHERE UserID=4;

UPDATE users
SET Mobile= '0556677343'
WHERE UserID=2;

ALTER TABLE users
ALTER COLUMN Mobile SET NOT NULL;


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

-- Add columns to Product table & update them

ALTER TABLE Product
ADD COLUMN CreateDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
ADD COLUMN Productslug VARCHAR(100);

UPDATE Product
SET
    Productslug = 
        CASE
            WHEN ProductName = 'Perfume' THEN 'perfume'
            WHEN ProductName = 'Lipstick' THEN 'lipstick'
            WHEN ProductName = 'Sunglasses' THEN 'sunglasses'
        END,
    ProductDescription =
        CASE
            WHEN  ProductName = 'Perfume' THEN 'Elegant fragrance for all occasions'
            WHEN ProductName = 'Lipstick' THEN 'Add a pop of color to your lips with our creamy lipstick.'
            WHEN ProductName = 'Sunglasses' THEN 'Stay stylish and protected from the sun with our fashionable sunglasses’
        END;
\x

SELECT * FROM Product;


-- Order
CREATE TABLE Orders( 
OrderID SERIAL PRIMARY KEY, 
OrderDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP, 
OrderStatus VARCHAR(50) NOT NULL,UserID INTEGER,
FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

INSERT INTO Orders(OrderDate, OrderStatus, UserID)
VALUES
    ( '2024-02-24', 'Processing', 3),
    ('2024-01-22', 'Closure', 2),
    ( '2024-04-23', 'Canceled', 4);

SELECT
    CONCAT(c.FirstName, ' ', c.LastName) AS CustomerName,
    b.OrderID,
    b.OrderStatus,
    b.OrderDate
FROM  
    Orders b
INNER JOIN 
    Users c ON c.UserID = b.UserID;

SELECT  CONCAT(c.FirstName, ' ', c.LastName) AS CustomerName , b.OrderDate AS "Last Orders :"
FROM Orders b
INNER JOIN Users c ON c.UserID = b.UserID
ORDER BY b.OrderDate DESC;

DELETE FROM Orders
WHERE OrderStatus = 'Canceled';

INSERT INTO Orders( OrderDate, OrderStatus, UserID)
VALUES ('2024-04-23','Processing ',4);