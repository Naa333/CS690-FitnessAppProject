namespace Fitness.Tests;

using Xunit;
using Fitness;
using System.Collections.Generic;
using System;
using System.Linq;

public class UserInfoTests
{
    [Fact]
    public void UserInfo_DefaultConstructor() //initializes to default values correctly
    {
        var user = new UserInfo();
        
        Assert.Equal(string.Empty, user.FirstName);
        Assert.Equal(string.Empty, user.LastName);
        Assert.Equal(string.Empty, user.Gender);
        Assert.Equal(string.Empty, user.WorkoutGoal);
        Assert.Equal(0, user.Age); // Default value for int
        Assert.Equal(0, user.Weight); // Default value for double
        Assert.NotNull(user.WorkoutPlans); // Lists are initialized
        Assert.NotNull(user.AchievementBadges);
        Assert.NotNull(user.WorkoutLogs);
    }

    [Fact]
    public void UserInfo_ParameterizedConstructor() //initializes correctly with parameters 
    {
        string firstName = "Test";
        string lastName = "User";
        int age = 30;
        string gender = "Male";
        double weight = 75.5;
        string workoutGoal = "Build Muscle";
        var workoutPlans = new List<string> { "Running" };
        var achievementBadges = new List<string> { "FirstWorkout" };

        var user = new UserInfo(firstName, lastName, age, gender, weight, workoutGoal, workoutPlans, achievementBadges);

        Assert.Equal(firstName, user.FirstName);
        Assert.Equal(lastName, user.LastName);
        Assert.Equal(age, user.Age);
        Assert.Equal(gender, user.Gender);
        Assert.Equal(weight, user.Weight);
        Assert.Equal(workoutGoal, user.WorkoutGoal);
        Assert.NotNull(user.WorkoutPlans);
        Assert.Single(user.WorkoutPlans);
        Assert.Contains("Running", user.WorkoutPlans);
        Assert.NotNull(user.AchievementBadges);
        Assert.Single(user.AchievementBadges);
        Assert.Contains("FirstWorkout", user.AchievementBadges);
        Assert.NotNull(user.WorkoutLogs);
        Assert.Empty(user.WorkoutLogs); // Constructor doesn't initialize WorkoutLogs
    }

   
    [Fact]
    public void UserInfo_SettingProperties() //setting properties after initialization
    {
        var user = new UserInfo();
        string newGoal = "Lose Weight";
        double newWeight = 70.0;

        user.WorkoutGoal = newGoal;
        user.Weight = newWeight;
        user.WorkoutPlans.Add("Cycling");
        user.AchievementBadges.Add("WorkoutStreak5");

        Assert.Equal(newGoal, user.WorkoutGoal);
        Assert.Equal(newWeight, user.Weight);
        Assert.Contains("Cycling", user.WorkoutPlans);
        Assert.Contains("WorkoutStreak5", user.AchievementBadges);
    }

    [Fact]
    public void UserInfo_AddingWorkouts()   //adding workouts after initialization
    {
        var user = new UserInfo();
        var log1 = new WorkoutSessionLog { WorkoutName = "Jogging", Duration = TimeSpan.FromMinutes(30) };
        var log2 = new WorkoutSessionLog { WorkoutName = "Cycling", Duration = TimeSpan.FromHours(1) };

        user.WorkoutLogs.Add(log1);
        user.WorkoutLogs.Add(log2);

        Assert.Contains(log1, user.WorkoutLogs);
        Assert.Contains(log2, user.WorkoutLogs);
        Assert.Equal(2, user.WorkoutLogs.Count);
    }
}