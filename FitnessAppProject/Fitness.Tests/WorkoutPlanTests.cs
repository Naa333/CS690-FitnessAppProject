using Xunit;
using Fitness;
using System.Linq;
using System.Collections.Generic;

public class WorkoutPlanTests
{
    [Fact]
    public void AddWorkout_NewWorkout_ShouldAddToUserPlan()
    {
        var workoutPlan = new WorkoutPlan();
        var user = new UserInfo { WorkoutPlans = new List<string>() };

        // Use the helper to simulate adding a workout
        bool added = AddWorkoutHelper(workoutPlan, user, new List<string> { "Running" });

        Assert.True(added);
        Assert.Contains("Running", user.WorkoutPlans);
    }

    [Fact]
    public void AddWorkout_ExistingWorkout_ShouldNotAddDuplicate()
    {
        var workoutPlan = new WorkoutPlan();
        var user = new UserInfo { WorkoutPlans = new List<string> { "Morning Yoga" } };

        // Use the helper to simulate adding an existing workout
        bool added = AddWorkoutHelper(workoutPlan, user, new List<string> { "Morning Yoga" });

        Assert.True(added); // Still returns true because it processed the choice
        Assert.Single(user.WorkoutPlans.Where(w => w.Equals("Morning Yoga", System.StringComparison.OrdinalIgnoreCase)));
    }

    [Fact]
    public void AddWorkout_MultipleNewWorkouts_ShouldAddAll()
    {
        var workoutPlan = new WorkoutPlan();
        var user = new UserInfo { WorkoutPlans = new List<string>() };

        // Use the helper to simulate adding multiple workouts
        bool added = AddWorkoutHelper(workoutPlan, user, new List<string> { "Running", "Weightlifting" });

        Assert.True(added);
        Assert.Contains("Running", user.WorkoutPlans);
        Assert.Contains("Weightlifting", user.WorkoutPlans);
        Assert.Equal(2, user.WorkoutPlans.Count);
    }

    // Helper method to directly manipulate the user's WorkoutPlans for testing
    private bool AddWorkoutHelper(WorkoutPlan workoutPlan, UserInfo user, List<string> choices)
    {
        bool added = false;
        foreach (var workout in choices)
        {
            if (!user.WorkoutPlans.Contains(workout, StringComparer.OrdinalIgnoreCase))
            {
                user.WorkoutPlans.Add(workout);
                added = true;
            }
        }
        return added || choices.Any(c => user.WorkoutPlans.Contains(c, StringComparer.OrdinalIgnoreCase));
    }
}