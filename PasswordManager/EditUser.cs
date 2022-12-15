
using System.Reflection;

namespace PasswordManager
{
    public class EditUser
    {
        public void Edit(dynamic toEdit, string nameOfToEdit)
        {
            Console.WriteLine();

            List<User> users = MenuManager.FileManager.Users;
            User currentUser = users[MenuManager.userPosition];
            PropertyInfo[] properties = currentUser.GetType().GetProperties();

            User tempUser = new();

            bool isInt = false;
            bool usernameExists = false;
            bool isPassword = false;

            if (toEdit.GetType() == typeof(int))
            {
                isInt = true;
            }

            dynamic old = toEdit;

            try
            {
                Console.Clear();

                bool same;

                if (nameOfToEdit == "Password")
                {
                    isPassword = true;

                    while (true)
                    {
                        if (MenuManager.userLoggedInPosition == MenuManager.userPosition)
                        {
                            string confirmPassword = MenuManager.CreateVariable($"Confirm your old {nameOfToEdit}: ", false, isInt, isPassword);

                            if (confirmPassword != currentUser.Password)
                            {
                                MenuManager.ClearCurrentCursorLine();
                                Console.WriteLine("Wrong password. Try again.");
                                Console.SetCursorPosition(0, 0);
                                continue;
                            }
                        }

                        Console.Clear();

                        break;
                    }

                }

                do
                {
                    same = false;
                    usernameExists = false;
                    usernameExists = false;

                    if (nameOfToEdit != "Password")
                    {
                        Console.WriteLine($"Old {nameOfToEdit}: {old}");
                        Console.WriteLine();
                    }

                    toEdit = MenuManager.CreateVariable($"Enter new {nameOfToEdit}: ", false, isInt, isPassword);

                    
                    if (isInt)
                    {
                        toEdit = Convert.ToInt32(toEdit);
                    }

                    if (toEdit == old)
                    {
                        MenuManager.ClearCurrentCursorLine();
                        Console.WriteLine($"New {nameOfToEdit} can not be the same as old {nameOfToEdit}. Try again.");
                        Console.SetCursorPosition(0, 0);
                        same = true;
                    }
                    else if (nameOfToEdit == "Username")
                        {
                            foreach (User user in users)
                            {
                                if (user.Username == toEdit)
                                {
                                    MenuManager.ClearCurrentCursorLine();
                                    Console.WriteLine("Username already exists. Try again.");
                                    Console.SetCursorPosition(0, 0);
                                    usernameExists = true;
                                    break;
                                }
                            }
                        }

                    foreach (var property in properties)
                    {
                        if (property.Name.ToString() == nameOfToEdit)
                        {
                            property.SetValue(tempUser, toEdit);
                            break;
                        }
                    }

                } while (MenuManager.Validate(tempUser) || same || usernameExists);
            }
            catch (Exception)
            {
                return;
            }

            if (!MenuManager.AreYouSure($"Are you sure you want to edit {nameOfToEdit}? y/N: "))
            {
                return;
            }

            foreach (var property in properties)
            {
                if (property.Name.ToString() == nameOfToEdit)
                {
                    property.SetValue(currentUser, toEdit);
                    break;
                }
            }

            MenuManager.FileManager.Update();

            return;
        }
    }
}
