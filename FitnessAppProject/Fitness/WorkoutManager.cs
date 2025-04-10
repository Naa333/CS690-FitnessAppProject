namespace Fitness;
using Spectre.Console;
using System.Collections.Generic;


public class WorkoutManager{
    private static readonly List<string> availableGoals = new List<string>
    {
        "Lose Weight",
        "Build Muscle",
        "Improve Endurance",
        "Maintain Fitness"
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
}