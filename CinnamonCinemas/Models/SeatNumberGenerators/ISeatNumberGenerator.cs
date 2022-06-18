namespace CinnamonCinemas.Models.SeatNumberGenerators;

public interface ISeatNumberGenerator
{
    string GenerateSeatNumber(int rowNumber, int columnNumber);
}