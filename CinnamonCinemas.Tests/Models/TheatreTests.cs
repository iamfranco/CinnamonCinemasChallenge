using CinnamonCinemas.Models;
using CinnamonCinemas.Models.SeatNumberGenerators;
using CinnamonCinemas.Models.Seats;
using System.Collections.ObjectModel;

namespace CinnamonCinemas.Tests.Models;
internal class TheatreTests
{
    Theatre theatre;
    int rowCount;
    int columnCount;
    string theatreInfo;
    ISeatNumberGenerator seatNumberGenerator;

    [SetUp]
    public void Setup()
    {
        rowCount = 3;
        columnCount = 5;
        theatreInfo = "Doctor Strange in the Multiverse of Madness, Theatre 1, 22:30 18th June 2022";
        seatNumberGenerator = new SeatNumberGenerator();

        theatre = new Theatre(rowCount, columnCount, theatreInfo, seatNumberGenerator);
    }

    [Test]
    public void Construct_With_RowCount_Below_Or_Equal_Zero_Should_Throw_Exception()
    {
        Action act;

        act = () => theatre = new Theatre(0, columnCount, theatreInfo, seatNumberGenerator);
        act.Should().Throw<ArgumentOutOfRangeException>();

        act = () => theatre = new Theatre(-2, columnCount, theatreInfo, seatNumberGenerator);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Test]
    public void Construct_With_ColumnCount_Below_Or_Equal_Zero_Should_Throw_Exception()
    {
        Action act;

        act = () => theatre = new Theatre(rowCount, 0, theatreInfo, seatNumberGenerator);
        act.Should().Throw<ArgumentOutOfRangeException>();

        act = () => theatre = new Theatre(rowCount, -1, theatreInfo, seatNumberGenerator);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Test]
    public void Construct_With_TheatreInfo_Null_Should_Throw_Exception()
    {
        Action act;

        act = () => theatre = new Theatre(rowCount, columnCount, null, seatNumberGenerator);
        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Construct_With_seatNumberGenerator_Null_Should_Throw_Exception()
    {
        Action act;

        act = () => theatre = new Theatre(rowCount, columnCount, theatreInfo, null);
        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void RowCount_Should_Return_Constructor_RowCount()
    {
        int expectedResult = 3;
        int actualResult = theatre.RowCount;

        actualResult.Should().Be(expectedResult);
    }

    [Test]
    public void ColumnCount_Should_Return_Constructor_ColumnCount()
    {
        int expectedResult = 5;
        int actualResult = theatre.ColumnCount;

        actualResult.Should().Be(expectedResult);
    }

    [Test]
    public void TheatreInfo_Should_Return_Constructor_TheatreInfo()
    {
        string expectedResult = theatreInfo;
        string actualResult = theatre.TheatreInfo;

        actualResult.Should().Be(expectedResult);
    }

    [Test]
    public void Seats_Should_Return_List_Of_Seats_Correspond_To_Constructor_RowLetters_ColumnNumbers_Initially()
    {
        ReadOnlyCollection<Seat> expectedResult = new List<string>()
        {
            "A1", "A2", "A3", "A4", "A5",
            "B1", "B2", "B3", "B4", "B5",
            "C1", "C2", "C3", "C4", "C5",
        }.Select(seatNumber => new Seat(seatNumber))
        .ToList().AsReadOnly();

        ReadOnlyCollection<Seat> actualResult = theatre.Seats;

        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    public void GetAvailableSeatsCount_Should_Return_Total_Number_Of_Seats_Initially()
    {
        int expectedResult = 15;
        int actualResult = theatre.GetAvailableSeatsCount();

        actualResult.Should().Be(expectedResult);
    }

    [Test]
    public void AllocateSeats_With_3_Should_Return_List_Of_3_Allocated_Seats()
    {
        ReadOnlyCollection<Seat>? allocatedSeats = theatre.AllocateSeats(3);
        ReadOnlyCollection<Seat> expectedSeats = theatre.Seats.Take(3).ToList().AsReadOnly();

        allocatedSeats.Should().BeEquivalentTo(expectedSeats);
    }

    [Test]
    public void AllocateSeats_With_3_Then_GetAvailableSeatsCount_Should_Return_12()
    {
        theatre.AllocateSeats(3);

        int expectedResult = 12;
        int actualResult = theatre.GetAvailableSeatsCount();

        actualResult.Should().Be(expectedResult);
    }

    [Test]
    public void AllocateSeats_With_3_2_Should_Return_List_Of_Recent_2_Allocated_Seats()
    {
        theatre.AllocateSeats(3);

        ReadOnlyCollection<Seat>? allocatedSeats = theatre.AllocateSeats(2);
        ReadOnlyCollection<Seat> expectedSeats = theatre.Seats.Skip(3).Take(2).ToList().AsReadOnly();

        allocatedSeats.Should().BeEquivalentTo(expectedSeats);
    }

    [Test]
    public void AllocateSeats_With_2_3_1_Then_GetAvailableSeatsCount_Should_Return_9()
    {
        theatre.AllocateSeats(2);
        theatre.AllocateSeats(3);
        theatre.AllocateSeats(1);

        int expectedResult = 9;
        int actualResult = theatre.GetAvailableSeatsCount();

        actualResult.Should().Be(expectedResult);
    }

    [Test]
    public void AllocateSeats_With_15_1_Should_Return_Null()
    {
        theatre.AllocateSeats(15);

        var allocatedSeats = theatre.AllocateSeats(1);
        allocatedSeats.Should().BeNull();
    }

    [Test]
    public void AllocateSeats_With_14_2_Should_Return_Null()
    {
        theatre.AllocateSeats(14);

        var allocatedSeats = theatre.AllocateSeats(2);
        allocatedSeats.Should().BeNull();
    }
}
