using System;
using MTCG_Wiktoria.Server;
using MTCG_Wiktoria.Menu;

namespace MTCG_Wiktoria
{
    internal class Program
    {
        private static IMenu _currentMenu;

        public static void Main()
        {
            RunTests();

            _currentMenu = new MenuMain();
            new Server.Server().Start();
            _currentMenu.DrawMenu();
        }

        private static void RunTests()
        {
            var tests = new RequestHandlerTests();
            tests.RunAllTests();
        }
    }
}