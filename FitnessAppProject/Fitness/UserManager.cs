namespace Fitness;

using System.Linq;  
using Spectre.Console;

//this class contains the user profile actions
public class UserManager
{
    private string userDataFile = "users.json"; 
    private UserInfo? loggedInUser = null;
    private FileSaver fileSaver;

    public UserManager(string? userDataFile = null)
    {
        string filePath = userDataFile ?? Environment.GetEnvironmentVariable("UserDataFile") ?? "users.json";
        this.userDataFile = filePath; // Store the resolved file path
        fileSaver = new FileSaver(filePath);
        if (fileSaver.LoadAllUsers().Count == 0) // Check if the file is empty (no users)
        {
            InitializeWithJaneDoe(); //the dummy user
        }
    }

    private void InitializeWithJaneDoe()    
    {
        UserInfo janeDoe = new UserInfo(
            "Jane",
            "Doe",
            28,
            "Female",
            50.0,
            "Maintain fitness",
            new System.Collections.Generic.List<string> { "Full Body Circuit", "Morning Yoga" },
            new System.Collections.Generic.List<string> { "First Workout", "Completed 7 Days" }
        );
        fileSaver.InitializeWithJaneDoe(janeDoe);
    }

    public UserInfo? RegisterUser(string firstName, string lastName, int age, string gender, double weight, string goal)
    {
        UserInfo newUser = new UserInfo(firstName, lastName, age, gender, weight, goal);
        fileSaver.SaveUser(newUser);
        return newUser; 
    }

    public UserInfo? LoginUser(string loginFirstName, string loginLastName)
    {
        var allUsers = fileSaver.LoadAllUsers();
        loggedInUser = allUsers.FirstOrDefault(user =>      //match the input to JSON entries
            user.FirstName?.Trim().Equals(loginFirstName.Trim(), StringComparison.OrdinalIgnoreCase) == true &&
            user.LastName?.Trim().Equals(loginLastName.Trim(), StringComparison.OrdinalIgnoreCase) == true);

        if (loggedInUser != null)
        { 
            return loggedInUser;
        }
        else
        {
            return null; 
        }
    }

    public UserInfo? GetLoggedInUser()
    {
        return loggedInUser;
    }

    public void UpdateUser(UserInfo updatedUser)
    {
        fileSaver.UpdateUser(updatedUser);
    }
    public UserInfo? CheckIfUserExists(string firstName, string lastName)
    {
        return fileSaver.LoadAllUsers().FirstOrDefault(u =>
            u.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase) &&
            u.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase)
        );
    }

    public void Logout()
    {
        loggedInUser = null;
    }
}