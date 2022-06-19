using CinnamonCinemas.AppUI;
using CinnamonCinemas.Controllers;
using CinnamonCinemas.Models.SeatNumberGenerators;

ISeatNumberGenerator seatNumberGenerator = new SeatNumberGenerator();
CinemasController cinemasController = new CinemasController(seatNumberGenerator);
TheatrePrinter theatrePrinter = new TheatrePrinter();
AppUISections appUISection = new AppUISections(cinemasController, theatrePrinter);

cinemasController.AddTheatre(
        rowCount: 3,
        columnCount: 5,
        theatreInfo: "Doctor Strange in the Multiverse of Madness, Theatre 1, 22:30 18th June 2022"
        );

appUISection.SelectTheatre();