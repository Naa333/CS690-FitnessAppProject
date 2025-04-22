namespace Fitness;

using System.Collections.Generic;
using Spectre.Console; 

//hosts the types of workouts and how to display and add them
public class WorkoutPlan
{
    //update the workout list to match the workouts in WorkoutManager
    private static List<string> listOfWorkouts = new List<string>
    {
        "Full Body Circuit",
        "Morning Yoga",
        "Strength Training",
        "Cardio Blast",     
        "Pilates", 
        "Dumbbell Workout", 
        "Resistance Band Exercises",
        "Bodyweight Basics",
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
        var workoutChoices = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
                .Title("[green]Select one or more workouts to add to your plan:[/]")
                .PageSize(10)
                .AddChoices(listOfWorkouts)
                .InstructionsText(
                    "[grey](Press [blue]<space>[/] to toggle a workout, " +
                    "[green]<enter>[/] to accept)[/]"
                )
        );

        List<string> actuallyAdded = new List<string>();
        List<string> alreadyPresent = new List<string>();

        foreach (var workout in workoutChoices)
        {
            if (!loggedInUser.WorkoutPlans.Contains(workout, StringComparer.OrdinalIgnoreCase))
            {
                loggedInUser.WorkoutPlans.Add(workout);
                actuallyAdded.Add(workout);
            }
            else
            {
                alreadyPresent.Add(workout);
            }
        }

        if (actuallyAdded.Any())
        {
            AnsiConsole.MarkupLine($"[green]You added: '{string.Join(", ", actuallyAdded)}' to your workout plan![/]");
        }

        if (alreadyPresent.Any())
        {
            AnsiConsole.MarkupLine($"[yellow]You already have [steelblue]'{string.Join(", ", alreadyPresent)}' [/] in your workout plan.[/]");
        }

        return actuallyAdded.Any() || alreadyPresent.Any();
    }
    
}