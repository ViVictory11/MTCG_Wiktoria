namespace MTCG_Wiktoria.Menu;

public class MenuMain : IMenu
{
    public void DrawMenu()
    {
        Console.WriteLine("~~~ Dungeon Fire ~~~/n");
        Console.WriteLine("/n~~~ Main Menu ~~~ /n");
        Console.WriteLine("/n You want to Sign up (0) or Log in (1)?/n");

        string userInput = Console.ReadLine();

        while (true)
        {
            if (userInput == "0")
            {
                SignUp();
                //break;
            }
            else if (userInput == "1")
            {
                //break;
            }
            else
            {
                Console.WriteLine("Wrong input!");
            }
        }
    }

    private void LogIn()
    {
    }

    private void SignUp()
    {
        Console.WriteLine("~~~ Sign In ~~~");
        Console.Write("Enter username: ");
        string username = Console.ReadLine();

        Console.Write("Enter password: ");
        string password = Console.ReadLine();
        
        Console.Write("Enter password again: ");
        string passwordAgain = Console.ReadLine();

        if (passwordAgain == password && username != "" && password != "" && passwordAgain != "")
        {
            new User.User(username, password);
        }

        //new User implementation
        

    }
}