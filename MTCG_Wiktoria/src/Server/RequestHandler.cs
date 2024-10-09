using MTCG_Wiktoria.Cards;
using System.Net;
using System.Collections.Generic;
using System.Web;
using System;

namespace MTCG_Wiktoria.Server
{
    public class RequestHandler
    {
        // Simulated in-memory storage for cards
        private static List<Card> _cards = new List<Card>
        {
            new MonsterCard("Dragon", Element.Fire, 100),
            new SpellCard("Water Blast", Element.Water, 80),
        };

        // Static user data (username, password)
        private static Dictionary<string, string> _users = new Dictionary<string, string>
        {
            { "john_doe", "password123" },
            { "alice_smith", "qwerty" }
        };

        // Store tokens associated with usernames
        private static Dictionary<string, string> _tokens = new Dictionary<string, string>();

        public static string GetRequest(string requestUrl, string authorizationHeader)
        {
            if (requestUrl == "/cards")
            {

                return GetCards();

                //return GenerateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized", "Invalid or missing token.");

            }

            return GenerateErrorResponse(HttpStatusCode.NotFound, "Not Found", "The requested resource was not found.");
        }

        public static string PostRequest(string requestUrl)
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

            return GenerateErrorResponse(HttpStatusCode.NotFound, "Not Found", "The requested resource was not found.");
        }

        public static string GenerateErrorResponse(HttpStatusCode statusCode, string statusMessage, string errorMessage)
        {
            // Return an error message formatted with the status code and description
            return $"{(int)statusCode} {statusMessage}: {errorMessage}";
        }

        private static string GetCards()
        {
            var cardDescriptions = new List<string>();
            foreach (var card in _cards)
            {
                cardDescriptions.Add($"{card.Name} - Element: {card.Element}, Damage: {card.Damage}");
            }

            return string.Join("\n", cardDescriptions);
        }

        private static string HandleSignup(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return GenerateErrorResponse(HttpStatusCode.BadRequest, "Bad Request",
                    "Invalid signup format. Provide 'username' and 'password' query parameters.");
            }

            if (_users.ContainsKey(username))
            {
                return GenerateErrorResponse(HttpStatusCode.Conflict, "Conflict", "User already exists.");
            }

            _users.Add(username, password);
            return "User signed up successfully.";
        }

        private static string HandleLogin(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return GenerateErrorResponse(HttpStatusCode.BadRequest, "Bad Request",
                    "Invalid login format. Provide 'username' and 'password' query parameters.");
            }

            // Check if user exists and password matches
            if (_users.TryGetValue(username, out string storedPassword))
            {
                if (storedPassword == password)
                {
                    // Generate a token and store it
                    string token = Guid.NewGuid().ToString();
                    _tokens[username] = token; // Store token associated with the user
                    return $"User logged in successfully. Token: {token}";
                }
                else
                {
                    return GenerateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized", "Invalid password.");
                }
            }

            return GenerateErrorResponse(HttpStatusCode.NotFound, "Not Found", "User not found.");
        }

        // Authorization method to validate the token
        /*private static bool Authorize(string authorizationHeader)
        {
            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                string token = authorizationHeader.Substring("Bearer ".Length).Trim();

                // Check if the token exists in our dictionary
                return _tokens.ContainsValue(token);
            }

            return false; // Token not provided or invalid
        }*/
    }
}
