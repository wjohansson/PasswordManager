
namespace PasswordManager
{
    public class Users
    {
        public bool ViewAllUsers()
        {
            var users = MenuManager.FileManager.Users;

            Console.Clear();
            Console.WriteLine("All users: ");
            Console.WriteLine();

            foreach (User user in users)
            {
                int userPosition = users.IndexOf(user);

                if (userPosition == 0)
                {
                    continue;
                }

                Console.WriteLine($"[{userPosition}] Username: {user.Username}");
                Console.WriteLine($"    User permission: {user.Permission}");
                Console.WriteLine();
            }

            string position;

            while (true)
            {
                try
                {
                    position = MenuManager.CreateVariable("Choose a user: ", false, true, false);

                    MenuManager.userPosition = Convert.ToInt32(position);

                    var temp = MenuManager.FileManager.Users[MenuManager.userPosition];

                    break;
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("Position does not exist. Try again.");
                    Console.SetCursorPosition(0, Console.CursorTop - 4);
                    continue;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
