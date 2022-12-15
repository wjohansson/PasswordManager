
using System.ComponentModel.DataAnnotations;

namespace PasswordManager
{
    public class User
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Username must be between 6 and 20 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[^\\da-zA-Z])(.{10,})$", ErrorMessage = "Password must be atleast 10 characters, one uppercase, one lowercase, one number, and one special")]
        public string Password { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Age must be 1 or more")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Adress is required")]
        public string Adress { get; set; }

        [Required(ErrorMessage = "Permission is requierd")]
        public string Permission { get; set; }


        public User() 
        {
            Username = "TempUsername";
            Password = "TempPassword1!";
            FirstName = "TempFirst";
            LastName = "TempLast";
            Email = "temp@mail.com";
            Age = 1;
            Gender = "TempGender";
            Adress = "TempAdress";
            Permission = "TempPermission";
        }

        public void IncreasePermission()
        {
            Console.WriteLine();

            if (!MenuManager.AreYouSure("Are you sure you want to increase this users permission? y/N: "))
            {
                return;
            }

            User currentUser = MenuManager.FileManager.Users[MenuManager.userPosition];
            User currentUserLoggedIn = MenuManager.FileManager.Users[MenuManager.userLoggedInPosition];
            
            Console.WriteLine();

            if (currentUser.Permission == "Admin") 
            {
                Console.WriteLine("Users permission is already admin, can not increase. Returning");
                Thread.Sleep(2000);
                return;
            }
            else if (currentUser.Permission == "Moderator" && currentUserLoggedIn.Permission == "Moderator")
            {
                Console.WriteLine("Logged in user can not promote to admin. Returning");
                Thread.Sleep(2000);
                return;
            }
            else if (currentUser.Permission == "Moderator" && currentUserLoggedIn.Permission == "Admin")
            {
                currentUser.Permission = "Admin";
            }
            else if (currentUser.Permission == "User")
            {
                currentUser.Permission = "Moderator";
            }

            MenuManager.FileManager.Update();
        }

        public void ReducePermission()
        {
            Console.WriteLine();

            if (!MenuManager.AreYouSure("Are you sure you want to reduce this users permission? y/N: "))
            {
                return;
            }

            Console.WriteLine();

            User currentUser = MenuManager.FileManager.Users[MenuManager.userPosition];
            User currentUserLoggedIn = MenuManager.FileManager.Users[MenuManager.userLoggedInPosition];

            if (currentUser.Permission == "User")
            {
                Console.WriteLine("Users permission is already user, can not reduce. Returning");
                Thread.Sleep(2000);
                return;
            }
            else if ((currentUser.Permission == "Admin" || currentUser.Permission == "Moderator") && currentUserLoggedIn.Permission == "Moderator")
            {
                Console.WriteLine("Logged in user can not demote this user. Returning");
                Thread.Sleep(2000);
                return;
            }
            else if (currentUser.Permission == "Admin" && currentUserLoggedIn.Permission == "Admin")
            {
                Console.WriteLine("Logged in user can not demote this user. Returning");
                Thread.Sleep(2000);
                return;
            }
            else if (currentUser.Permission == "Admin" && currentUserLoggedIn.Permission == "System")
            {
                currentUser.Permission = "Moderator";
            }
            else if (currentUser.Permission == "Moderator")
            {
                currentUser.Permission = "User";
            }

            MenuManager.FileManager.Update();
        }

        public bool DeleteUser()
        {
            Console.WriteLine();

            if (!MenuManager.AreYouSure("Are you sure you want to delete this user? y/N: "))
            {
                return false;
            }

            List<User> users = MenuManager.FileManager.Users;

            users.RemoveAt(MenuManager.userPosition);

            MenuManager.FileManager.Update();

            return true;
        }
    }
}
