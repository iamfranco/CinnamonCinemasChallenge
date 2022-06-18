using CinnamonCinemas.AppUI;
using CinnamonCinemas.Models;
using CinnamonCinemas.Models.SeatNumberGenerators;
using Spectre.Console;

SeatNumberGenerator seatNumberGenerator = new SeatNumberGenerator();
Theatre theatre = new Theatre(rowCount: 3, columnCount: 5, seatNumberGenerator);
TheatrePrinter theatrePrinter = new TheatrePrinter();

theatrePrinter.Print(theatre);
while (true)
{
    int availableSeatsCount = theatre.GetAvailableSeatsCount();

    if (availableSeatsCount == 0)
    {
        Console.WriteLine("All seats are allocated, no more seats to sell.");
        break;
    }

    int numberOfSeats = AnsiConsole.Prompt(
        new TextPrompt<int>("Enter [blue]number of seats[/] to purchase (between 1 and 3): ")
            .ValidationErrorMessage("[red]Number of seats[/] must be [blue]between 1 and 3[/].\n")
            .Validate(x => x >= 1 && x <= 3)
        );

    var allocatedSeats = theatre.AllocateSeats(numberOfSeats);
    
    if (allocatedSeats is null)
    {
        string seatsString = availableSeatsCount > 1 ? "seats" : "seat";

        AnsiConsole.MarkupLine($"Customer wanted [orange3]{numberOfSeats}[/] seats, " +
            $"but we only have [red]{availableSeatsCount}[/] {seatsString} available.\n");
        continue;
    }

    Console.Clear();
    theatrePrinter.Print(theatre, allocatedSeats);

    var allocatedSeatNumbers = allocatedSeats.Select(seat => seat.SeatNumber).ToList();
    AnsiConsole.MarkupLine($"Customer purchased [orange3]{numberOfSeats}[/] seats, " +
        $"allocated at [orange3]{string.Join(", ", allocatedSeatNumbers)}[/]\n");
}

Console.ReadLine();