// FileSaver.cs
using System.Text.Json;
using System.IO;
using System.Collections.Generic;

namespace Fitness
{
    public class FileSaver
    {
        private string filePath;

        public FileSaver(string path)
        {
            filePath = path;
        }

        public List<UserInfo> LoadAllUsers()
        {
            if (!File.Exists(filePath))
                return new List<UserInfo>();

            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<UserInfo>>(json) ?? new List<UserInfo>();
        }

        public void SaveUser(UserInfo user)
        {
            var users = LoadAllUsers();
            users.Add(user);
            File.WriteAllText(filePath, JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true }));
        }

        public void UpdateUser(UserInfo updatedUser)
        {
            var users = LoadAllUsers();
            var index = users.FindIndex(u =>
                u.FirstName.Equals(updatedUser.FirstName, StringComparison.OrdinalIgnoreCase) &&
                u.LastName.Equals(updatedUser.LastName, StringComparison.OrdinalIgnoreCase)
            );
            if (index >= 0)
                users[index] = updatedUser;

            File.WriteAllText(filePath, JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true }));
        }

        public void InitializeWithJaneDoe(UserInfo janeDoe)
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(new List<UserInfo> { janeDoe }, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}
