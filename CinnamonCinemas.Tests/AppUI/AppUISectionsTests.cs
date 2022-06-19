using CinnamonCinemas.AppUI;
using CinnamonCinemas.Controllers;
using CinnamonCinemas.Models.SeatNumberGenerators;

namespace CinnamonCinemas.Tests.AppUI;
internal class AppUISectionsTests
{
    AppUISections appUISections;
    ISeatNumberGenerator seatNumberGenerator;
    CinemasController cinemasController;
    TheatrePrinter theatrePrinter;
    [SetUp]
    public void Setup()
    {
        seatNumberGenerator = new SeatNumberGenerator();
        cinemasController = new CinemasController();
        theatrePrinter = new TheatrePrinter();

        appUISections = new AppUISections(seatNumberGenerator, cinemasController, theatrePrinter);
    }

    [Test]
    public void Construct_With_Null_SeatNumberGenerator_Should_Throw_Exception()
    {
        Action act;

        act = () => appUISections = new AppUISections(null, cinemasController, theatrePrinter);
        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Construct_With_Null_CinemasController_Should_Throw_Exception()
    {
        Action act;

        act = () => appUISections = new AppUISections(seatNumberGenerator, null, theatrePrinter);
        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Construct_With_Null_TheatrePrinter_Should_Throw_Exception()
    {
        Action act;

        act = () => appUISections = new AppUISections(seatNumberGenerator, cinemasController, null);
        act.Should().Throw<ArgumentNullException>();
    }
}
