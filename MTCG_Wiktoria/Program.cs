using MTCG_Wiktoria.Menu;

namespace MTCG_Wiktoria;

internal class Program
{
    private static IMenu _currentMenu;
    public static void Main(string[] args)
    {
        _currentMenu = new MenuMain();
        
        new Server.Server().Start();
        _currentMenu.DrawMenu();
    }
}