using System;
using System.Collections.Generic;
using MTCG_Wiktoria.Cards;
using MTCG_Wiktoria.Users;
using System.Web;
using Npgsql;

namespace MTCG_Wiktoria.Server
{
    public static class RequestHandler
    {
        private static List<Card> _cards = new()
        {
            new Dragon("Dragon", Element.FIRE, 100),
            new Goblin("Goblin", Element.NORMAL, 50),
            new Knight("Knight", Element.EARTH, 60),
            new Kraken("Kraken", Element.WATER, 90),
            new FireElf("FireElf", Element.FIRE, 40),
            new MonsterCard("ThunderWolf", Element.LIGHTNING, 70),
            new MonsterCard("EarthGolem", Element.EARTH, 80),
            new MonsterCard("StormPhoenix", Element.LIGHTNING, 95),
            new MonsterCard("SeaSerpent", Element.WATER, 85),
            new SpellCard("Fireball", Element.FIRE, 80),
            new SpellCard("Water Blast", Element.WATER, 60),
            new SpellCard("Earthquake", Element.EARTH, 70),
            new SpellCard("Lightning Strike", Element.LIGHTNING, 100),
            new SpellCard("Inferno", Element.FIRE, 100),
            new SpellCard("Tsunami", Element.WATER, 90),
            new SpellCard("Rockslide", Element.EARTH, 75),
            new SpellCard("Thunderstorm", Element.LIGHTNING, 85),
            new SpellCard("Whirlpool", Element.WATER, 65),
            new SpellCard("Flame Burst", Element.FIRE, 90),
            new MonsterCard("IceDragon", Element.WATER, 110),
            new MonsterCard("LavaBeast", Element.FIRE, 95),
        };

        private static Dictionary<string, User> _users = new()
        {
            { "john_doe", new User("john_doe", "password123") },
            { "alice_smith", new User("alice_smith", "qwerty") },
            { "bob_miller", new User("bob_miller", "securepass") }
        };

        private static Dictionary<string, (string Username, DateTime Expiry)> _activeTokens = new();
        public static (int, string) GetRequest(string requestUrl, string authorizationHeader)
        {
            var uri = new Uri("http://localhost" + requestUrl);
            var query = HttpUtility.ParseQueryString(uri.Query);
            string token = query.Get("token");

            if (requestUrl == "/cards")
            {
                return (200, GetCards());
            }
            else if (requestUrl.StartsWith("/view-cards"))
            {
                if (!IsValidToken(token, out _))
                {
                    return (401, "Unauthorized: Invalid or expired token.");
                }

                return HandleViewAllCards(token);
            }
            else if (requestUrl.StartsWith("/view-deck"))
            {
                if (!IsValidToken(token, out _))
                {
                    return (401, "Unauthorized: Invalid or expired token.");
                }

                return HandleViewDeck(token);
            }

            return (404, "Not Found: The requested resource was not found.");
        }


        public static (int, string) PostRequest(string requestUrl)
        {
            var uri = new Uri("http://localhost" + requestUrl);
            var query = HttpUtility.ParseQueryString(uri.Query);
            string username = query.Get("username");
            string password = query.Get("password");
            string token = query.Get("token");
            string newPassword = query.Get("newPassword");
            string cardNamesQuery = query.Get("cardNames");
            bool cancelDeckConfig = bool.TryParse(query.Get("cancel"), out var cancel) && cancel;

            List<string> cardNames = cardNamesQuery?.Split(',').ToList() ?? new List<string>();

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
                if (!IsValidToken(token, out string tokenUsername) || tokenUsername != username)
                {
                    return (401, "Unauthorized: Invalid or expired token.");
                }

                return HandleDelete(username);
            }
            else if (requestUrl.StartsWith("/update-password"))
            {
                if (!IsValidToken(token, out string tokenUsername) || tokenUsername != username)
                {
                    return (401, "Unauthorized: Invalid or expired token.");
                }

                return HandleUpdatePassword(username, newPassword);
            }
            else if (requestUrl.StartsWith("/logout"))
            {
                if (!IsValidToken(token, out _))
                {
                    return (401, "Unauthorized: Invalid or expired token.");
                }

                return HandleLogout(token);
            }
            else if (requestUrl.StartsWith("/acquire-package"))
            {
                if (!IsValidToken(token, out _))
                {
                    return (401, "Unauthorized: Invalid or expired token.");
                }

                return HandleAcquirePackage(token);
            }
            else if (requestUrl.StartsWith("/configure-deck"))
            {
                if (!IsValidToken(token, out _))
                {
                    return (401, "Unauthorized: Invalid or expired token.");
                }

                return HandleConfigureDeck(token, cardNames, cancelDeckConfig);
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
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return (400, "Invalid signup: Provide 'username' and 'password'.");

            if (_users.ContainsKey(username))
                return (409, "User already exists.");

            _users[username] = new User(username, password);
            _users[username]._stack = new List<Card>();
            _users[username]._deck = new List<Card>();

            return (200, "User registered successfully.");
        }
        

