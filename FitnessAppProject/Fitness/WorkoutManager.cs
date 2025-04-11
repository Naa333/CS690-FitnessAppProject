namespace Fitness;

using Spectre.Console;
using System.Collections.Generic;
using System.Linq;

//workout goals and related functions
public class WorkoutManager
{
    private static readonly List<string> availableGoals = new List<string>
    {
        "Lose Weight",
        "Build Muscle",
        "Improve Endurance",
        "Maintain Fitness"
    };
    //workout selection by tools
    private readonly List<Workout> allWorkouts = new List<Workout>()
    {
        new Workout { Name = "Full Body Circuit", RequiredTools = new List<string> { "Dumbbells", "Resistance Bands", "Yoga Mat" } },
        new Workout { Name = "Morning Yoga", RequiredTools = new List<string> { "Yoga Mat" } },
        new Workout { Name = "Strength Training", RequiredTools = new List<string> { "Dumbbells", "Barbell" } },
        new Workout { Name = "Cardio Blast", RequiredTools = new List<string> { } }, // No specific tools
        new Workout { Name = "Pilates", RequiredTools = new List<string> { "Yoga Mat" } },
        new Workout { Name = "Dumbbell Workout", RequiredTools = new List<string> { "Dumbbells" } },
        new Workout { Name = "Resistance Band Exercises", RequiredTools = new List<string> { "Resistance Bands" } },
        new Workout { Name = "Bodyweight Basics", RequiredTools = new List<string> { } }
        
    };

    public void DisplayAvailableGoals()
    {
        AnsiConsole.MarkupLine("[underline green]Available Workout Goals:[/]");
        foreach (var goal in availableGoals)
        {
            AnsiConsole.WriteLine($"- {goal}");
        }
    }

    public void SetGoal(UserInfo loggedInUser, double weight, string goalChoice)
    {
        if (loggedInUser == null)
        {
            AnsiConsole.MarkupLine("[red]No user logged in to set a goal.[/]");
            return;
        }

        loggedInUser.Weight = weight;
        loggedInUser.WorkoutGoal = goalChoice;
        AnsiConsole.MarkupLine($"[green]Your workout goal has been set to: '{loggedInUser.WorkoutGoal}'.[/]");
    }

    public List<string> GetAvailableGoals()
    {
        return availableGoals;
    }

    public IEnumerable<Workout> GetWorkoutsByTools(List<string> tools)
    {
        if (tools == null || !tools.Any() || tools.Contains("None", StringComparer.OrdinalIgnoreCase))
        {
            return allWorkouts.Where(w => !w.RequiredTools.Any()); // Suggest bodyweight or no-tool workouts
        }

        return allWorkouts.Where(workout =>
            workout.RequiredTools.All(tool => tools.Contains(tool, StringComparer.OrdinalIgnoreCase)));
    }

    public IEnumerable<Workout> GetAllWorkouts()
    {
        return allWorkouts;
    }
}

public class Workout
{
    public string Name { get; set; }
    public List<string> RequiredTools { get; set; } = new List<string>();
}