using CinnamonCinemas.Models;
using System.Collections.ObjectModel;

namespace CinnamonCinemas.Tests.Models;
internal class TheatreTests
{
    Theatre theatre;
    List<string> rowLetters;
    List<int> columnNumbers;
    [SetUp]
    public void Setup()
    {
        rowLetters = new() { "A", "B", "C" };
        columnNumbers = new() { 1, 2, 3, 4, 5 };
        theatre = new Theatre(rowLetters, columnNumbers);
    }

    [Test]
    public void Construct_With_Null_RowLetters_Should_Throw_Exception()
    {
        rowLetters = null;

        Action act = () => theatre = new Theatre(rowLetters, columnNumbers);
        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Construct_With_Empty_RowLetters_Should_Throw_Exception()
    {
        rowLetters = new() { };

        Action act = () => theatre = new Theatre(rowLetters, columnNumbers);
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Construct_With_Null_ColumnNumbers_Should_Throw_Exception()
    {
        columnNumbers = null;

        Action act = () => theatre = new Theatre(rowLetters, columnNumbers);
        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Construct_With_Empty_ColumnNumbers_Should_Throw_Exception()
    {
        columnNumbers = new() { };

        Action act = () => theatre = new Theatre(rowLetters, columnNumbers);
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Seats_Should_Return_List_Of_Seats_Correspond_To_Constructor_RowLetters_ColumnNumbers_Initially()
    {
        rowLetters = new() { "A", "B", "C" };
        columnNumbers = new() { 1, 2, 3, 4, 5 };
        theatre = new Theatre(rowLetters, columnNumbers);

        ReadOnlyCollection<Seat> expectedResult = rowLetters.SelectMany(
            _ => columnNumbers,
            (row, column) => new Seat($"{row}{column}")).ToList().AsReadOnly();

        ReadOnlyCollection<Seat> actualResult = theatre.Seats;

        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    public void GetAvailableSeatsCount_Should_Return_Total_Number_Of_Seats_Initially()
    {
        rowLetters = new() { "A", "B", "C" };
        columnNumbers = new() { 1, 2, 3, 4, 5 };
        theatre = new Theatre(rowLetters, columnNumbers);

        int expectedResult = 15;
        int actualResult = theatre.GetAvailableSeatsCount();

        actualResult.Should().Be(expectedResult);
    }

    [Test]
    public void AllocateSeats_With_3_Should_Return_List_Of_3_Allocated_Seats()
    {
        ReadOnlyCollection<Seat> allocatedSeats = theatre.AllocateSeats(3);
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

        ReadOnlyCollection<Seat> allocatedSeats = theatre.AllocateSeats(2);
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
