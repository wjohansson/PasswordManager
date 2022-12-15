
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Security;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace PasswordManager
{
    public class CreateUser
    {
        public bool Create(bool fromAdmin)
        {
            List<User> users = MenuManager.FileManager.Users;
            PropertyInfo[] properties = users[0].GetType().GetProperties();

            Console.Clear();

            User newUser = new User();

            try
            {

                foreach (var property in properties)
                {
                    var isPermission = false;
                    var isInt = false;
                    var isPassword = false;
                    var usernameExists = false;
                    string randomPassword = "";

                    if (property.Name == "Permission")
                    {
                        isPermission = true;
                    }

                    if (property.PropertyType == typeof(int))
                    {
                        isInt = true;
                    }

                    if (property.Name == "Password")
                    {
                        isPassword = true;

                        randomPassword = RandomPasswordGenerator();

                        Console.Clear();

                        if (randomPassword != "")
                        {
                            newUser.Password = randomPassword;
                            break;
                        }
                    }

                    do
                    {
                        if (!fromAdmin && property.Name == "Permission")
                        {
                            newUser.Permission = "User";
                            break;
                        }

                        dynamic info = MenuManager.CreateVariable($"Enter new {property.Name}: ", isPermission, isInt, isPassword);

                        usernameExists = false;

                        if (property.Name == "Username")
                        {
                            foreach (User user in users)
                            {
                                if (user.Username == info)
                                {
                                    MenuManager.ClearCurrentCursorLine();
                                    Console.WriteLine("Username already exists. Try again.");
                                    Console.SetCursorPosition(0, Console.CursorTop - 4);
                                    usernameExists = true;
                                    break;
                                }
                            }
                        }

                        if (isInt)
                        {
                            info = Convert.ToInt32(info);
                        }

                        property.SetValue(newUser, info);
                    } while (MenuManager.Validate(newUser) || usernameExists);

                    Console.Clear();
                }
            }
            catch (Exception)
            {
                return false;
            }

            users.Add(newUser);

            if (!fromAdmin)
            {
                MenuManager.userLoggedInPosition = MenuManager.FileManager.Users.IndexOf(newUser);
                MenuManager.userPosition = MenuManager.userLoggedInPosition;
            }

            MenuManager.FileManager.Update();

            return true;
        }

        public string RandomPasswordGenerator()
        {
            Console.Clear();
            Console.WriteLine();

            Random random = new Random();

            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!#%&?";
            string randomPassword = "";

            for (int i = 0; i < 10; i++)
            {
                int randomNumber = random.Next(chars.Length);
                randomPassword += chars[randomNumber];
            }

            var regex = new Regex("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[^\\da-zA-Z])(.{10,})$");

            if (!regex.IsMatch(randomPassword))
            {
                return RandomPasswordGenerator();
            }

            Console.WriteLine($"Random generated password: {randomPassword}");
            Console.WriteLine();
            Console.WriteLine("[0] to go back");
            Console.WriteLine("[1] to choose random password");
            Console.WriteLine("[2] to generate new random password");
            Console.WriteLine("[3] to create new password");
            Console.WriteLine();
            Console.Write("Choose: ");

            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D0:

                    throw new Exception();
                case ConsoleKey.D1:
                    return randomPassword;
                case ConsoleKey.D2:
                    return RandomPasswordGenerator();
                default:
                    return "";

            }
        }
    }
}
