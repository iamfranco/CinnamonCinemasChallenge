using System.Collections.ObjectModel;

namespace CinnamonCinemas.Models;
public class Theatre
{
    private List<Seat> _seats;
    public ReadOnlyCollection<Seat> Seats => _seats.AsReadOnly();

    public Theatre(List<string> rowLetters, List<int> columnNumbers)
    {
        if (rowLetters is null)
            throw new ArgumentNullException(nameof(rowLetters));

        if (rowLetters.Count == 0)
            throw new ArgumentException($"{nameof(rowLetters)} cannot be empty");

        if (columnNumbers is null)
            throw new ArgumentNullException(nameof(columnNumbers));

        if (columnNumbers.Count == 0)
            throw new ArgumentException($"{nameof(columnNumbers)} cannot be empty");

        _seats = rowLetters.SelectMany(
            _ => columnNumbers,
            (row, column) => new Seat($"{row}{column}")).ToList();
    }

    public int GetAvailableSeatsCount() => _seats.Count(seat => seat.Status is Status.Available);

    public void AllocateSeats(int numberOfSeats)
    {
        int availableSeatsCount = GetAvailableSeatsCount();
        if (availableSeatsCount < numberOfSeats)
            throw new ArgumentOutOfRangeException($"{nameof(numberOfSeats)} ({numberOfSeats}) exceed number of available seats ({availableSeatsCount})");

        List<Seat> seatsToAllocate = _seats.Where(seat => seat.Status is Status.Available)
                                           .Take(numberOfSeats).ToList();

        seatsToAllocate.ForEach(seat => seat.Allocate());
    }
}
