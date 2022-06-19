using CinnamonCinemas.Models;
using CinnamonCinemas.Models.Seats;
using System.Collections.ObjectModel;

namespace CinnamonCinemas.AppUI.TheatrePrinters;
public interface ITheatrePrinter
{
    void Print(Theatre theatre);
    void Print(Theatre theatre, ReadOnlyCollection<Seat> recentlyAllocatedSeats);
}