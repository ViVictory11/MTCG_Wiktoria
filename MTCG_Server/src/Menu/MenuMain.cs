namespace MTCG_Wiktoria.Menu
{
    public class MenuMain : IMenu
    {
        public void DrawMenu()
        {
            Console.WriteLine("~~~ Dungeon Fire ~~~\n");
            Console.WriteLine("\n~~~ Main Menu ~~~ \n");
            Console.WriteLine("\nYou want to Sign up (0) or Log in (1)?\n");

            string userInput = Console.ReadLine();

            while (true)
            {
                if (userInput == "0")
                {
                    SignUp();
                    break;
                }
                else if (userInput == "1")
                {
                    LogIn();
                    break;
                }
                else
                {
                    Console.WriteLine("Wrong input! Please enter 0 for Sign up or 1 for Log in.");
                    userInput = Console.ReadLine();
                }
            }
        }

        private void LogIn()
        {
            Console.WriteLine("~~~ Log In ~~~");
            Console.Write("Enter username: ");
            string username = Console.ReadLine();

            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            string requestBody = $"{username},{password}";
            //string response = MTCG_Wiktoria.Server.RequestHandler.PostRequest("/login", requestBody);

            //Console.WriteLine(response);
        }

        private void SignUp()
        {
            Console.WriteLine("~~~ Sign Up ~~~");
            Console.Write("Enter username: ");
            string username = Console.ReadLine();

            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            Console.Write("Enter password again: ");
            string passwordAgain = Console.ReadLine();

            if (password == passwordAgain && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                string requestBody = $"{username},{password}";
                //string response = MTCG_Wiktoria.Server.RequestHandler.PostRequest("/signup", requestBody);

                //Console.WriteLine(response);
            }
            else
            {
                Console.WriteLine("Passwords do not match or inputs are invalid. Please try again.");
                SignUp();
            }
        }
    }
}
