using MTCG_Wiktoria.Cards;

namespace MTCG_Wiktoria.Users;

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

    public List<Card> _stack = new();
    public List<Card> _deck = new();

    public int Coins { get; private set; } = 20;
    public int ELO { get; private set; } = 100;
    
    public void AddCoins(int amount)
    {
        Coins += amount;
    }

    public void DeductCoins(int amount)
    {
        if (Coins >= amount)
            Coins -= amount;
    }

}

