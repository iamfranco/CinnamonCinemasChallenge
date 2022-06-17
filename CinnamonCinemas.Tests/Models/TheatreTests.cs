using CinnamonCinemas.Models;

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

}
