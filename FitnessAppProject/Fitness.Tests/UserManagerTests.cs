using System;
using System.IO;
using Xunit;
using Fitness;
using System.Collections.Generic;

public class UserManagerTests : IDisposable
{
    private readonly string tempFile;
    private readonly UserManager userManager;

    public UserManagerTests()
    {
        tempFile = Path.GetTempFileName(); // create isolated file for test
        userManager = new UserManager(tempFile);
    }

    public void Dispose()
    {
        if (File.Exists(tempFile))
            File.Delete(tempFile);
    }

    [Fact]
    public void RegisterUser_ShouldAddNewUser()
    {
        var user = userManager.RegisterUser("John", "Doe", 30, "Male");

        Assert.NotNull(user);
        Assert.Equal("John", user.FirstName);
        Assert.Equal("Doe", user.LastName);
        Assert.Equal(30, user.Age);
        Assert.Equal("Male", user.Gender);
    }

    [Fact]
    public void LoginUser_ShouldReturnUser_WhenCredentialsMatch()
    {
        userManager.RegisterUser("Alice", "Smith", 25, "Female");

        var loggedIn = userManager.LoginUser("Alice", "Smith");

        Assert.NotNull(loggedIn);
        Assert.Equal("Alice", loggedIn?.FirstName);
        Assert.Equal("Smith", loggedIn?.LastName);
    }

    [Fact]
    public void LoginUser_ShouldReturnNull_WhenUserNotFound()
    {
        var loggedIn = userManager.LoginUser("Not", "Exist");

        Assert.Null(loggedIn);
    }

    [Fact]
    public void GetLoggedInUser_ShouldReturnUser_AfterLogin()
    {
        userManager.RegisterUser("Tom", "Hanks", 50, "Male");
        userManager.LoginUser("Tom", "Hanks");

        var loggedIn = userManager.GetLoggedInUser();

        Assert.NotNull(loggedIn);
        Assert.Equal("Tom", loggedIn?.FirstName);
    }

    [Fact]
    public void Logout_ShouldClearLoggedInUser()
    {
        userManager.RegisterUser("Eva", "Green", 35, "Female");
        userManager.LoginUser("Eva", "Green");

        userManager.Logout();
        var result = userManager.GetLoggedInUser();

        Assert.Null(result);
    }

    [Fact]
    public void CheckIfUserExists_ShouldReturnUser_IfExists()
    {
        userManager.RegisterUser("Mia", "Wong", 22, "Female");

        var exists = userManager.CheckIfUserExists("Mia", "Wong");

        Assert.NotNull(exists);
        Assert.Equal("Mia", exists?.FirstName);
    }

    [Fact]
    public void CheckIfUserExists_ShouldReturnNull_IfNotExists()
    {
        var result = userManager.CheckIfUserExists("No", "Name");

        Assert.Null(result);
    }

    [Fact]
    
    public void UpdateUser_ShouldChangeUserData()
    {
        var user = userManager.RegisterUser("Bob", "Marley", 36, "Male");

        // Add a null check here to satisfy the compiler
        if (user != null)
        {
            user.Weight = 65.0;
            userManager.UpdateUser(user);

            var reloaded = userManager.LoginUser("Bob", "Marley");
            Assert.NotNull(reloaded);
            Assert.Equal(65.0, reloaded.Weight);
        }
        else
        {
            Assert.Fail("RegisterUser returned null unexpectedly.");
        }
    }

    [Fact]
    public void InitializeWithJaneDoe_ShouldExistOnFirstRun()
    {
        var jane = userManager.LoginUser("Jane", "Doe");
        Assert.NotNull(jane);
        Assert.Equal("Jane", jane?.FirstName);
        Assert.Equal("Doe", jane?.LastName);
    }
}
