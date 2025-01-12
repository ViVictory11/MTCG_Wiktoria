using System;

namespace MTCG_Wiktoria.Menu;

public class MenuTrade : IMenu
{
        public void DrawMenu()
        {
            Console.WriteLine("~~~ Trading Menu ~~~");
            Console.WriteLine("1. Create a Trade");
            Console.WriteLine("2. View Open Trades");
            Console.WriteLine("3. Back to Main Menu");

            string userInput = Console.ReadLine();

            switch (userInput)
            {
                case "1":
                    CreateTrade();
                    break;
                case "2":
                    ViewTrades();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid input. Try again.");
                    DrawMenu();
                    break;
            }
        }

        private void CreateTrade()
        {
            Console.WriteLine("Creating a trade...");
            // tbc logic
        }

        private void ViewTrades()
        {
            Console.WriteLine("Viewing open trades...");
        }
    }