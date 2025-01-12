using System;
namespace MTCG_Wiktoria.Menu;

public class MenuProfile : IMenu
{
    public void DrawMenu()
    {
        Console.WriteLine("~~~ Profile Menu ~~~");
        Console.WriteLine("1. View Profile");
        Console.WriteLine("2. Edit Profile");
        Console.WriteLine("3. Back to Main Menu");

        string userInput = Console.ReadLine();

        switch (userInput)
        {
            case "1":
                ViewProfile();
                break;
            case "2":
                EditProfile();
                break;
            case "3":
                return;
            default:
                Console.WriteLine("Invalid input. Try again.");
                DrawMenu();
                break;
        }
    }

    private void ViewProfile()
    {
        Console.WriteLine("Viewing profile...");
        // tbc logic
    }

    private void EditProfile()
    {
        Console.WriteLine("Editing profile...");
        // tbc logic
    }
}
