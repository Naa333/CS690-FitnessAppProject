using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fitness
{
    public class ConsoleUI
    {
        private readonly UserManager userManager = new();
        private readonly WorkoutPlan workoutPlan = new();
        private readonly WorkoutManager workoutManager = new();

        private static readonly Dictionary<string, double> workoutMetValues = new()
        {
            {"Running", 8.0}, {"Jogging", 4.0}, {"Morning Yoga", 2.5},
            {"Weightlifting", 4.0}, {"Swimming", 6.0}, {"Brisk walking", 3.5},
            {"Cycling", 5.5}, {"Pilates", 3.0}, {"Resistance band training", 3.0},
            {"Bodyweight exercises", 3.5}
        };

        public void Show()
        {
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

        private void HandleUserRegistration()
        {
            AnsiConsole.MarkupLine("[green]--- User Registration ---[/]");
            string firstName = AskName("First name: ");
            string lastName = AskName("Last name: ");
            int age = AskInt("Age: ", 10, 120, "Please enter a valid age (10-120).");

            string gender = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Gender:")
                    .AddChoices("Male", "Female", "Prefer not to say")
            );

            double weight = AskPositiveDouble("Enter your current weight in lbs/kg: ");

            workoutManager.DisplayAvailableGoals();
            var goal = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select your workout goal:")
                    .AddChoices(workoutManager.GetAvailableGoals())
            );

            if (userManager.CheckIfUserExists(firstName, lastName) != null)
            {
                AnsiConsole.MarkupLine("[red]User with this name already exists![/]");
                return;
            }

            var user = userManager.RegisterUser(firstName, lastName, age, gender);
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
                        HandleStartWorkoutSession(user);
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

        private void ShowWorkoutsMenu(UserInfo user)
        {
            while (true)
            {
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[green]Workout Options[/]")
                        .PageSize(5)
                        .AddChoices("Add New Workouts", "Set Goal", "Modify Plan", "View Plan", "Back")
                );

                Console.Clear();

                switch (choice)
                {
                    case "Add New Workouts":
                        workoutPlan.AddWorkout(user);
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
                    case "Back":
                        return;
                }

                userManager.UpdateUser(user);
               
            }
        }

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

      private void HandleStartWorkoutSession(UserInfo user)
        {
            if (!user.WorkoutPlans.Any())
            {
                AnsiConsole.MarkupLine("[yellow]Your workout plan is empty.[/]");
                PromptReturnToMenu();
                return;
            }

            var workout = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select a workout to start:")
                    .AddChoices(user.WorkoutPlans)
            );

            double hours = AnsiConsole.Ask<double>($"[green]Duration for '{workout}' in hours (e.g., 1 or 0.5):[/]");

            if (hours <= 0)
            {
                AnsiConsole.MarkupLine("[red]Workout duration must be greater than zero.[/]");
                PromptReturnToMenu();
                return;
            }

            // Real workout duration
            int totalSeconds = (int)(hours * 3600);

            // Simulated progress bar duration (in milliseconds)
            int simulationDurationMs = 8000; // 8 seconds total simulation
            int simulationSteps = 100;
            int sleepDurationMs = simulationDurationMs / simulationSteps;

            AnsiConsole.Progress()
                .Start(ctx =>
                {
                    var task = ctx.AddTask($"[green]{workout}[/]");
                    for (int i = 0; i <= simulationSteps; i++)
                    {
                        Thread.Sleep(sleepDurationMs);
                        task.Value = (double)i / simulationSteps * 100;
                    }

                    TimeSpan workoutDuration = TimeSpan.FromSeconds(totalSeconds);
                    AnsiConsole.MarkupLine($"\n[green]Workout '{workout}' completed![/] Duration: [cyan]{workoutDuration:hh\\:mm\\:ss}[/]");

                    // Log the workout session
                    DateTime startTime = DateTime.Now.AddSeconds(-totalSeconds);
                    DateTime endTime = DateTime.Now;
                    double caloriesBurned = workoutMetValues.TryGetValue(workout, out var met)
                        ? met * user.Weight * workoutDuration.TotalHours
                        : 0;

                    var logEntry = new WorkoutSessionLog
                    {
                        WorkoutName = workout,
                        StartTime = startTime,
                        EndTime = endTime,
                        Duration = workoutDuration,
                        CaloriesBurned = caloriesBurned
                    };

                    user.WorkoutLogs.Add(logEntry);
                    userManager.UpdateUser(user);
                    CheckAndAwardAchievements(user);
                });

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
                        _ => log.WorkoutName
                    };

                    table.AddRow(
                        log.StartTime.ToString("yyyy-MM-dd"),
                        workoutTypeColored,
                        $"[cyan]{log.Duration.ToString(@"mm\:ss")}[/]",
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
            userManager.UpdateUser(user); // Persist the awarded badge
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
        // Utility Methods

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
}
