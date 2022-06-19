using CinnamonCinemas.Controllers;
using CinnamonCinemas.Models;
using CinnamonCinemas.Models.SeatNumberGenerators;
using Spectre.Console;

namespace CinnamonCinemas.AppUI;
public static class AppUISections
{
    public static void SelectTheatre(
        ISeatNumberGenerator seatNumberGenerator,
        CinemasController cinemasController,
        TheatrePrinter theatrePrinter)
    {
        Console.Clear();

        var theatreInfos = cinemasController.Theatres.Select(x => x.TheatreInfo).ToList();
        theatreInfos.Add("<< Add New Theatre >>");

        string selectedTheatreInfoString = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select a [blue]theatre[/]: ")
                .AddChoices(theatreInfos));

        if (selectedTheatreInfoString == "<< Add New Theatre >>")
        {
            int rowCountInput = AnsiConsole.Prompt(
                new TextPrompt<int>("Enter [blue]number of rows[/] in theatre: ")
                    .ValidationErrorMessage("[red]Number of rows[/] must be [blue]1 or above[/].\n")
                    .Validate(x => x >= 1)
                );

            int columnCountInput = AnsiConsole.Prompt(
                new TextPrompt<int>("Enter [blue]number of columns[/] in theatre: ")
                    .ValidationErrorMessage("[red]Number of columns[/] must be [blue]1 or above[/].\n")
                    .Validate(x => x >= 1)
                );

            string theatreInfoInput = AnsiConsole.Ask<string>("Enter Theatre Info (movie name, theatre number, play time): ");

            cinemasController.AddTheatre(
                rowCountInput,
                columnCountInput,
                theatreInfoInput,
                seatNumberGenerator);
        }
        else
        {
            Theatre selectedTheatre = cinemasController.Theatres.FirstOrDefault(x => x.TheatreInfo == selectedTheatreInfoString)!;
            cinemasController.SelectTheatre(selectedTheatre);
        }

        ActionOnSelectedTheatre(seatNumberGenerator, cinemasController, theatrePrinter);
    }

    public static void ActionOnSelectedTheatre(
        ISeatNumberGenerator seatNumberGenerator,
        CinemasController cinemasController,
        TheatrePrinter theatrePrinter)
    {
        Console.Clear();
        theatrePrinter.Print(cinemasController.SelectedTheatre!, cinemasController.RecentlyAllocatedSeats);

        string theatreInfoString = cinemasController.SelectedTheatre!.TheatreInfo;

        string selectedTheatreInfoString = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($"Theatre ({theatreInfoString}) selected. \nChoose an [blue]action[/]: ")
                .AddChoices(new[] {
            "<< Allocate Seats >>",
            "<< Select Another Theatre >>",
            "<< Delete Current Theatre >>"
                }));

        if (selectedTheatreInfoString == "<< Allocate Seats >>")
            AllocateSeat(seatNumberGenerator, cinemasController, theatrePrinter);

        if (selectedTheatreInfoString == "<< Select Another Theatre >>")
            SelectTheatre(seatNumberGenerator, cinemasController, theatrePrinter);

        if (selectedTheatreInfoString == "<< Delete Current Theatre >>")
            DeleteTheatre(seatNumberGenerator, cinemasController, theatrePrinter);
    }

    public static void AllocateSeat(
        ISeatNumberGenerator seatNumberGenerator,
        CinemasController cinemasController,
        TheatrePrinter theatrePrinter)
    {
        int numberOfSeatsInput = AnsiConsole.Prompt(
            new TextPrompt<int>($"Enter [blue]number of seats[/] to allocate (between 1 or 3): ")
                .ValidationErrorMessage($"[red]Number of seats[/] must be [blue]between 1 or 3[/].\n")
                .Validate(x => x >= 1 && x <= 3)
            );

        try
        {
            cinemasController.AllocateSeatsOnSelectedTheatre(numberOfSeatsInput);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        ActionOnSelectedTheatre(seatNumberGenerator, cinemasController, theatrePrinter);
    }

    public static void DeleteTheatre(
        ISeatNumberGenerator seatNumberGenerator,
        CinemasController cinemasController,
        TheatrePrinter theatrePrinter)
    {
        string confirmDeletionInput = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[blue]Delete[/] current theatre?")
                    .AddChoices(new[] {
            "No",
            "Yes"
                    }));

        if (confirmDeletionInput == "Yes")
        {
            cinemasController.DeleteTheatre(cinemasController.SelectedTheatre!);
            SelectTheatre(seatNumberGenerator, cinemasController, theatrePrinter);
        }
        else
        {
            ActionOnSelectedTheatre(seatNumberGenerator, cinemasController, theatrePrinter);
        }
    }
}
