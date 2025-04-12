namespace Fitness; 

using System.Collections.Generic;
using System.Text.Json.Serialization;  //to store user information for future reference

public class UserInfo  //User information will contain all these fields
{
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; }

    [JsonPropertyName("lastName")]
    public string LastName { get; set; }

    [JsonPropertyName("age")]
    public int Age { get; set; }

    [JsonPropertyName("gender")]
    public string Gender { get; set; }

    // Workout-related fields
    [JsonPropertyName("weight")]
    public double Weight { get; set; }

    [JsonPropertyName("workoutGoal")]
    public string WorkoutGoal { get; set; }

    [JsonPropertyName("workoutPlans")]
    public List<string> WorkoutPlans { get; set; } = new List<string>();

    [JsonPropertyName("achievements")]
    public List<string> AchievementBadges { get; set; } = new List<string>();

    [JsonPropertyName("workoutLogs")]
    public List<WorkoutSessionLog> WorkoutLogs { get; set; } = new List<WorkoutSessionLog>();

    public UserInfo() 
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        Gender = string.Empty;
        WorkoutGoal = string.Empty;
     } 

    public UserInfo(string firstName, string lastName, int age, string gender, double weight = 0, string workoutGoal = "", List<string>? workoutPlans = null, List<string>? achievementBadges = null)
    {
        FirstName = firstName;
        LastName = lastName;
        Age = age;
        Gender = gender;
        Weight = weight;
        WorkoutGoal = workoutGoal;
        WorkoutPlans = workoutPlans ?? new List<string>();
        AchievementBadges = achievementBadges ?? new List<string>();
    }
}