
using System.Diagnostics;
using System.Text.Json;

namespace PasswordManager
{
    public abstract class FileManager : IFileManager
    {
        public readonly string _currentDir = Environment.CurrentDirectory;
        public abstract string Path { get; set; }
        public abstract string FileName { get; set; }
        public abstract List<User> Users { get; set; }

        public FileManager()
        {
            Create();
            Users = Get();
        }


        public virtual void Create()
        {
            Path = Directory.GetParent(_currentDir).Parent.Parent.FullName + FileName;

            if (!File.Exists(Path) || String.IsNullOrEmpty(File.ReadAllText(Path)))
            {
                using (FileStream fs = File.Create(Path)) { }

                File.WriteAllText(Path, "[]");
            }
        }

        public List<User> Get()
        {
            string jsonData = File.ReadAllText(Path);
            List<User> lists = JsonSerializer.Deserialize<List<User>>(jsonData);

            return lists;
        }

        public void Update()
        {
            string jsonData = JsonSerializer.Serialize(Users, new JsonSerializerOptions() { WriteIndented = true });

            File.WriteAllText(Path, jsonData);
        }
    }
}
