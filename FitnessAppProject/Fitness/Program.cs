namespace Fitness;
using Spectre.Console;
using System;


//the main method is handling the UI for now
class Program
{
    static void Main(string[] args)
    {
        //create a new instance of the user manager class
        UserManager userManager = new UserManager(); 
        

        AnsiConsole.WriteLine("Welcome to the fitness program!");

        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[blue]Select an option by using the arrow keys, then press enter[/]")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "Login",
                        "Create a new user",
                        "Exit" 
                    }));

            switch (choice)
            {
                case "Login":
                    AnsiConsole.MarkupLine("[green]Logging in...[/]");
                    string loginFirstName = AnsiConsole.Ask<string>("Enter your [purple3]first name[/]: ");
                    string loginLastName = AnsiConsole.Ask<string>("Enter your [purple3]last name[/]: ");
                    userManager.LoginUser(loginFirstName, loginLastName);
                    
                    if (userManager.GetLoggedInUser() != null)
                    {
        
                        AnsiConsole.MarkupLine($"[green]Successfully logged in as {userManager.GetLoggedInUser().FirstName} {userManager.GetLoggedInUser().LastName}[/]");
                        break;
                    }
                    else
                    {
                        AnsiConsole.MarkupLine($"[red]User '[yellow]{loginFirstName} {loginLastName}[/]' not found.[/]");
                    }
                    break;

                case "Create a new user":
                    AnsiConsole.MarkupLine("[green]Creating a new user...[/]");
                    userManager.RegisterUser();
                    AnsiConsole.MarkupLine("[green]New user created successfully![/]");
                    break;

                case "Exit":
                    AnsiConsole.MarkupLine("[yellow]Exiting the program.[/]");
                    return; // Exit the application

                default:
                    AnsiConsole.MarkupLine("[red]Invalid choice.[/]");
                    break;
            }

            //return to the main menu after creating the user or logging in
            if (choice != "Exit")
            {
                Console.WriteLine("\nPress any key to return to the main menu...");
                Console.ReadKey();
                Console.Clear(); // Clear the console for the next menu display
            }
        }
    }
}