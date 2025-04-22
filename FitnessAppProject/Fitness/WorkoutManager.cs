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
    //extend workout selection by tools to include descriptions and instructions
   private readonly List<Workout> allWorkouts = new List<Workout>()
    {
        new Workout {
            Name = "Full Body Circuit",
            RequiredTools = new List<string> { "Dumbbells", "Resistance Bands", "Yoga Mat" },
            Description = "Targets all major muscle groups.",
            Instructions = new List<string>
            {
                "1. Jumping Jacks: 30 seconds,",
                "2. Dumbbell Squats: 3 sets of 10-12 reps.",
                "3. Rest: 45 seconds.",
                "4. Resistance Band Rows: 3 sets of 12-15 reps per arm.",
                "5. Rest: 30 seconds.",
                "6. Push-ups: 3 sets.",
                "7. Plank (on mat): 3 sets, hold for 30-60 seconds.",
                "8. Repeat as needed."
            },

        },   
    
        new Workout {
            Name = "Morning Yoga",
            RequiredTools = new List<string> { "Yoga Mat" },
            Description = "A gentle yoga flow to start your day with flexibility and mindfulness.",
            Instructions = new List<string>
            {
                "1. Cat-Cow Pose (5 breaths).",
                "2. Downward-Facing Dog (5 breaths).",
                "3. Warrior I (3 breaths per side).",
                
            },
        },
        new Workout {
            Name = "Strength Training",
            RequiredTools = new List<string> { "Dumbbells", "Barbell" },
            Description = "A fundamental strength training workout focusing on compound movements.",
            Instructions = new List<string>
            {
                "1. Barbell Squats: 3 sets of 5-8 reps.",
                "2. Bench Press: 3 sets of 5-8 reps.",
                "3. Barbell Rows: 3 sets of 5-8 reps.",
            },
        },
        new Workout {
            Name = "Cardio Blast",
            RequiredTools = new List<string> { },
            Description = "A high-intensity cardio workout to boost your heart rate and burn calories.",
            Instructions = new List<string>
            {
                "1. Jumping Jacks (30 seconds).",
                "2. High Knees (30 seconds).",
                "3. Burpees (30 seconds).",
            },
        },
        new Workout {
            Name = "Pilates",
            RequiredTools = new List<string> { "Yoga Mat" },
            Description = "A series of controlled movements to improve core strength, flexibility, and posture.",
            Instructions = new List<string>
            {
                "1. The Hundred (10 reps).",
                "2. Roll-Up (5 reps).",
                "3. Single Leg Circles (5 per leg).",
                // ... more exercises ...
            },
        },
        new Workout {
            Name = "Dumbbell Workout",
            RequiredTools = new List<string> { "Dumbbells" },
            Description = "A focused strength workout using dumbbells to target major upper body muscles.",
            Instructions = new List<string>
            {
                "1. Dumbbell Bicep Curls: 3 sets of 10-12 reps.",
                "2. Dumbbell Shoulder Press: 3 sets of 8-10 reps.",
                "3. Dumbbell Chest Press: 3 sets of 10-12 reps.",
            },
        },
        new Workout {
            Name = "Resistance Band Exercises",
            RequiredTools = new List<string> { "Resistance Bands" },
            Description = "A versatile workout using resistance bands for strength and flexibility.",
            Instructions = new List<string>
            {
                "1. Banded Glute Bridges: 3 sets of 15-20 reps.",
                "2. Banded Lateral Walks: 3 sets of 10-12 steps per side.",
                "3. Banded Bicep Curls: 3 sets of 12-15 reps.",
            },
        },
        new Workout {
            Name = "Bodyweight Basics",
            RequiredTools = new List<string> { },
            Description = "A fundamental bodyweight workout perfect for beginners or when no equipment is available.",
            Instructions = new List<string>
            {
                "1. Squats: 3 sets of 10-15 reps.",
                "2. Push-ups (on knees if needed): 3 sets to failure.",
                "3. Lunges (per leg): 3 sets of 10-12 reps.",
                "4. Plank: 3 sets, hold for 20-45 seconds.",
                // ... more exercises ...
            },
        }
    };

    public Workout? GetWorkoutDetails(string workoutName)
    {
        return allWorkouts.FirstOrDefault(w => w.Name.Equals(workoutName, StringComparison.OrdinalIgnoreCase));
    }

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
            workout.RequiredTools.Any() && // Add this condition to exclude no-tool workouts
            workout.RequiredTools.All(tool => tools.Contains(tool, StringComparer.OrdinalIgnoreCase)));
    }

    public IEnumerable<Workout> GetAllWorkouts()
    {
        return allWorkouts;
    }
}

public class Workout
{
    public string Name { get; set; }  = string.Empty;
    public List<string> RequiredTools { get; set; } = new List<string>();
    public List<string> Instructions { get; set; } = new List<string>();
    public string Description { get; set; } = string.Empty;
}