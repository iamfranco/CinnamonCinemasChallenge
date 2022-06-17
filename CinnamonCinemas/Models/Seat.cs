namespace CinnamonCinemas.Models;
public class Seat
{
    public string SeatNumber { get; }
    public Status Status { get; private set; }

    public Seat(string seatNumber)
    {
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
