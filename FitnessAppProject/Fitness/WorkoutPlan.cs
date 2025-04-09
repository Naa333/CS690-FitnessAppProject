//should have the type of workout, goals and system suggestions?
namespace Fitness;

using System.Collections.Generic;
using Spectre.Console; 

public class WorkoutPlan
{
    private static List<string> listOfWorkouts = new List<string>
    {
        "Running",
        "Jogging",
        "Morning Yoga",
        "Weightlifting",
        "Swimming",
        "Brisk walking",
        "Cycling",
        "Pilates",
        "Resistance band training",
        "Bodyweight exercises"
    };

    // Method to display available workouts to the user
    public void DisplayListOfWorkouts()
    {
        AnsiConsole.MarkupLine("[underline green]Available Workouts:[/]");
        foreach (var workout in listOfWorkouts)
        {
            AnsiConsole.WriteLine($"- {workout}");
        }
    }

    // Method to allow a logged-in user to add a workout to their plan
    public bool AddWorkout(UserInfo loggedInUser)
    {
        if (loggedInUser == null)
        {
            AnsiConsole.MarkupLine("[red]No user logged in.[/]");
            return false;
        }

        

        if (listOfWorkouts.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No workouts available to add.[/]");
            return false;
        }

        var workoutChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[green]Select a workout to add to your plan:[/]")
                .PageSize(10)
                .AddChoices(listOfWorkouts)
        );

        if (!string.IsNullOrEmpty(workoutChoice))
        {
            loggedInUser.WorkoutPlans.Add(workoutChoice);
            AnsiConsole.MarkupLine($"[green]'{workoutChoice}' added to your workout plan.[/]");

            return true;
        }
        else
        {
            AnsiConsole.MarkupLine("[yellow]No workout selected.[/]");
            return false;
        }
    }
    
}