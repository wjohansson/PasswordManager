
using System.ComponentModel.DataAnnotations;
using System.Security;
using System.Security.Cryptography.X509Certificates;

namespace PasswordManager
{
    public class MenuManager
    {
        public static int userPosition;
        public static int userLoggedInPosition;
        private readonly Login _login;
        private readonly CreateUser _createUser;
        private readonly Users _users;
        private readonly User _user;
        private readonly EditUser _editUser;
        public static FileManager FileManager { get; private set; }

        public MenuManager()
        {
            userPosition = -1;
            userLoggedInPosition = -1;
            _login = new Login();
            _createUser = new CreateUser();
            _users = new Users();
            _user = new User();
            _editUser = new EditUser();
            FileManager = new UserManager();

        }

        public void LoginMenu()
        {
            Console.Clear();
            Console.WriteLine("LOGIN MENU");
            Console.WriteLine();

            Console.WriteLine("[L] To login");
            Console.WriteLine("[C] To create new user");
            Console.WriteLine("[Q] To quit");
            Console.WriteLine();

            Console.Write("Input: ");
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.L:
                    if (_login.UserLogin())
                    {
                        UsersMenu();
                        break;
                    }

                    LoginMenu();

                    break;
                case ConsoleKey.C:
                    CreateUserMenu();
                    if (_createUser.Create(false))
                    {
                        UsersMenu();
                        break;
                    }

                    LoginMenu();

                    break;
                case ConsoleKey.Q:
                    QuitProgram();
                    LoginMenu();

                    break;
                default:
                    LoginMenu();

                    break;
            }
        }

        public void CreateUserMenu()
        {
            Console.Clear();
            Console.WriteLine("CREATE USER MENU");
            Console.WriteLine();
            //Byggs ut vid påkoppling av UI?
        }

