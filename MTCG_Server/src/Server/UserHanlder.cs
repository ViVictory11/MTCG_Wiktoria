using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using Npgsql;

namespace MTCG_Wiktoria.Server
{
    public static class UserHandler
    {
        public static readonly string ConnectionString =
            "Host=localhost;Username=admin;Password=admin;Database=postgres;";

        // Add a new user
        public static bool AddUser(string username, string password)
        {
            try
            {
                var salt = GenerateSalt();
                var hashedPassword = HashPassword(password, salt);

                using var connection = new NpgsqlConnection(ConnectionString);
                connection.Open();

                using var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO users (username, password) VALUES (@username, @password)";
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", $"{Convert.ToBase64String(salt)}:{hashedPassword}");
                command.ExecuteNonQuery();

                return true;
            }
            catch (PostgresException ex) when (ex.SqlState == "23505")
            {
                return false; // User already exists
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                throw;
            }
        }

        // Verify user credentials
        public static bool VerifyUser(string username, string password)
        {
            try
            {
                using var connection = new NpgsqlConnection(ConnectionString);
                connection.Open();

                using var command = connection.CreateCommand();
                command.CommandText = "SELECT password FROM users WHERE username = @username";
                command.Parameters.AddWithValue("username", username);

                var storedPassword = command.ExecuteScalar() as string;
                if (storedPassword == null) return false;

                var parts = storedPassword.Split(':');
                if (parts.Length != 2) return false;

                var salt = Convert.FromBase64String(parts[0]);
                var storedHash = parts[1];
                var computedHash = HashPassword(password, salt);

                return storedHash == computedHash;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                throw;
            }
        }

        // Delete a user
        public static bool DeleteUser(string username)
        {
            try
            {
                using var connection = new NpgsqlConnection(ConnectionString);
                connection.Open();

                using var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM users WHERE username = @username";
                command.Parameters.AddWithValue("username", username);

                return command.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                throw;
            }
        }

        // Update user password
        public static bool UpdatePassword(string username, string newPassword)
        {
            try
            {
                var salt = GenerateSalt();
                var hashedPassword = HashPassword(newPassword, salt);

                using var connection = new NpgsqlConnection(ConnectionString);
                connection.Open();

                using var command = connection.CreateCommand();
                command.CommandText = "UPDATE users SET password = @password WHERE username = @username";
                command.Parameters.AddWithValue("username", username);
                command.Parameters.AddWithValue("password", $"{Convert.ToBase64String(salt)}:{hashedPassword}");

                return command.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                throw;
            }
        }

        // Helper functions
        private static byte[] GenerateSalt()
        {
            using var rng = new RNGCryptoServiceProvider();
            var salt = new byte[16];
            rng.GetBytes(salt);
            return salt;
        }

        private static string HashPassword(string password, byte[] salt)
        {
            using var sha256 = SHA256.Create();
            var saltedPassword = Encoding.UTF8.GetBytes(password);
            var saltedPasswordWithSalt = new byte[saltedPassword.Length + salt.Length];

            Buffer.BlockCopy(salt, 0, saltedPasswordWithSalt, 0, salt.Length);
            Buffer.BlockCopy(saltedPassword, 0, saltedPasswordWithSalt, salt.Length, saltedPassword.Length);

            var hash = sha256.ComputeHash(saltedPasswordWithSalt);
            return Convert.ToBase64String(hash);
        }
    }
}
