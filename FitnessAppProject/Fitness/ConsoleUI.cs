using Spectre.Console;
using System;

namespace Fitness
{
    public class ConsoleUI
    {
        private readonly UserManager userManager;
        private readonly WorkoutPlan workoutPlan;

        public ConsoleUI()
        {
            userManager = new UserManager();
            workoutPlan = new WorkoutPlan();
        }

        public void Show()
        {
            AnsiConsole.WriteLine("Welcome to the fitness program!");

            while (true)
            {
                var mainMenuChoice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[blue]Select an option by using the arrow keys, then press enter[/]")
                        .PageSize(10)
                        .AddChoices(new[] {
                            "Login",
                            "Create a new user",
                            "Exit"
                        }));

                Console.Clear();

                switch (mainMenuChoice)
                {
                    case "Login":
                        HandleLogin();
                        break;

                    case "Create a new user":
                        HandleUserRegistration();
                        break;

                    case "Exit":
                        AnsiConsole.MarkupLine("[yellow]Exiting the program.[/]");
                        return;

                    default:
                        AnsiConsole.MarkupLine("[red]Invalid choice.[/]");
                        break;
                }

                Console.WriteLine("\nPress any key to return to the main menu...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        private void HandleUserRegistration()
        {
            AnsiConsole.MarkupLine("[green]Creating a new user...[/]");
            userManager.RegisterUser();
            AnsiConsole.MarkupLine("[green]New user created successfully![/]");
        }

        private void HandleLogin()
        {
            AnsiConsole.MarkupLine("[green]Logging in...[/]");
            string loginFirstName = AnsiConsole.Ask<string>("Enter your [purple3]first name[/]: ");
            string loginLastName = AnsiConsole.Ask<string>("Enter your [purple3]last name[/]: ");
            userManager.LoginUser(loginFirstName, loginLastName);

            if (userManager.GetLoggedInUser() != null)
            {
                ShowLoggedInMenu();
            }
            else
            {
                AnsiConsole.MarkupLine($"[red]Login failed. User [yellow]'{loginFirstName} {loginLastName}'[/] does not exist[/]");
            }
        }
        private void ShowLoggedInMenu()
        {
            while (userManager.GetLoggedInUser() != null)
            {
                var secondaryMenuChoice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title($"[green]Welcome, {userManager.GetLoggedInUser().FirstName}! Use the arrow keys, then press ENTER to make a selection[/]")
                        .PageSize(10)
                        .AddChoices(new[] {
                            "Add Workout to Plan",
                            "Start a new workout session [grey](Not implemented)[/]",
                            "Workouts [grey](Not implemented)[/]",
                            "Engagement and activity records [grey](Not implemented)[/]",
                            "Logout"
                        }));

                Console.Clear();

                switch (secondaryMenuChoice)
                {
                    case "Add Workout to Plan":
                        workoutPlan.AddWorkout(userManager.GetLoggedInUser());
                        userManager.UpdateUser(userManager.GetLoggedInUser());
                        break;

                    case "Start a new workout session [grey](Not implemented)[/]":
                        AnsiConsole.MarkupLine("[yellow]Feature coming soon![/]");
                        break;

                    case "Workouts [grey](Not implemented)[/]":
                        AnsiConsole.MarkupLine("[yellow]Feature coming soon![/]");
                        break;

                    case "Engagement and activity records [grey](Not implemented)[/]":
                        AnsiConsole.MarkupLine("[yellow]Feature coming soon![/]");
                        break;

                    case "Logout":
                        AnsiConsole.MarkupLine($"[yellow]{userManager.GetLoggedInUser().FirstName} has been logged out.[/]");
                        return;

                    default:
                        AnsiConsole.MarkupLine("[red]Invalid choice.[/]");
                        break;
                }

                if (secondaryMenuChoice != "Logout")
                {
                    Console.WriteLine("\nPress any key to return to the workout options menu...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }
    }
}

