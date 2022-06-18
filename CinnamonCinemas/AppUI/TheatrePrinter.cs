using CinnamonCinemas.Models;
using Spectre.Console;
using System.Collections.ObjectModel;

namespace CinnamonCinemas.AppUI;
public class TheatrePrinter
{
    public void Print(Theatre theatre) => Print(theatre, new List<Seat>().AsReadOnly());

    public void Print(Theatre theatre, ReadOnlyCollection<Seat> recentlyAllocatedSeats)
    {
        var table = new Table();

        table.AddColumns(Enumerable.Range(0, theatre.ColumnCount).Select(x => x.ToString()).ToArray());
        table.HideHeaders();

        Queue<Seat> seats = new(theatre.Seats);

        for (int row = 0; row < theatre.RowCount; row++)
        {
            table.AddEmptyRow();
            for (int column = 0; column < theatre.ColumnCount; column++)
            {
                Seat seat = seats.Dequeue();
                string seatNumber = seat.SeatNumber;

                if (seat.Status is Models.Status.Allocated)
                    seatNumber = $"[red]{seat.SeatNumber}[/]";

                if (recentlyAllocatedSeats.Contains(seat))
                    seatNumber = $"[orange3]{seat.SeatNumber}[/]";

                table.UpdateCell(row, column, seatNumber);
            }
        }

        AnsiConsole.Write(table);

        int totalSeatsCount = theatre.Seats.Count();
        int availableSeatsCount = theatre.GetAvailableSeatsCount();

        AnsiConsole.MarkupLine($"Available Seats : {availableSeatsCount}");
        AnsiConsole.MarkupLine($"[red]Allocated[/] Seats : {totalSeatsCount - availableSeatsCount}");

        Console.WriteLine();
    }
}
