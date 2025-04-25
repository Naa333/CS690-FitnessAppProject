namespace Fitness;

using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;

    //the UI 
    public class ConsoleUI
    {
        private readonly UserManager userManager = new();
        private readonly WorkoutPlan workoutPlan = new();
        private readonly WorkoutManager workoutManager = new();

        //Assigns estimated MET Values to each workout
        private static readonly Dictionary<string, double> workoutMetValues = new()
        {
            {"Full Body Circuit", 4.0}, 
            {"Morning Yoga", 2.5},
            {"Strength Training", 4.0}, 
            {"Cardio Blast", 8.0},     
            {"Pilates", 3.0},
            {"Dumbbell Workout", 4.5}, 
            {"Resistance Band Exercises", 3.0},
            {"Bodyweight Basics", 3.5}
            
        };
        
        public void Show()
        {   //Main menu 
            AnsiConsole.WriteLine("\n🏋️ Welcome to the Fitness Program!\n");
            AnsiConsole.WriteLine("---------------------------------------");

            while (true)
            {
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[blue]Main Menu[/]")
                        .PageSize(5)
                        .AddChoices("Login", "Create a new user", "Exit")
                );

                Console.Clear();

                switch (choice)
                {
                    case "Login":
                        HandleLogin();
                        break;
                    case "Create a new user":
                        HandleUserRegistration();
                        break;
                    case "Exit":
                        AnsiConsole.MarkupLine("[yellow]Exiting the program. Stay fit![/]");
                        return;
                }

                PromptReturnToMenu();
            }
        }
        
        //main menu methods
        private void HandleUserRegistration()
        {
            AnsiConsole.MarkupLine("[green]--- User Registration ---[/]");
            string firstName = AskName("First name: ");
            string lastName = AskName("Last name: ");

            if (userManager.CheckIfUserExists(firstName, lastName) != null)
            {
                AnsiConsole.MarkupLine("[red]User with this name already exists![/]");
                return;
            }

            int age = AskInt("Age: ", 10, 120, "Please enter a valid age (10-120).");

            string gender = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Gender:")
                    .AddChoices("Male", "Female", "Prefer not to say")
            );

            // Add weight check here
            double weight_unknown_unit = AskPositiveDouble("Enter your current weight: ");
            string weightUnit = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select your weight unit:")
                    .AddChoices("lbs", "kg")
            );

            // Convert weight to kg
            double weight = weight_unknown_unit;
            if (weightUnit.ToLower() == "lbs")
            {
                weight = weight_unknown_unit * 0.453592;
            }

            // Now 'weightKg' holds the weight in kilograms

            var goal = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select your workout goal:")
                    .AddChoices(workoutManager.GetAvailableGoals())
            );

            

            var user = userManager.RegisterUser(firstName, lastName, age, gender, weight, goal);
            AnsiConsole.MarkupLine(user != null
                ? "[green]✅ User registered successfully![/]"
                : "[red]❌ Registration failed.[/]");
        }

        private void HandleLogin()
        {
            AnsiConsole.MarkupLine("[green]--- Login ---[/]");
            string firstName = AskNonEmpty("First name: ");
            string lastName = AskNonEmpty("Last name: ");
            var user = userManager.LoginUser(firstName, lastName);

            if (user != null) ShowUserMenu(user);
            else AnsiConsole.MarkupLine("[red]❌ Login failed. User not found.[/]");
        }

        //second level menu after login
        private void ShowUserMenu(UserInfo user)
        {
            while (userManager.GetLoggedInUser() != null)
            {
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title($"[green]Welcome, {user.FirstName}! Choose an option:[/]")
                        .AddChoices("Workouts menu", "Start a new workout session", "Engagement and activity records", "Logout")
                );

                Console.Clear();

                switch (choice)
                {
                    case "Workouts menu":
                        ShowWorkoutsMenu(user);
                        break;
                    case "Start a new workout session":
                        StartWorkoutSession(user); // Ensure you await the completion
                        // After StartWorkoutSession completes, the loop will continue
                        break;
                    case "Engagement and activity records":
                        ShowEngagementActivityMenu(user);
                        break;
                    case "Logout":
                        AnsiConsole.MarkupLine($"[yellow]👋 Logged out {user.FirstName}.[/]");
                        userManager.Logout();
                        return;
                }
                userManager.UpdateUser(user);
            }
        }
        //second level menu methods
        private void ShowWorkoutsMenu(UserInfo user)
        {
            //third level menu
            while (true)
            {
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[green]Workout Options[/]")
                        .PageSize(5)
                        .AddChoices("Add a Workout Plan",  "Suggest Workout by Tools", "Set Goal", "Modify Plan", "View Plan","View Workout Instructions", "Back")
                );

                Console.Clear();

                switch (choice)
                {
                    case "Add a Workout Plan":
                        workoutPlan.AddWorkout(user);
                        break;

                    case "Suggest Workout by Tools":
                    HandleSuggestWorkoutByTools(user);
                    break;

                    case "Set Goal":
                        user.WorkoutGoal = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("Select your goal:")
                                .AddChoices(workoutManager.GetAvailableGoals())
                        );
                        AnsiConsole.MarkupLine("[green]🎯 Goal set successfully![/]");
                        break;
                    case "Modify Plan":
                        HandleModifyWorkoutPlan(user);
                        break;
                    case "View Plan":
                        HandleViewWorkoutPlan(user);
                        break;
                    case "View Workout Instructions": // New case to call the new method
                        HandleViewWorkoutInstructions(user);
                        break;
                    case "Back":
                        return;
                }

                userManager.UpdateUser(user);
               
            }
        }

        private void StartWorkoutSession(UserInfo user)
        {
            if (!user.WorkoutPlans.Any())
            {
                AnsiConsole.MarkupLine("[yellow]Your workout plan is empty. Go back to 'Workouts menu' and add a workout to start a session.[/]");
                PromptReturnToMenu();
                return;
            }

            var workoutName = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select a workout to start:")
                    .AddChoices(user.WorkoutPlans)
            );

            var selectedWorkout = workoutManager.GetWorkoutDetails(workoutName);
            if (selectedWorkout == null) return; // Handle case where workout details are not found

            // Create a copy of the Feedback dictionary for this session
            var sessionFeedback = new Dictionary<string, List<string>>(selectedWorkout.Feedback);
            double hours = AnsiConsole.Ask<double>($"[green]Duration for '{workoutName}' in hours (e.g., 1 or 0.5):[/]");

            if (hours <= 0)
            {
                AnsiConsole.MarkupLine("[red]Workout duration must be greater than zero.[/]");
                PromptReturnToMenu();
                return;
            }
            //simulate workout progress
            int totalSeconds = (int)(hours * 3600);
            int simulationDurationMs = 8000;
            int simulationSteps = 100;
            int sleepDurationMs = simulationDurationMs / simulationSteps;
            int instructionsCount = selectedWorkout.Instructions.Count;

            AnsiConsole.Progress()
                .Start(ctx =>
                {
                    var task = ctx.AddTask($"[green]{workoutName}[/]");
                    for (int i = 0; i <= simulationSteps; i++)
                    {
                        Thread.Sleep(sleepDurationMs);
                        task.Value = (double)i / simulationSteps * 100;

                        // Determine the current instruction based on the progress
                        int currentInstructionIndex = (int)((double)i / simulationSteps * instructionsCount);
                        if (currentInstructionIndex < instructionsCount)
                        {
                            string currentInstruction = selectedWorkout.Instructions[currentInstructionIndex];
                            string feedbackKey = GetFeedbackKey(currentInstruction);

                            if (sessionFeedback.TryGetValue(feedbackKey, out var feedbackList))
                            {
                                AnsiConsole.MarkupLine("\n[blue]Starting '{0}':[/]", GetExerciseName(currentInstruction));
                                foreach (var feedback in feedbackList)
                                {
                                    AnsiConsole.MarkupLine($"[grey]- {feedback}[/]");
                                }
                                // Only show feedback once per instruction
                                sessionFeedback.Remove(feedbackKey);
                            }
                        }
                    }

                    TimeSpan workoutDuration = TimeSpan.FromSeconds(totalSeconds);
                    AnsiConsole.MarkupLine($"\n[green]Workout '{workoutName}' completed![/] Duration: [cyan]{workoutDuration:hh\\:mm\\:ss}[/]");

                    // Create the WorkoutLog entry
                    var logEntry = new WorkoutSessionLog
                    {
                        StartTime = DateTime.Now,
                        WorkoutName = workoutName,
                        Duration = workoutDuration,
                        // Calculate calories burned
                        CaloriesBurned = workoutMetValues.TryGetValue(workoutName, out var metValue)
                            ? metValue * user.Weight * (hours / 60.0)
                            : 0
                    };
                    //add workout logs to user
                    user.WorkoutLogs.Add(logEntry);
                    userManager.UpdateUser(user);
                    CheckAndAwardAchievements(user);
                });

            PromptReturnToMenu();
        }

        private string GetFeedbackKey(string instruction)
        {
            return instruction.Split(' ').FirstOrDefault()?.TrimEnd(',') ?? instruction.TrimEnd(',');
        }

        private string GetExerciseName(string instruction)
        {
            var parts = instruction.Split(':');
            return parts.Length > 0 ? parts[0].Trim() : instruction.Trim();
        }

        private void ShowEngagementActivityMenu(UserInfo user)
        {
            while (true)
            {
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[green]Engagement and Activity Records[/]")
                        .PageSize(5)
                        .AddChoices("View progress report", "View achievement badges", "Back")
                );

                Console.Clear();

                switch (choice)
                {
                    case "View progress report":
                        HandleViewProgressReport(user);
                        break;
                    case "View achievement badges":
                        HandleViewAchievementBadges(user);
                        break;
                    case "Back":
                        return;
                }

                
            }
        }

        //third level menu methods
        private void HandleModifyWorkoutPlan(UserInfo user)
        {
            if (!user.WorkoutPlans.Any())
            {
                AnsiConsole.MarkupLine("[yellow]Your workout plan is currently empty.[/]");
                return;
            }

            var toRemove = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .Title("[red]Select workouts to remove:[/]")
                    .PageSize(10)
                    .AddChoices(user.WorkoutPlans)
            );

            foreach (var w in toRemove) user.WorkoutPlans.Remove(w);
            AnsiConsole.MarkupLine($"[green]Removed: {string.Join(", ", toRemove)}[/]");
        }

        private void HandleViewWorkoutPlan(UserInfo user)
        {
            AnsiConsole.MarkupLine("[green]--- Your Workout Plan ---[/]");
            if (user.WorkoutPlans.Any())
                user.WorkoutPlans.ForEach(w => AnsiConsole.MarkupLine($"[blue]- {w}[/]"));
            else
                AnsiConsole.MarkupLine("[yellow]Your workout plan is empty.[/]");
            PromptReturnToMenu();
        }

        private void HandleViewWorkoutInstructions(UserInfo user)
        {
            var allWorkoutsList = workoutManager.GetAllWorkouts().ToList();

            if (!allWorkoutsList.Any())
            {
                AnsiConsole.MarkupLine("[yellow]No workouts available to view. Consider adding a workout to your plan.[/]");
                PromptReturnToMenu();
                return;
            }

            var workoutName = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Select a workout to view instructions:[/]")
                    .AddChoices(allWorkoutsList.Select(w => w.Name))
                    .PageSize(10)
            );

            var selectedWorkout = workoutManager.GetWorkoutDetails(workoutName);

            if (selectedWorkout != null)
            {
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine($"[underline magenta2_1]{selectedWorkout.Name}[/]");
                AnsiConsole.MarkupLine($"[grey]Description: {selectedWorkout.Description}[/]");
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine($"[blue]Instructions:[/]");
                foreach (var instruction in selectedWorkout.Instructions)
                {
                    AnsiConsole.WriteLine($"- {instruction}");
                }
            }

            PromptReturnToMenu();
        }
        private void HandleViewProgressReport(UserInfo user)
        {
            AnsiConsole.MarkupLine("[green]--- Your Progress Report ---[/]");

            if (user.WorkoutLogs.Any())
            {
                var table = new Table();
                table.Border = TableBorder.SimpleHeavy;
                table.AddColumn("[b]Date[/]");
                table.AddColumn("[b]Type[/]");
                table.AddColumn(new TableColumn("[b]Time[/]").Centered());
                table.AddColumn(new TableColumn("[b]Calories Burned[/]").Centered());

                foreach (var log in user.WorkoutLogs)
                {
                    string workoutTypeColored = log.WorkoutName switch
                    {
                        "Running" => $"[green]{log.WorkoutName}[/]",
                        "Jogging" => $"[blue]{log.WorkoutName}[/]",
                        "Weightlifting" => $"[yellow]{log.WorkoutName}[/]",
                        _ => log.WorkoutName ?? "[grey]Unknown[/]"
                    };

                    table.AddRow(
                        log.StartTime.ToString("yyyy-MM-dd"),
                        workoutTypeColored ?? "[grey]Unknown[/]",
                        $"[cyan]{log.Duration.ToString(@"hh\:mm\:ss")}[/]",
                        $"[magenta]{log.CaloriesBurned:F2}[/]"
                    );
                }

                AnsiConsole.Write(table);
            }
            else
            {
                AnsiConsole.MarkupLine("[yellow]No workout sessions logged yet.[/]");
            }
           
        }

        private void HandleViewAchievementBadges(UserInfo user)
        {
            AnsiConsole.MarkupLine("[green]--- Your Achievement Badges ---[/]");

            if (user.AchievementBadges.Any())
            {
                foreach (var badgeId in user.AchievementBadges)
                {
                    string badgeName = badgeId switch
                    {
                        "FirstWorkout" => "First Workout Complete!",
                        "WorkoutStreak10" => "10 Workouts Completed!",
                        _ => badgeId
                    };
                    GenerateAchievementBadgeDisplay(badgeId, badgeName);
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[yellow]No achievement badges earned yet.[/]");
            }
            
        }
        private void HandleSuggestWorkoutByTools(UserInfo user)
        {
            var availableTools = new List<string> { "Yoga mat", "Dumbbells", "Barbells", "Resistance bands", "Treadmill", "Training bench", "Battle ropes", "Squat rack", "None" };

            var selectedTools = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .Title("[blue]Select the tools you have available:[/]")
                    .PageSize(10)
                    .AddChoices(availableTools)
                    .InstructionsText("[grey](Press <space> to toggle, <enter> to confirm)[/]")
            );

            if (selectedTools == null || selectedTools.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]No tools selected.[/]");
                PromptReturnToMenu();
                return;
            }

            var tools = selectedTools.Any(t => t.Equals("None", StringComparison.OrdinalIgnoreCase))
                ? new List<string>()
                : selectedTools;

            var allPossibleWorkouts = workoutManager.GetWorkoutsByTools(tools).ToList();
            var suggestedWorkouts = allPossibleWorkouts
                .Where(w => !user.WorkoutPlans.Contains(w.Name))
                .ToList();

            if (suggestedWorkouts.Any())
            {
                var suggestionChoice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[green]Suggested Workouts (not in your plan) based on your tools:[/]")
                        .PageSize(5)
                        .AddChoices(suggestedWorkouts.Select(w => w.Name).ToList())
                );

                var selectedWorkout = suggestedWorkouts.FirstOrDefault(w => w.Name == suggestionChoice);

                if (selectedWorkout != null)
                {
                    AnsiConsole.MarkupLine($"[green]💡 We suggest: [bold]{selectedWorkout.Name}[/][/]");

                    var addToPlan = AnsiConsole.Confirm("Would you like to add this workout to your plan?");
                    if (addToPlan)
                    {
                        if (!user.WorkoutPlans.Contains(selectedWorkout.Name))
                        {
                            user.WorkoutPlans.Add(selectedWorkout.Name);
                            AnsiConsole.MarkupLine($"[green]✅ '{selectedWorkout.Name}' added to your workout plan.[/]");
                        }
                    }
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[yellow]No new workouts found that match your selected tools.[/]");
            }

            PromptReturnToMenu();
        }

        //create achievement criteria
        public static void GenerateAchievementBadgeDisplay(string achievementId, string achievementName)
        {
            AnsiConsole.Write(new Panel($@"
            [yellow]★[/] [bold green]{achievementName}[/] [yellow]★[/]
            [grey]You've earned this badge![/]
            ")
            .Header($"[blue]Badge: {achievementId}[/]")
            .Border(BoxBorder.Rounded));
            AnsiConsole.WriteLine();
        }
        private void CheckAndAwardAchievements(UserInfo user)
        {
            if (user.WorkoutLogs.Count == 1 && !user.AchievementBadges.Contains("FirstWorkout"))
            {
                AwardAchievement(user, "FirstWorkout", "First Workout Complete!");
            }

            if (user.WorkoutLogs.Count >= 10 && !user.AchievementBadges.Contains("WorkoutStreak10"))
            {
                AwardAchievement(user, "WorkoutStreak10", "10 Workouts Completed!");
            }

            // Add more achievement checks later
        }

        //design award badges
        private void AwardAchievement(UserInfo user, string achievementId, string achievementName)
        {
            user.AchievementBadges.Add(achievementId);
            AnsiConsole.Write(new Panel($@"
            [yellow]★[/] [bold green]{achievementName}[/] [yellow]★[/]
            [grey]You've earned this badge![/]
            ")
            .Header($"[blue]Achievement Unlocked: {achievementId}[/]")
            .Border(BoxBorder.Rounded));
            AnsiConsole.WriteLine();
            userManager.UpdateUser(user); 
        }


      
        // Validation Methods

        private string AskName(string prompt)
        {
            string input;
            do
            {
                Console.Write(prompt);
                input = Console.ReadLine()?.Trim() ?? "";
                if (string.IsNullOrEmpty(input))
                    AnsiConsole.MarkupLine("[red]Name cannot be empty.[/]");
                else if (input.Any(char.IsDigit))
                    AnsiConsole.MarkupLine("[red]Name cannot contain numbers.[/]");
                else break;
            } while (true);

            return input;
        }

        private string AskNonEmpty(string prompt)
        {
            string input;
            do
            {
                Console.Write(prompt);
                input = Console.ReadLine()?.Trim() ?? "";
                if (string.IsNullOrEmpty(input))
                    AnsiConsole.MarkupLine("[red]Input cannot be empty.[/]");
            } while (string.IsNullOrEmpty(input));
            return input;
        }

        private int AskInt(string prompt, int min, int max, string error)
        {
            int value;
            do
            {
                Console.Write(prompt);
                if (!int.TryParse(Console.ReadLine(), out value) || value < min || value > max)
                    AnsiConsole.MarkupLine($"[red]{error}[/]");
                else break;
            } while (true);
            return value;
        }

        private double AskPositiveDouble(string prompt)
        {
            double value;
            do
            {
                value = AnsiConsole.Ask<double>(prompt);
                if (value <= 0)
                    AnsiConsole.MarkupLine("[red]Value must be greater than 0.[/]");
            } while (value <= 0);
            return value;
        }

        private void PromptReturnToMenu()
        {
            AnsiConsole.MarkupLine("\n[grey]Press any key to return...[/]");
            Console.ReadKey(true);
            Console.Clear();
        }
    }

