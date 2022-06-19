using CinnamonCinemas.AppUI;
using CinnamonCinemas.Controllers;
using CinnamonCinemas.Models.SeatNumberGenerators;

ISeatNumberGenerator seatNumberGenerator = new SeatNumberGenerator();
CinemasController cinemasController = new CinemasController();
TheatrePrinter theatrePrinter = new TheatrePrinter();

cinemasController.AddTheatre(
        rowCount: 3,
        columnCount: 5,
        theatreInfo: "Doctor Strange in the Multiverse of Madness, Theatre 1, 22:30 18th June 2022",
        seatNumberGenerator);

AppUISections.SelectTheatre(seatNumberGenerator, cinemasController, theatrePrinter);