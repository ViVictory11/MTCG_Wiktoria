using System;
using System.Data;
using Npgsql;

namespace MTCG_Wiktoria.Server;

public static class DatabaseHanlder
{
     private const string ConnectionString = "Host=localhost;Port=5432;Database=mydb;Username=admin;Password=admin";

        public static void Initialize()
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS users (
                    id SERIAL PRIMARY KEY,
                    username TEXT UNIQUE NOT NULL,
                    password TEXT NOT NULL
                );
            ";
            command.ExecuteNonQuery();

            Console.WriteLine("Database initialized successfully.");
        }

        public static bool AddUser(string username, string password)
        {
            try
            {
                using var connection = new NpgsqlConnection(ConnectionString);
                connection.Open();

                using var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO users (username, password) VALUES (@username, @password)";
                command.Parameters.AddWithValue("username", username);
                command.Parameters.AddWithValue("password", password);
                command.ExecuteNonQuery();

                return true;
            }
            catch (PostgresException ex) when (ex.SqlState == "23505") // Unique violation
            {
                return false;
            }
        }

        public static bool VerifyUser(string username, string password)
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM users WHERE username = @username AND password = @password";
            command.Parameters.AddWithValue("username", username);
            command.Parameters.AddWithValue("password", password);

            return (long)command.ExecuteScalar() > 0;
        }
}
