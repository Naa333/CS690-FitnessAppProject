namespace Fitness;

using System.Linq;  
using Spectre.Console;

public class UserManager
{
    private string userDataFile = "users.json"; 
    private UserInfo? loggedInUser = null;
    private FileSaver fileSaver;

    public UserManager()
    {
        fileSaver = new FileSaver(userDataFile);
        if (fileSaver.LoadAllUsers().Count == 0) // Check if the file is empty (no users)
        {
            InitializeWithJaneDoe();
        }
    }

    private void InitializeWithJaneDoe()    //the dummy user
    {
        UserInfo janeDoe = new UserInfo(
            "Jane",
            "Doe",
            28,
            "Female",
            165.0,
            60.0,
            50.0,
            "Maintain fitness",
            new System.Collections.Generic.List<string> { "Full Body Circuit", "Morning Yoga" },
            new System.Collections.Generic.List<string> { "First Workout", "Completed 7 Days" }
        );
        fileSaver.InitializeWithJaneDoe(janeDoe);
    }

    public UserInfo? RegisterUser(string firstName, string lastName, int age, string gender)
    {
        UserInfo newUser = new UserInfo(firstName, lastName, age, gender);
        fileSaver.SaveUser(newUser);
        return newUser; // Return the newly created user
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
}