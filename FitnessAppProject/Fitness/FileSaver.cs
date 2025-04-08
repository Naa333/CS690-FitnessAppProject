namespace Fitness;

using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Spectre.Console; 

public class FileSaver
{
    private string fileName;

    public FileSaver(string fileName)
    {
        this.fileName = fileName;
        if (!File.Exists(this.fileName))
        {
            File.Create(this.fileName).Close();
        }
    }

    public void SaveUser(UserInfo user)
    {
        List<UserInfo> allUsers = LoadAllUsers();
        allUsers.Add(user);
        SaveAllUsers(allUsers);
    }

    public List<UserInfo> LoadAllUsers()
    {
        if (File.Exists(fileName))
        {
            try
            {
                string jsonString = File.ReadAllText(fileName);
                var users = JsonSerializer.Deserialize<List<UserInfo>>(jsonString);
                return users ?? new List<UserInfo>();
            }
            catch (JsonException ex)
            {
                AnsiConsole.WriteLine($"[red]Error reading user data file (loading all users from FileSaver): {ex.Message}[/]");
                return new List<UserInfo>();
            }
        }
        return new List<UserInfo>();
    }

    private void SaveAllUsers(List<UserInfo> users)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(users, options);
        File.WriteAllText(fileName, jsonString);
    }

    public void InitializeWithJaneDoe(UserInfo janeDoe)
    {
        SaveAllUsers(new List<UserInfo> { janeDoe });
        AnsiConsole.WriteLine("[green]Initialized with Jane Doe (JSON array format via FileSaver).[/]");
    }
}