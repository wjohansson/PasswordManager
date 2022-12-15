
namespace PasswordManager
{
    public class UserManager : FileManager
    {
        public override string Path { get; set; }
        public override string FileName { get; set; }
        public override List<User> Users { get; set; }

        public override void Create()
        {
            FileName = @"\Users.json";

            Path = Directory.GetParent(_currentDir).Parent.Parent.FullName + FileName;

            if (!File.Exists(Path) || String.IsNullOrEmpty(File.ReadAllText(Path)) || File.ReadAllText(Path) == "[]")
            {
                File.WriteAllText(Path, @"[{""Username"":""S"",""Password"":""S"",""FirstName"":""System"",""LastName"":""Manager"",""Email"":""admin@mail.com"",""Age"":9000,""Gender"":""Robot"",""Adress"":""113.56.121.167"",""Permission"":""System""}]");
            }
        }
    }
}
