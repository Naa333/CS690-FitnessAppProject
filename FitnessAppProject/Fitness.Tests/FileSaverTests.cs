namespace Fitness.Tests;

using Xunit;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using Fitness;

public class FileSaverTests
{
    private readonly string _testFilePath = "test_users.json";

    // Helper method to clean up the test file after each test
    private void CleanupTestFile()
    {
        if (File.Exists(_testFilePath))
        {
            File.Delete(_testFilePath);
        }
    }

    [Fact]
    public void LoadAllUsers_NoFile() //File does not exist
    {
        CleanupTestFile(); 

        var fileSaver = new FileSaver(_testFilePath);
        var users = fileSaver.LoadAllUsers();

        Assert.NotNull(users);
        Assert.Empty(users);
    }

    [Fact]
    public void LoadAllUsers_FileExists()
    {
        CleanupTestFile();

        var initialUsers = new List<UserInfo>
        {
            new UserInfo { FirstName = "John", LastName = "Doe", Age = 30 },
            new UserInfo { FirstName = "Jane", LastName = "Smith", Age = 25 }
        };
        File.WriteAllText(_testFilePath, JsonSerializer.Serialize(initialUsers));

        var fileSaver = new FileSaver(_testFilePath);
        var users = fileSaver.LoadAllUsers();

        Assert.NotNull(users);
        Assert.Equal(2, users.Count);
        Assert.Contains(users, u => u.FirstName == "John" && u.LastName == "Doe");
        Assert.Contains(users, u => u.FirstName == "Jane" && u.LastName == "Smith");
    }

    [Fact]
    public void SaveUser_NewUser()
    {
        CleanupTestFile();

        var fileSaver = new FileSaver(_testFilePath);
        var newUser = new UserInfo { FirstName = "Peter", LastName = "Jones", Age = 40 };
        fileSaver.SaveUser(newUser);

        var loadedUsers = fileSaver.LoadAllUsers();
        Assert.Single(loadedUsers);
        Assert.Contains(loadedUsers, u => u.FirstName == "Peter" && u.LastName == "Jones");
    }


    [Fact]
    public void UpdateUser_ExistingUser() //can make changes to existing user information
    {
        CleanupTestFile();

        var initialUsers = new List<UserInfo>
        {
            new UserInfo { FirstName = "Charlie", LastName = "Garcia", Age = 28, Weight = 70 },
            new UserInfo { FirstName = "David", LastName = "Miller", Age = 31 }
        };
        File.WriteAllText(_testFilePath, JsonSerializer.Serialize(initialUsers));

        var fileSaver = new FileSaver(_testFilePath);
        var updatedUser = new UserInfo { FirstName = "Charlie", LastName = "Garcia", Age = 29, Weight = 72 };
        fileSaver.UpdateUser(updatedUser);

        var loadedUsers = fileSaver.LoadAllUsers();
        Assert.Equal(2, loadedUsers.Count);
        Assert.Contains(loadedUsers, u => u.FirstName == "Charlie" && u.LastName == "Garcia" && u.Age == 29 && u.Weight == 72);
        Assert.Contains(loadedUsers, u => u.FirstName == "David" && u.LastName == "Miller" && u.Age == 31);
    }

    [Fact]
    public void UpdateUser_NonExistingUser() //cannot overwrite information with a non-existing user
    {
        CleanupTestFile();

        var initialUsers = new List<UserInfo>
        {
            new UserInfo { FirstName = "Eve", LastName = "Moore", Age = 26 }
        };
        File.WriteAllText(_testFilePath, JsonSerializer.Serialize(initialUsers));

        var fileSaver = new FileSaver(_testFilePath);
        var nonExistingUser = new UserInfo { FirstName = "Frank", LastName = "Taylor", Age = 33 };
        fileSaver.UpdateUser(nonExistingUser);

        var loadedUsers = fileSaver.LoadAllUsers();
        Assert.Single(loadedUsers);
        Assert.Contains(loadedUsers, u => u.FirstName == "Eve" && u.LastName == "Moore" && u.Age == 26);
    }

    [Fact]
    public void InitializeWithJaneDoe_NewFile() //creates a new file with this data
    {
        CleanupTestFile();

        var fileSaver = new FileSaver(_testFilePath);
        var janeDoe = new UserInfo { FirstName = "Jane", LastName = "Doe", Age = 99 };
        fileSaver.InitializeWithJaneDoe(janeDoe);

        var loadedUsers = fileSaver.LoadAllUsers();
        Assert.Single(loadedUsers);
        Assert.Contains(loadedUsers, u => u.FirstName == "Jane" && u.LastName == "Doe" && u.Age == 99);
    }

    [Fact]
    public void InitializeWithJaneDoe_OverwritesFile() //calling this mehod overwrites an existing file
    {
        CleanupTestFile();

        
        var existingUsers = new List<UserInfo>
        {
            new UserInfo { FirstName = "Test", LastName = "User", Age = 1 }
        };
        File.WriteAllText(_testFilePath, JsonSerializer.Serialize(existingUsers));

        var fileSaver = new FileSaver(_testFilePath);
        var janeDoe = new UserInfo { FirstName = "Jane", LastName = "Doe", Age = 99 };
        fileSaver.InitializeWithJaneDoe(janeDoe);

        var loadedUsers = fileSaver.LoadAllUsers();
        Assert.Single(loadedUsers);
        Assert.Contains(loadedUsers, u => u.FirstName == "Jane" && u.LastName == "Doe" && u.Age == 99);
    }
}