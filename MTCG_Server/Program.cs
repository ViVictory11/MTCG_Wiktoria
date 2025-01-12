using System;
using System.Threading.Tasks;
using MTCG_Wiktoria.Server;
using MTCG_Wiktoria.Menu;

namespace MTCG_Wiktoria
{
    internal class Program
    {
        private static IMenu _currentMenu;

        public static async Task Main()
        {
            _currentMenu = new MenuMain();

            Task serverTask = Task.Run(() =>
            {
                new Server.Server().Start();
            });

            await RunMenuAsync();

            await serverTask;
        }

        private static Task RunMenuAsync()
        {
            return Task.Run(() =>
            {
                _currentMenu.DrawMenu();
            });
        }
    }
}
