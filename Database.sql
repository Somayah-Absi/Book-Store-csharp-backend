--------------------------------- Create OrderProduct Table

CREATE TABLE OrderProduct (
    OrderProductID SERIAL PRIMARY KEY,
    Quantity INT DEFAULT 1 CHECK (Quantity >= 1),
    OrderID INT,
    ProductID INT,
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    FOREIGN KEY (ProductID) REFERENCES Product(ProductID)
);

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



