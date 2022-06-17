namespace CinnamonCinemas.Models;
public class Theatre
{
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
    }
}
