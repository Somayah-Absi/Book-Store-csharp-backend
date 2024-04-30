--------------------------------- Create Database

CREATE DATABASE ecommerce_sda; 

--------------------------------- Create Category Table

CREATE TABLE category(
category_id SERIAL PRIMARY KEY,
category_name VARCHAR(50) UNIQUE NOT NULL,
category_slug VARCHAR(100) UNIQUE NOT NULL,
category_description TEXT,
created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

---------------------------- Insert into Category Table

INSERT INTO category (category_name, category_slug, category_description)
VALUES 
    ('Clothing', 'clothing', 'Discover the latest trends in fashion with our Clothing category. From casual wear to elegant attire, find the perfect outfit for any occasion.'),
    ('Beauty & Personal Care', 'beauty-and-personal-care', 'Enhance your natural beauty and pamper yourself with our Beauty & Personal Care products. Explore a range of skincare, cosmetics, and grooming essentials.'),
    ('Shoes & Accessories', 'shoes-and-accessories', 'Step out in style with our Shoes & Accessories collection. Whether you''re looking for trendy footwear or statement accessories, we''ve got you covered.'),
    ('Toys & Games', 'toys-and-games', 'Spark imagination and endless fun with our Toys & Games selection. From educational toys to exciting games, there''s something for every age and interest.'),
    ('Arts & Crafts', 'arts-and-crafts', 'Unleash your creativity with our Arts & Crafts supplies. Dive into a world of colors, textures, and possibilities to bring your artistic visions to life.'),
    ('Electronics', 'electronics', 'Stay connected and up-to-date with our Electronics category. Discover the latest gadgets, devices, and tech innovations to elevate your digital lifestyle.');

--------------------------------- Create User Table

CREATE TABLE "user"( 
user_id SERIAL PRIMARY KEY,
first_name VARCHAR(50) NOT NULL,
last_name VARCHAR(50) NOT NULL,
email VARCHAR(100) UNIQUE NOT NULL,
mobile VARCHAR(50) UNIQUE ,
password VARCHAR(100) NOT NULL,
created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
is_admin BOOLEAN DEFAULT FALSE,
is_banned BOOLEAN DEFAULT FALSE
);


---------------------------- Insert into User Table

INSERT INTO "user" (first_name, last_name, email, mobile, password, is_admin, is_banned)
VALUES
    ('Raghad', 'Alotaibi', 'Raghad@gmail.com','0539482044','11112',TRUE , FALSE),
    ('Somayah', 'Absi', 'somayah@gmail.com','0556677343','222221',FALSE , FALSE),
    ('Nada', 'Yhaya', 'Nada@gmail.com','0539444478','333311', FALSE , FALSE),
    ('Sadeem', 'Alghamdi','Sadeem@gmail.com','0556678553', '15542', FALSE, FALSE),
    ('Albandri', 'Alotaibi','Albandri@gmail.com','0556677223','11442', FALSE, FALSE);

--------------------------------- Create Product Table





---------------------------- Insert into Product Table
