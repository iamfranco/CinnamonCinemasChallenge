namespace CinnamonCinemas.Models;
public struct Seat
{
    public string SeatNumber { get; }
    public Status Status { get; private set; }

    public Seat(string seatNumber)
    {
        if (seatNumber is null)
            throw new ArgumentNullException(nameof(seatNumber));

        if (seatNumber == String.Empty)
            throw new ArgumentException($"{nameof(seatNumber)} cannot be empty string");

        SeatNumber = seatNumber;
        Status = Status.Available;
    }

    public void Allocate()
    {
        if (Status is Status.Allocated)
            throw new InvalidOperationException($"Seat {SeatNumber} is already allocated");

        Status = Status.Allocated;
    }
}
