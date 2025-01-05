-- Create the database
CREATE DATABASE mydb;

-- Connect to the newly created database
\c mydb

-- Create the users table
CREATE TABLE users (
                       id SERIAL PRIMARY KEY,           -- Unique identifier for each user
                       username TEXT UNIQUE NOT NULL,   -- Username must be unique and not null
                       password TEXT NOT NULL,          -- Password must not be null
                       created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP -- Timestamp for user creation
);

-- Insert example data into the users table (optional)
INSERT INTO users (username, password)
VALUES
    ('admin', 'admin123'),
    ('user1', 'password1'),
    ('user2', 'password2');

-- Query to check the table structure (optional)
\d users;

-- Query to fetch all users (optional)
SELECT * FROM users;
