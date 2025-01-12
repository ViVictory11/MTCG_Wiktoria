namespace MTCG_Wiktoria.Menu;

public class MenuBattle : IMenu
{
    public void DrawMenu()
    {
        Console.WriteLine("~~~ Battle Menu ~~~");
        Console.WriteLine("1. Start a Battle");
        Console.WriteLine("2. Back to Main Menu");

        string userInput = Console.ReadLine();

        switch (userInput)
        {
            case "1":
                /*StartBattle();*/
                break;
            case "2":
                return;
            default:
                Console.WriteLine("Invalid input. Try again.");
                DrawMenu();
                break;
        }
    }
    
}
