using System;
using System.Collections.Generic;
using System.Web;
using Npgsql;

namespace MTCG_Wiktoria.Server
{
    public static class RequestHandler
    {
        private static Dictionary<string, string> _activeTokens = new Dictionary<string, string>(); // Token -> Username

        public static (int, string) GetRequest(string requestUrl, string authorizationHeader)
        {
            if (requestUrl == "/cards")
            {
                if (!IsValidToken(authorizationHeader))
                {
                    return (401, "Unauthorized: Invalid or missing token.");
                }
                return (200, GetCards());
            }
            return (404, "Not Found: The requested resource was not found.");
        }

        public static (int, string) PostRequest(string requestUrl)
        {
            var uri = new Uri("http://localhost" + requestUrl);
            var query = HttpUtility.ParseQueryString(uri.Query);
            string username = query.Get("username");
            string password = query.Get("password");

            if (requestUrl.StartsWith("/signup"))
            {
                return HandleSignup(username, password);
            }
            else if (requestUrl.StartsWith("/login"))
            {
                return HandleLogin(username, password);
            }
            else if (requestUrl.StartsWith("/delete"))
            {
                if (!IsValidToken(query.Get("token")))
                {
                    return (401, "Unauthorized: Invalid or missing token.");
                }
                return HandleDelete(username);
            }
            else if (requestUrl.StartsWith("/update-password"))
            {
                string newPassword = query.Get("newPassword");
                if (!IsValidToken(query.Get("token")))
                {
                    return (401, "Unauthorized: Invalid or missing token.");
                }
                return HandleUpdatePassword(username, newPassword);
            }
            return (404, "Not Found: The requested resource was not found.");
        }

        private static string GetCards()
        {
            var cardDescriptions = new List<string>();
            try
            {
                using var connection = new NpgsqlConnection(UserHandler.ConnectionString);
                connection.Open();
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT name, type, element, damage FROM cards";
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string name = reader.GetString(0);
                    string type = reader.GetString(1);
                    string element = reader.GetString(2);
                    int damage = reader.GetInt32(3);
                    cardDescriptions.Add($"{name} - Type: {type}, Element: {element}, Damage: {damage}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error fetching cards: {ex.Message}");
                return "Error fetching cards";
            }
            return string.Join("\n", cardDescriptions);
        }

        private static (int, string) HandleSignup(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return (400, "Bad Request: Invalid signup format. Provide 'username' and 'password'.");
            }

            bool success = UserHandler.AddUser(username, password);
            if (success)
            {
                return (200, "User signed up successfully.");
            }
            else
            {
                return (409, "Conflict: User already exists.");
            }
        }

        private static (int, string) HandleLogin(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return (400, "Bad Request: Invalid login format. Provide 'username' and 'password'.");
            }

            bool isVerified = UserHandler.VerifyUser(username, password);
            if (isVerified)
            {
                string token = Guid.NewGuid().ToString();
                _activeTokens[token] = username; // Store the token and username mapping
                return (200, $"User logged in successfully. Token: {token}");
            }
            else
            {
                return (401, "Unauthorized: Invalid username or password.");
            }
        }

        private static (int, string) HandleDelete(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return (400, "Bad Request: Invalid delete format. Provide 'username'.");
            }

            bool success = UserHandler.DeleteUser(username);
            if (success)
            {
                return (200, "User deleted successfully.");
            }
            else
            {
                return (404, "Not Found: User does not exist.");
            }
        }

        private static (int, string) HandleUpdatePassword(string username, string newPassword)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(newPassword))
            {
                return (400, "Bad Request: Invalid update format. Provide 'username' and 'newPassword'.");
            }

            bool success = UserHandler.UpdatePassword(username, newPassword);
            if (success)
            {
                return (200, "Password updated successfully.");
            }
            else
            {
                return (404, "Not Found: User does not exist.");
            }
        }

        private static bool IsValidToken(string token)
        {
            return !string.IsNullOrEmpty(token) && _activeTokens.ContainsKey(token);
        }
    }
}
