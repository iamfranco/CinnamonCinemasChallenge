using CinnamonCinemas.Models.SeatNumberGenerators;
using System.Collections.ObjectModel;

namespace CinnamonCinemas.Models;
public class Theatre
{
    private List<Seat> _seats;
    public ReadOnlyCollection<Seat> Seats => _seats.AsReadOnly();

    public int RowCount { get; }
    public int ColumnCount { get; }

    public Theatre(int rowCount, int columnCount, SeatNumberGenerator seatNumberGenerator)
    {
        if (rowCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(rowCount));

        if (columnCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(columnCount));

        if (seatNumberGenerator is null)
            throw new ArgumentNullException(nameof(seatNumberGenerator));

        _seats = new();

        for (int rowNumber = 1; rowNumber <= rowCount; rowNumber++)
        {
            for (int columnNumber = 1; columnNumber <= columnCount; columnNumber++)
            {
                string seatNumber = seatNumberGenerator.GenerateSeatNumber(rowNumber, columnNumber);
                _seats.Add(new Seat(seatNumber));
            }
        }

        RowCount = rowCount;
        ColumnCount = columnCount;
    }

    public int GetAvailableSeatsCount() => _seats.Count(seat => seat.Status is Status.Available);

    public ReadOnlyCollection<Seat>? AllocateSeats(int numberOfSeats)
    {
        int availableSeatsCount = GetAvailableSeatsCount();
        if (availableSeatsCount < numberOfSeats)
            return null;

        List<Seat> seatsToAllocate = _seats.Where(seat => seat.Status is Status.Available)
                                           .Take(numberOfSeats).ToList();

        seatsToAllocate.ForEach(seat => seat.Allocate());

        return seatsToAllocate.AsReadOnly();
    }
}
