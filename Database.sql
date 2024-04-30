--------------------------------- Create User Table

CREATE TABLE users( 
UserID SERIAL PRIMARY KEY, 
FirstName VARCHAR(50) NOT NULL, 
LastName VARCHAR(50) NOT NULL,
Email VARCHAR(100) UNIQUE NOT NULL, 
Password VARCHAR(100) NOT NULL,  
CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
Role VARCHAR(10) NOT NULL CHECK (Role IN ('User', 'Admin'))
);

--------------------------------- Create Product Table

CREATE TABLE Product(
ProductID SERIAL PRIMARY KEY,
ProductName VARCHAR(50) NOT NULL,
ProductDescription TEXT NOT NULL,
ProductPrice NUMERIC(10, 2) NOT NULL,
ProductQuantityInStock INTEGER NOT NULL, 
CategoryID INTEGER,
FOREIGN KEY (CategoryID) REFERENCES Category(CategoryID)
);

--------------------------------- Create Order Table

CREATE TABLE Orders( 
OrderID SERIAL PRIMARY KEY, 
OrderDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP, 
OrderStatus VARCHAR(50)   NOT NULL,UserID INTEGER,
FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

--------------------------------- Create OrderProduct Table

CREATE TABLE OrderProduct (
    OrderProductID SERIAL PRIMARY KEY,
    Quantity INT DEFAULT 1 CHECK (Quantity >= 1),
    OrderID INT,
    ProductID INT,
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    FOREIGN KEY (ProductID) REFERENCES Product(ProductID)
);

---------------------------- Insert into User Table

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

---------------------------- Add columns to User table & update them

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

ALTER TABLE users
DROP UserRole;

ALTER TABLE users ADD COLUMN isAdmin BOOLEAN DEFAULT FALSE ;

ALTER TABLE users ADD COLUMN isBanned BOOLEAN DEFAULT FALSE ;


---------------------------- Insert into Product Table

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

---------------------------- Add columns to Product table & update them

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
            WHEN ProductName = 'Sunglasses' THEN 'Stay stylish and protected from the sun with our fashionable sunglasses'
        END;
\x
SELECT * FROM Product;

---------------------------- Insert into Order Table

INSERT INTO Orders(OrderDate, OrderStatus, UserID)
VALUES
    ( '2024-02-24', 'Processing', 3),
    ('2024-01-22', 'Closure', 2),
    ( '2024-04-23', 'Canceled', 4);

SELECT CONCAT(c.FirstName, ' ', c.LastName) AS CustomerName ,
    b.OrderID,
    b.OrderStatus,
    b.OrderDate
FROM  Orders b
INNER JOIN Users c USING(UserID);

SELECT  CONCAT(c.FirstName, ' ', c.LastName) AS CustomerName , b.OrderDate AS "Last Orders :"
FROM Orders b
INNER JOIN Users c USING(UserID)
ORDER BY b.OrderDate DESC;

DELETE FROM Orders
WHERE OrderStatus = 'Canceled';

INSERT INTO Orders( OrderDate, OrderStatus, UserID)
VALUES ('2024-04-23','Processing ',4);

---------------------------- Add columns to Order table & update them & add constrain
ALTER TABLE Orders ADD COLUMN Payment JSONB ;

UPDATE Orders
SET Payment = '{"method ": "Credit Card" }'
WHERE OrderID =1;

UPDATE Orders
SET Payment = '{"method ": "Credit Card" }'
WHERE OrderID =2;

UPDATE Orders
SET Payment = '{"method ": "Cash On Delivery" }'
WHERE OrderID =4;

ALTER TABLE Orders 
ALTER COLUMN OrderStatus SET DEFAULT 'Pending';

---------------------------- Insert into OrderProduct Table

INSERT INTO OrderProduct (Quantity, OrderID, ProductID)
VALUES 
    (2, 1, 3),
    (2, 1, 4),
    (2, 1, 4);

---------------------------- Add columns to OrderProduct table & update them

-- * Function get_total_order_price()
CREATE OR REPLACE FUNCTION get_total_order_price() RETURNS NUMERIC AS $$
DECLARE
    total_price NUMERIC := 0;
BEGIN
    SELECT
        SUM(OrderTotalPrice)
    INTO
        total_price
    FROM
        (
            SELECT
                COALESCE(SUM(p.ProductPrice * op.Quantity), 0) AS OrderTotalPrice
            FROM
                OrderProduct op
            JOIN
                Product p ON op.ProductID = p.ProductID
            GROUP BY
                op.OrderID
        ) AS OrderTotals;

    RETURN total_price;
END;
$$ LANGUAGE plpgsql;

SELECT
    op.OrderProductID,
    op.Quantity,
    op.OrderID,
    op.ProductID,
    COALESCE(SUM(p.ProductPrice * op.Quantity), 0) AS TotalPrice,
    p.ProductName,
    get_total_order_price() AS totalOrderPrice 
FROM
    OrderProduct op
JOIN
    Orders oc ON op.OrderID = oc.OrderID
JOIN
    Product p ON op.ProductID = p.ProductID
WHERE
    op.OrderID = 1
GROUP BY
    op.OrderProductID,
    op.Quantity,
    op.OrderID,
    op.ProductID,
    p.ProductName;

-- * CR(U)D Update operations
--  ? Function To Update Values
CREATE OR REPLACE FUNCTION update_order_product(
    order_product_id INT,
    new_quantity INT
) RETURNS VOID AS $$
BEGIN
    UPDATE OrderProduct SET Quantity = new_quantity WHERE OrderProductID = order_product_id;
END;
$$ LANGUAGE plpgsql;

SELECT update_order_product(1, 3);   

-- * CRU(D) Delete operations
--  * Function To Delete specific product on Order
CREATE OR REPLACE FUNCTION delete_order_product(
    order_product_id INT
) RETURNS VOID AS $$
BEGIN
    DELETE FROM OrderProduct WHERE OrderProductID = order_product_id;
END;
$$ LANGUAGE plpgsql;

SELECT delete_order_product(1);