        private static (int, string) HandleLogin(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return (400, "Bad Request: Invalid login format. Provide 'username' and 'password'.");
            }

            if (_users.TryGetValue(username, out var userData))
            {
                if (userData.Password == password)
                {
                    string token = Guid.NewGuid().ToString();
                    _activeTokens[token] = (username, DateTime.UtcNow.AddHours(1)); // Token valid for 1 hour
                    return (200, $"User logged in successfully. Token: {token}");
                }
                else
                {
                    return (401, "Unauthorized: Invalid password.");
                }
            }

            return (404, "Not Found: User not found.");
        }
        

        private static (int, string) HandleDelete(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return (400, "Bad Request: Invalid delete format. Provide 'username'.");
            }

            if (_users.ContainsKey(username))
            {
                _users.Remove(username); // Remove user from dictionary
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

            if (_users.TryGetValue(username, out var userData))
            {
                // Create a new User instance with the updated password but copy over existing data
                var updatedUser = new User(username, newPassword)
                {
                    _stack = new List<Card>(userData._stack), // Copy the stack
                    _deck = new List<Card>(userData._deck)   // Copy the deck
                };

                // Replace the old user in the dictionary
                _users[username] = updatedUser;

                return (200, "Password updated successfully.");
            }

            return (404, "Not Found: User does not exist.");
        }



        private static bool IsValidToken(string token, out string username)
        {
            username = null;

            if (_activeTokens.TryGetValue(token, out var tokenData))
            {
                if (DateTime.UtcNow <= tokenData.Expiry)
                {
                    username = tokenData.Username;
                    return true;
                }
                else
                {
                    _activeTokens.Remove(token); // Remove expired tokens
                }
            }

            return false;
        }



        private static (int, string) HandleLogout(string token)
        {
            if (string.IsNullOrEmpty(token) || !_activeTokens.ContainsKey(token))
            {
                return (401, "Unauthorized: Invalid or missing token.");
            }

            _activeTokens.Remove(token);
            return (200, "User logged out successfully.");
        }

        private static List<Card> GenerateRandomPackage()
        {
            Random rand = new Random();
            return _cards.OrderBy(x => rand.Next()).Take(5).ToList();
        }

        private static (int, string) HandleAcquirePackage(string token)
        {
            if (!IsValidToken(token, out string username))
                return (401, "Unauthorized: Invalid token.");

            var userData = _users[username];
            if (userData.Coins < 5)
                return (400, "Insufficient coins.");

            var package = GenerateRandomPackage();
            userData._stack.AddRange(package);
            userData.DeductCoins(5);

            _users[username] = userData;
            return (200, $"Package acquired: {string.Join(", ", package.Select(c => c.Name))}");
        }

        private static (int, string) HandleViewDeck(string token)
        {
            if (!IsValidToken(token, out string username))
                return (401, "Unauthorized: Invalid token.");

            var deck = _users[username]._deck;

            if (!deck.Any())
                return (200, "Your deck is empty.");

            var deckDetails = string.Join("\n",
                deck.Select(card => $"{card.Name} - Element: {card.Element}, Damage: {card.Damage}"));
            return (200, $"Your deck:\n{deckDetails}");
        }

        private static (int, string) HandleViewAllCards(string token)
        {
            if (!IsValidToken(token, out string username))
                return (401, "Unauthorized: Invalid token.");

            var stack = _users[username]._stack;

            if (!stack.Any())
                return (200, "Your stack is empty.");

            var stackDetails = string.Join("\n",
                stack.Select(card => $"{card.Name} - Element: {card.Element}, Damage: {card.Damage}"));
            return (200, $"Your cards:\n{stackDetails}");
        }


        private static (int, string) HandleConfigureDeck(string token, List<string> cardNames, bool cancel = false)
        {
            if (!IsValidToken(token, out string username))
                return (401, "Unauthorized: Invalid token.");

            var userData = _users[username];
            
            if (cancel)
            {
                userData._deck.Clear();
                _users[username] = userData;
                return (200, "Deck configuration canceled. Deck is now empty.");
            }
            
            if (userData._stack.Count < 4)
                return (400, "Not enough cards in stack to configure a deck. Acquire more cards first.");
            
            if (cardNames.Count != 4)
                return (400, "Invalid deck configuration. You must select exactly 4 cards.");
            
            var selectedCards = new List<Card>();
            var usedCardIndices = new HashSet<int>();

            foreach (var name in cardNames)
            {
                var cardIndex = userData._stack.FindIndex(card =>
                    card.Name == name && !usedCardIndices.Contains(userData._stack.IndexOf(card)));
                if (cardIndex == -1)
                    return (400, $"Card '{name}' is not available in your stack or has already been selected.");

                selectedCards.Add(userData._stack[cardIndex]);
                usedCardIndices.Add(cardIndex);
            }
            
            userData._deck = selectedCards;
            _users[username] = userData;

            return (200, $"Deck configured successfully: {string.Join(", ", userData._deck.Select(card => card.Name))}");
        }
    }
}