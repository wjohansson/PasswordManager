using System.Diagnostics;

namespace PasswordManager
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Kvar:
            // Tvåfaktorsautentisering
            
            UserManager userManager = new UserManager();

            MenuManager menuManager = new MenuManager();
            menuManager.LoginMenu();

        }
    }
}
