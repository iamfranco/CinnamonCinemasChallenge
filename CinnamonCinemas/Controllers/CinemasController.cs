using CinnamonCinemas.Models;
using CinnamonCinemas.Models.SeatNumberGenerators;
using CinnamonCinemas.Models.Seats;
using System.Collections.ObjectModel;

namespace CinnamonCinemas.Controllers;
public class CinemasController
{
    private ISeatNumberGenerator _seatNumberGenerator;
    private List<Theatre> _theatres;
    private List<Seat> _recentlyAllocatedSeats;
    public CinemasController(ISeatNumberGenerator seatNumberGenerator)
    {
        _seatNumberGenerator = seatNumberGenerator;
        _theatres = new List<Theatre>();
        _recentlyAllocatedSeats = new List<Seat>();
    }

    public Theatre? SelectedTheatre { get; private set; }
    public ReadOnlyCollection<Theatre> Theatres => _theatres.AsReadOnly();
    public ReadOnlyCollection<Seat> RecentlyAllocatedSeats => _recentlyAllocatedSeats.AsReadOnly();

    public void AddTheatre(int rowCount, int columnCount, string theatreInfo)
    {
        Theatre theatre = new Theatre(rowCount, columnCount, theatreInfo, _seatNumberGenerator);
        _theatres.Add(theatre);

        SelectedTheatre = theatre;
    }

    public void SelectTheatre(Theatre theatre)
    {
        if (theatre is null)
            throw new ArgumentNullException(nameof(theatre));

        if (!_theatres.Contains(theatre))
            throw new ArgumentException($"{nameof(theatre)} not found in list of theatres");

        _recentlyAllocatedSeats = new();
        SelectedTheatre = theatre;
    }

    public void AllocateSeatsOnSelectedTheatre(int numberOfSeats)
    {
        if (SelectedTheatre is null)
            throw new InvalidOperationException($"Cannot allocate seats before a theatre is selected.");

        var recentlyAllocatedSeats = SelectedTheatre.AllocateSeats(numberOfSeats);

        if (recentlyAllocatedSeats is null)
            throw new InvalidOperationException($"Requested for {numberOfSeats} seats, " +
                $"but only {SelectedTheatre.GetAvailableSeatsCount()} seats available.");

        _recentlyAllocatedSeats = recentlyAllocatedSeats.ToList();
    }

    public void DeleteTheatre(Theatre theatre)
    {
        if (theatre is null)
            throw new ArgumentNullException(nameof(theatre));

        if (!_theatres.Contains(theatre))
            throw new ArgumentException($"{nameof(theatre)} not found in list of theatres");

        if (theatre == SelectedTheatre)
        {
            SelectedTheatre = null;
            _recentlyAllocatedSeats = new();
        }

        _theatres.Remove(theatre);
    }
}
