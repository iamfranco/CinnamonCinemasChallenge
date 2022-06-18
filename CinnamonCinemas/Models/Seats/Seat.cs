namespace CinnamonCinemas.Models.Seats;
public class Seat
{
    public string SeatNumber { get; }
    public SeatStatus Status { get; private set; }

    public Seat(string seatNumber)
    {
        if (seatNumber is null)
            throw new ArgumentNullException(nameof(seatNumber));

        if (seatNumber == string.Empty)
            throw new ArgumentException($"{nameof(seatNumber)} cannot be empty string");

        SeatNumber = seatNumber;
        Status = SeatStatus.Available;
    }

    public void Allocate()
    {
        if (Status is SeatStatus.Allocated)
            throw new InvalidOperationException($"Seat {SeatNumber} is already allocated");

        Status = SeatStatus.Allocated;
    }
}
