using CinnamonCinemas.Controllers;
using CinnamonCinemas.Models;
using CinnamonCinemas.Models.SeatNumberGenerators;
using Spectre.Console;

namespace CinnamonCinemas.AppUI;
public class AppUISections
{
    private readonly ISeatNumberGenerator _seatNumberGenerator;
    private readonly CinemasController _cinemasController;
    private readonly TheatrePrinter _theatrePrinter;

    public AppUISections(ISeatNumberGenerator seatNumberGenerator, CinemasController cinemasController, 
        TheatrePrinter theatrePrinter)
    {
        if (seatNumberGenerator is null)
            throw new ArgumentNullException(nameof(seatNumberGenerator));

        if (cinemasController is null)
            throw new ArgumentNullException(nameof(cinemasController));

        if (theatrePrinter is null)
            throw new ArgumentNullException(nameof(theatrePrinter));

        _seatNumberGenerator = seatNumberGenerator;
        _cinemasController = cinemasController;
        _theatrePrinter = theatrePrinter;
    }

    public void SelectTheatre()
    {
        Console.Clear();

        var theatreInfos = _cinemasController.Theatres.Select(x => x.TheatreInfo).ToList();
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

            string theatreInfoInput = AnsiConsole.Ask<string>("Enter Theatre Info (movie name, theatre number, start time): ");

            _cinemasController.AddTheatre(
                rowCountInput,
                columnCountInput,
                theatreInfoInput,
                _seatNumberGenerator);
        }
        else
        {
            Theatre selectedTheatre = _cinemasController.Theatres.FirstOrDefault(x => x.TheatreInfo == selectedTheatreInfoString)!;
            _cinemasController.SelectTheatre(selectedTheatre);
        }

        ActionOnSelectedTheatre();
    }

    public void ActionOnSelectedTheatre()
    {
        Console.Clear();

        string theatreInfoString = _cinemasController.SelectedTheatre!.TheatreInfo;
        Console.WriteLine($"Theatre [{theatreInfoString}]");
        
        _theatrePrinter.Print(_cinemasController.SelectedTheatre!, _cinemasController.RecentlyAllocatedSeats);

        string selectedTheatreInfoString = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose an [blue]action[/]: ")
                .AddChoices(new[] {
                    "<< Allocate Seats >>",
                    "<< Select Another Theatre >>",
                    "<< Delete Current Theatre >>"
                }));

        if (selectedTheatreInfoString == "<< Allocate Seats >>")
            AllocateSeat();

        if (selectedTheatreInfoString == "<< Select Another Theatre >>")
            SelectTheatre();

        if (selectedTheatreInfoString == "<< Delete Current Theatre >>")
            DeleteTheatre();
    }

    public void AllocateSeat()
    {
        if (_cinemasController.SelectedTheatre!.GetAvailableSeatsCount() == 0)
        {
            Console.WriteLine("Theatre already fully allocated, no seats available. \nPress Enter to continue...");
            Console.ReadLine();
            ActionOnSelectedTheatre();
        }

        int numberOfSeatsInput = AnsiConsole.Prompt(
            new TextPrompt<int>($"Enter [blue]number of seats[/] to allocate (between 1 or 3): ")
                .ValidationErrorMessage($"[red]Number of seats[/] must be [blue]between 1 or 3[/].\n")
                .Validate(x => x >= 1 && x <= 3)
            );

        try
        {
            _cinemasController.AllocateSeatsOnSelectedTheatre(numberOfSeatsInput);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        ActionOnSelectedTheatre();
    }

    public void DeleteTheatre()
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
            _cinemasController.DeleteTheatre(_cinemasController.SelectedTheatre!);
            SelectTheatre();
        }
        else
        {
            ActionOnSelectedTheatre();
        }
    }
}
