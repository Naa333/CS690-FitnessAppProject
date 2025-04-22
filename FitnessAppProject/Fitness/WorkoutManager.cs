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
        new Workout 
        {
            Name = "Full Body Circuit",
            RequiredTools = new List<string> { "Dumbbells", "Resistance Bands", "Yoga Mat" },
            Description = "Targets all major muscle groups.",
            Instructions = new List<string>
            {
                "Jumping Jacks: 30 seconds,",
                "Dumbbell Squats: 3 sets of 10-12 reps.",
                "Rest: 45 seconds.",
                "Resistance Band Rows: 3 sets of 12-15 reps per arm.",
                "Rest: 30 seconds.",
                "Push ups: 3 sets.",
                "Plank (on mat): 3 sets, hold for 30-60 seconds.",
                "Repeat as needed."
            },
            //add feedback to be displayed at runtime
            Feedback = new Dictionary<string, List<string>>
            {
                {"Jumping", new List<string>{"Keep your arms and legs moving in sync.", "Land softly on the balls of your feet."}},
                {"Dumbbell", new List<string>{"Keep your back straight.", "Lower your hips as if sitting in a chair.", "Ensure your knees don't go past your toes."}},
                {"Push", new List<string>{"Do not flare your elbows"}}
            }
        },   
    
        new Workout {
            Name = "Morning Yoga",
            RequiredTools = new List<string> { "Yoga Mat" },
            Description = "A gentle yoga flow to start your day with flexibility and mindfulness.",
            Instructions = new List<string>
            {
                "Cat-Cow Pose (5 breaths).",
                "Downward-Facing Dog (5 breaths).",
                "Warrior I (3 breaths per side).",
                
            },
            Feedback = new Dictionary<string, List<string>>
            {
                {"Cat-Cow", new List<string>{"Coordinate your breath with your movement."}},
                {"Downward-Facing", new List<string>{"Try to lengthen your spine."}},
                {"Warrior", new List<string>{"Ensure your front knee is over your ankle."}}
            }
        },
        new Workout {
            Name = "Strength Training",
            RequiredTools = new List<string> { "Dumbbells", "Barbell" },
            Description = "A fundamental strength training workout focusing on compound movements.",
            Instructions = new List<string>
            {
                "Barbell Squats: 3 sets of 5-8 reps.",
                "Bench Press: 3 sets of 5-8 reps.",
                "Barbell Rows: 3 sets of 5-8 reps.",
            },
            Feedback = new Dictionary<string, List<string>>
            {
                {"Barbell", new List<string>{"Maintain a neutral spine."}},
                {"Bench", new List<string>{"Lower the bar with control."}},
                {"Rows", new List<string>{"Pull the bar towards your chest."}}
            }
        },
        new Workout {
            Name = "Cardio Blast",
            RequiredTools = new List<string> { },
            Description = "A high-intensity cardio workout to boost your heart rate and burn calories.",
            Instructions = new List<string>
            {
                "Jumping Jacks (30 seconds).",
                "High Knees (30 seconds).",
                "Burpees (30 seconds).",
            },
            Feedback = new Dictionary<string, List<string>>
            {
                {"Jumping", new List<string>{"Keep a steady pace."}},
                {"High", new List<string>{"Bring your knees up high."}},
                {"Burpees", new List<string>{"Maintain a fluid movement."}}
            }
        },
        new Workout {
            Name = "Pilates",
            RequiredTools = new List<string> { "Yoga Mat" },
            Description = "A series of controlled movements to improve core strength, flexibility, and posture.",
            Instructions = new List<string>
            {
                "The Hundred (10 reps).",
                "Roll-Up (5 reps).",
                "Single Leg Circles (5 per leg).",
                // ... more exercises ...
            },
            Feedback = new Dictionary<string, List<string>>
            {
                {"Hundred", new List<string>{"Engage your deep core muscles."}},
                {"Roll-Up", new List<string>{"Move with control and precision."}},
                {"Leg", new List<string>{"Keep your hips stable."}}
            }
        },
        new Workout {
            Name = "Dumbbell Workout",
            RequiredTools = new List<string> { "Dumbbells" },
            Description = "A focused strength workout using dumbbells to target major upper body muscles.",
            Instructions = new List<string>
            {
                "Dumbbell Bicep Curls: 3 sets of 10-12 reps.",
                "Dumbbell Shoulder Press: 3 sets of 8-10 reps.",
                "Dumbbell Chest Press: 3 sets of 10-12 reps.",
            },
            Feedback = new Dictionary<string, List<string>>
            {
                {"Bicep", new List<string>{"Keep your elbows tucked in."}},
                {"Shoulder", new List<string>{"Press the dumbbells straight overhead."}},
                {"Chest", new List<string>{"Lower the dumbbells with control."}}
            }
        },
        new Workout {
            Name = "Resistance Band Exercises",
            RequiredTools = new List<string> { "Resistance Bands" },
            Description = "A versatile workout using resistance bands for strength and flexibility.",
            Instructions = new List<string>
            {
                "Banded Glute Bridges: 3 sets of 15-20 reps.",
                "Banded Lateral Walks: 3 sets of 10-12 steps per side.",
                "Banded Bicep Curls: 3 sets of 12-15 reps.",
            },
            Feedback = new Dictionary<string, List<string>>
            {
                {"Glute", new List<string>{"Squeeze your glutes at the top."}},
                {"Lateral", new List<string>{"Maintain tension on the band."}},
                {"Banded Bicep", new List<string>{"Control the resistance on the way back."}}
            }
        },
        new Workout {
            Name = "Bodyweight Basics",
            RequiredTools = new List<string> { },
            Description = "A fundamental bodyweight workout perfect for beginners or when no equipment is available.",
            Instructions = new List<string>
            {
                "Squats: 3 sets of 10-15 reps.",
                "Push-ups (on knees if needed): 3 sets to failure.",
                "Lunges (per leg): 3 sets of 10-12 reps.",
                "Plank: 3 sets, hold for 20-45 seconds.",
                // ... more exercises ...
            },
            Feedback = new Dictionary<string, List<string>>
            {
                {"Squats", new List<string>{"Keep your chest up."}},
                {"Push-ups", new List<string>{"Engage your core."}},
                {"Lunges", new List<string>{"Ensure your front knee doesn't go past your toes."}},
                {"Plank", new List<string>{"Avoid letting your hips drop or rise too high."}}
            }
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
    public Dictionary<string, List<string>> Feedback { get; set; } = new Dictionary<string, List<string>>();
}
