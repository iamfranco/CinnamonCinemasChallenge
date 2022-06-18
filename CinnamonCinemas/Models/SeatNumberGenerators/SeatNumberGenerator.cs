namespace CinnamonCinemas.Models.SeatNumberGenerators;
public class SeatNumberGenerator
{
    private string _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public string GenerateSeatNumber(int rowNumber, int columnNumber)
    {
        if (rowNumber <= 0)
            throw new ArgumentOutOfRangeException(nameof(rowNumber));

        if (columnNumber <= 0)
            throw new ArgumentOutOfRangeException(nameof(columnNumber));

        string rowLetter = GetRowLetter(rowNumber);

        return $"{rowLetter}{columnNumber}";
    }

    private string GetRowLetter(int rowNumber)
    {
        string rowLetter = "";
        while (rowNumber > 0)
        {
            int remainder = rowNumber % _alphabet.Length;
            rowLetter = _alphabet[remainder - 1] + rowLetter;

            rowNumber /= _alphabet.Length;
        }

        return rowLetter;
    }
}
