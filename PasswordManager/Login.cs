
using System.Threading.Tasks.Sources;

namespace PasswordManager
{
    public class Login
    {
        public bool UserLogin()
        {
            List<User> users = MenuManager.FileManager.Users;
            
            Console.Clear();

            string username;
            string password;

            while (true)
            {
                try
                {
                    username = MenuManager.CreateVariable("Enter your username: ", false, false, false);

                    foreach (User user in users)
                    {
                        if (user.Username == username)
                        {
                            MenuManager.userPosition = users.IndexOf(user);
                            break;
                        }
                    }

                    Console.Clear();

                    password = MenuManager.CreateVariable("Enter your password: ", false, false, true);

                    if (MenuManager.userPosition == -1 || users[MenuManager.userPosition].Password != password)
                    {
                        MenuManager.userPosition = -1;
                        Console.WriteLine();
                        Console.WriteLine("Incorrect username or password. Try again.");
                        Console.SetCursorPosition(0, 0);

                        continue;
                    }

                    break;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            MenuManager.userLoggedInPosition = MenuManager.userPosition;

            return true;
        }
    }
}
