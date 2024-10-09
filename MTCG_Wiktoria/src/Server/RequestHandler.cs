
using MTCG_Wiktoria.Cards;
using System.Collections.Generic;
using System.Web;
using System;

namespace MTCG_Wiktoria.Server
{
    public class RequestHandler
    {
        private static List<Card> _cards = new List<Card>
        {
            new MonsterCard("Dragon", Element.Fire, 100),
            new SpellCard("Water Blast", Element.Water, 80),
        };

        private static Dictionary<string, string> _users = new Dictionary<string, string>
        {
            { "john_doe", "password123" },
            { "alice_smith", "qwerty" }
        };

        private static Dictionary<string, string> _tokens = new Dictionary<string, string>();

        public static (int, string) GetRequest(string requestUrl, string authorizationHeader)
        {
            if (requestUrl == "/cards")
            {
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

            return (404, "Not Found: The requested resource was not found.");
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

        private static (int, string) HandleSignup(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return (400, "Bad Request: Invalid signup format. Provide 'username' and 'password'.");
            }

            if (_users.ContainsKey(username))
            {
                return (409, "Conflict: User already exists.");
            }

            _users.Add(username, password);
            return (200, "User signed up successfully.");
        }

        private static (int, string) HandleLogin(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return (400, "Bad Request: Invalid login format. Provide 'username' and 'password'.");
            }

            if (_users.TryGetValue(username, out string storedPassword))
            {
                if (storedPassword == password)
                {
                    string token = Guid.NewGuid().ToString();
                    _tokens[username] = token;
                    return (200, $"User logged in successfully. Token: {token}");
                }
                else
                {
                    return (401, "Unauthorized: Invalid password.");
                }
            }

            return (404, "Not Found: User not found.");
        }
    }
}
