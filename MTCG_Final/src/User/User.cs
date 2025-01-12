using MTCG_Wiktoria.Cards;

namespace MTCG_Wiktoria.User;

public class User
{
    public User(string username, string password)
    {
        Username = username;
        Password = password;
    }

    public string Username { get; private set; }
    public string Password { get; private set; }
    public string Token { get; set; }

    private List<Card> _stack = new();
    private List<Card> _deck = new();

    public int Coins { get; private set; } = 20;
    public int ELO { get; private set; } = 100;
}