        public void UsersMenu()
        {
            User currentUserLoggedIn = FileManager.Users[userLoggedInPosition];
            string permission = currentUserLoggedIn.Permission;
            Console.Clear();
            Console.WriteLine("USER OVERVIEW MENU");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine($"Current user logged in: {currentUserLoggedIn.Username}");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.White;

            if (permission == "Admin" || permission == "System")
            {
                Console.WriteLine("[C] To create another user");
                Console.WriteLine("[V] To view another user");
                Console.WriteLine("[E] To edit another user");
            }
            else if (permission == "Moderator")
            {
                Console.WriteLine("[V] To view another user");
            }

            Console.WriteLine();
            Console.WriteLine("[Y] To view your user");

            if (permission != "System")
            {
                Console.WriteLine("[M] To edit your user");
            }

            Console.WriteLine("[X] To log out");
            Console.WriteLine("[Q] To quit the program");
            Console.WriteLine();

            Console.Write("Input: ");
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.C:
                    if (permission != "Admin" && permission != "System")
                    {
                        UsersMenu();
                        break;
                    }

                    CreateUserMenu();
                    _createUser.Create(true);
                    UsersMenu();

                    break;
                case ConsoleKey.V:
                    if (permission == "User")
                    {
                        UsersMenu();
                        break;
                    }

                    if (_users.ViewAllUsers())
                    {
                        UserMenu();
                        break;
                    }

                    UsersMenu();

                    break;
                case ConsoleKey.E:
                    if (permission != "Admin" && permission != "System")
                    {
                        UsersMenu();
                        break;
                    }

                    if (_users.ViewAllUsers())
                    {
                        EditUserMenu();
                        break;
                    }

                    UsersMenu();

                    break;
                case ConsoleKey.Y:
                    userPosition = userLoggedInPosition;

                    UserMenu();
                    break;
                case ConsoleKey.M:
                    if (userPosition == 0)
                    {
                        UsersMenu();
                    }

                    userPosition = userLoggedInPosition;

                    EditUserMenu();
                    break;
                case ConsoleKey.X:
                    if (LogOut())
                    {
                        break;
                    }

                    UsersMenu();

                    break;
                case ConsoleKey.Q:
                    QuitProgram();
                    UsersMenu();

                    break;
                default:
                    UsersMenu();

                    break;
            }
        }

        public void UserMenu()
        {
            User currentUser = FileManager.Users[userPosition];
            User currentUserLoggedIn = FileManager.Users[userLoggedInPosition];
            string permission = currentUserLoggedIn.Permission;

            Console.Clear();
            Console.WriteLine("USER MENU");
            Console.WriteLine();

            if (currentUser == currentUserLoggedIn)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;

                Console.WriteLine("Viewing your user.");
                Console.WriteLine();

                Console.WriteLine($"Username: {currentUserLoggedIn.Username}");
                Console.WriteLine($"    First name: {currentUserLoggedIn.FirstName}");
                Console.WriteLine($"    Last name: {currentUserLoggedIn.LastName}");
                Console.WriteLine($"    Email: {currentUserLoggedIn.Email}");
                Console.WriteLine($"    Age: {currentUserLoggedIn.Age}");
                Console.WriteLine($"    Gender: {currentUserLoggedIn.Gender}");
                Console.WriteLine($"    Adress: {currentUserLoggedIn.Adress}");
                Console.WriteLine($"    Permission: {currentUserLoggedIn.Permission}");
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;

                Console.WriteLine($"Current user logged in: {currentUserLoggedIn.Username}");
                Console.WriteLine($"Your permission: {currentUserLoggedIn.Permission}");
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.White;

                Console.WriteLine($"Currently viewing user: {currentUser.Username}");
                Console.WriteLine($"    First name: {currentUser.FirstName}");
                Console.WriteLine($"    Last name: {currentUser.LastName}");
                Console.WriteLine($"    Email: {currentUser.Email}");
                Console.WriteLine($"    Age: {currentUser.Age}");
                Console.WriteLine($"    Gender: {currentUser.Gender}");
                Console.WriteLine($"    Adress: {currentUser.Adress}");
                Console.WriteLine($"    Permission: {currentUser.Permission}");
                Console.WriteLine();
            }

            if (permission != "User")
            {
                Console.WriteLine("[I] To increase user permission");
                Console.WriteLine("[R] To reduce user permission");
            }
            Console.WriteLine("[D] To delete user");
            Console.WriteLine("[B] To go back");
            Console.WriteLine("[X] To log out");
            Console.WriteLine("[Q] To quit the program");
            Console.WriteLine();

            Console.Write("Input: ");
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.I:
                    if (permission == "User")
                    {
                        UserMenu();
                        break;
                    }

                    if (userPosition == 0)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Can not change System user. Returning");
                        Thread.Sleep(2000);
                        UserMenu();
                    }

                    _user.IncreasePermission();
                    UserMenu();

                    break;
                case ConsoleKey.R:
                    if (permission == "User")
                    {
                        UserMenu();
                        break;
                    }

                    if (userPosition == 0)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Can not change System user. Returning");
                        Thread.Sleep(2000);
                        UserMenu();
                    }

                    _user.ReducePermission();
                    UserMenu();

                    break;

                case ConsoleKey.D:
                    if (userPosition == 0)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Can not change System user. Returning");
                        Thread.Sleep(2000);
                        UserMenu();
                    }

                    if (_user.DeleteUser())
                    {
                        userPosition = -1;

                        if (currentUserLoggedIn == currentUser)
                        {
                            userLoggedInPosition = -1;
                            LoginMenu();
                        }
                        else
                        {
                            UsersMenu();
                        }
                    }

                    UserMenu();

                    break;
                case ConsoleKey.B:
                    UsersMenu();

                    break;
                case ConsoleKey.X:
                    if (LogOut())
                    {
                        break;
                    }

                    UserMenu();
                    break;
                case ConsoleKey.Q:
                    QuitProgram();
                    UserMenu();

                    break;
                default:
                    UserMenu();

                    break;
            }
        }

        public void EditUserMenu()
        {
            User currentUser = FileManager.Users[userPosition];
            User currentUserLoggedIn = FileManager.Users[userLoggedInPosition];

            Console.Clear();
            Console.WriteLine("EDIT USER MENU");
            Console.WriteLine();

            if (currentUser == currentUserLoggedIn)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;

                Console.WriteLine("Editing your user.");
                Console.WriteLine();

                Console.WriteLine($"Username: {currentUserLoggedIn.Username}");
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;

                Console.WriteLine($"Current user logged in: {currentUserLoggedIn.Username}");
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.White;

                Console.WriteLine($"Currently editing user: {currentUser.Username}");
                Console.WriteLine($"{currentUser.Username}'s Permission: {currentUser.Permission}");
                Console.WriteLine();
            }

            Console.WriteLine("[U] To edit username");
            Console.WriteLine("[P] To edit password");
            Console.WriteLine("[F] To edit first name");
            Console.WriteLine("[L] To edit last name");
            Console.WriteLine("[E] To edit email");
            Console.WriteLine("[N] To edit age");
            Console.WriteLine("[G] To edit gender");
            Console.WriteLine("[A] To edit adress");
            Console.WriteLine("[B] To go back");
            Console.WriteLine("[X] To log out");
            Console.WriteLine("[Q] To quit the program");
            Console.WriteLine();

            Console.Write("Input: ");
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.U:
                    _editUser.Edit(currentUser.Username, "Username");

                    EditUserMenu();

                    break;
                case ConsoleKey.P:
                    _editUser.Edit(currentUser.Password, "Password");

                    EditUserMenu();

                    break;
                case ConsoleKey.F:
                    _editUser.Edit(currentUser.FirstName, "FirstName");

                    EditUserMenu();

                    break;
                case ConsoleKey.L:
                    _editUser.Edit(currentUser.LastName, "LastName");

                    EditUserMenu();

                    break;
                case ConsoleKey.E:
                    _editUser.Edit(currentUser.Email, "Email");

                    EditUserMenu();

                    break;
                case ConsoleKey.N:
                    _editUser.Edit(currentUser.Age, "Age");

                    EditUserMenu();

                    break;
                case ConsoleKey.G:
                    _editUser.Edit(currentUser.Gender, "Gender");
                    
                    EditUserMenu();

                    break;
                case ConsoleKey.A:
                    _editUser.Edit(currentUser.Adress, "Adress");
                   
                    EditUserMenu();

                    break;
                case ConsoleKey.B:
                    UsersMenu();

                    break;
                case ConsoleKey.X:
                    if (LogOut())
                    {
                        break;
                    }

                    EditUserMenu();
                    break;
                case ConsoleKey.Q:
                    QuitProgram();
                    EditUserMenu();

                    break;
                default:
                    EditUserMenu();

                    break;
            }
        }


        public static bool Validate(User user)
        {
            ValidationContext context = new ValidationContext(user, null, null);
            List<ValidationResult> validationResults = new List<ValidationResult>();

            bool valid = Validator.TryValidateObject(user, context, validationResults, true);

            if (!valid)
            {
                foreach (ValidationResult validationResult in validationResults)
                {
                    ClearCurrentCursorLine();
                    Console.WriteLine(validationResult.ErrorMessage);
                }

                Console.SetCursorPosition(0, 0);

                return true;
            }

            return false;

        }

        public static void ClearCurrentCursorLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        public static string CreateVariable(string message, bool isPermission, bool isInt, bool isPassword)
        {
            while (isPermission)
            {
                Console.Clear();
                Console.WriteLine("Choose Permission: ");
                Console.WriteLine();
                Console.WriteLine("[0] To go back");
                Console.WriteLine("[1] To choose User");
                Console.WriteLine("[2] To choose Moderator");
                Console.WriteLine("[3] To choose Admin");

                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.D0:
                        throw new Exception();
                    case ConsoleKey.D1:
                        return "User";
                    case ConsoleKey.D2:
                        return "Moderator";
                    case ConsoleKey.D3:
                        return "Admin";
                    default:
                        continue;
                }
            }

            while (isPassword)
            {
                Console.WriteLine("[0] To go back");
                Console.WriteLine();

                ClearCurrentCursorLine();

                Console.Write(message);

                string pass = string.Empty;
                ConsoleKey key;
                do
                {
                    var keyInfo = Console.ReadKey(true);
                    key = keyInfo.Key;
                    if (key == ConsoleKey.D0)
                    {
                        throw new Exception();
                    }
                    else if (key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        Console.Write("\b \b");
                        pass = pass[..^1];
                    }
                    else if (!char.IsControl(keyInfo.KeyChar))
                    {
                        Console.Write("*");
                        pass += keyInfo.KeyChar;
                    }
                } while (key != ConsoleKey.Enter);

                Console.WriteLine();
                return pass;
            }

            while (true)
            {
                Console.WriteLine("[0] To go back");
                Console.WriteLine();

                ClearCurrentCursorLine();

                Console.Write(message);
                string variable = Console.ReadLine();

                if (variable == "0")
                {
                    throw new Exception();
                }

                if (isInt)
                {
                    try
                    {
                        Convert.ToInt32(variable);
                    }
                    catch (FormatException)
                    {
                        ClearCurrentCursorLine();
                        Console.WriteLine("Must be a number. Try again.");
                        Console.SetCursorPosition(0, Console.CursorTop - 4);
                        continue;
                    }
                }

                return variable;
            }

        }

        public static bool AreYouSure(string message)
        {
            ClearCurrentCursorLine();
            Console.Write(message);

            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.Y:
                    return true;
                default:
                    return false;
            }
        }

        public static bool LogOut()
        {
            Console.WriteLine();

            if (AreYouSure("Are you sure you want to log out? y/N: "))
            {
                userLoggedInPosition = -1;
                userPosition = -1;
                MenuManager menuManager = new();
                menuManager.LoginMenu();

                return true;
            }

            return false;
        }

        public static void QuitProgram()
        {
            Console.WriteLine();
            Console.Write("Are you sure you want to quit? Y/n: ");

            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.N:
                    break;
                default:

                    Environment.Exit(-1);
                    break;
            }
        }

    }
}
