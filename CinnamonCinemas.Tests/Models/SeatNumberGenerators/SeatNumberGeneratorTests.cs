using CinnamonCinemas.Models.SeatNumberGenerators;

namespace CinnamonCinemas.Tests.Models.SeatNumberGenerators;
internal class SeatNumberGeneratorTests
{
    SeatNumberGenerator seatNumberGenerator;
    [SetUp]
    public void Setup()
    {
        seatNumberGenerator = new();
    }

    [Test]
    public void GenerateSeatNumber_With_1_1_Should_Return_A1()
    {
        seatNumberGenerator.GenerateSeatNumber(1, 1).Should().Be("A1");
    }

    [Test]
    public void GenerateSeatNumber_With_3_2_Should_Return_A1()
    {
        seatNumberGenerator.GenerateSeatNumber(3, 2).Should().Be("C2");
    }

    [Test]
    public void GenerateSeatNumber_With_27_3_Should_Return_A1()
    {
        seatNumberGenerator.GenerateSeatNumber(28, 3).Should().Be("AB3");
    }

    [Test]
    public void GenerateSeatNumber_With_RowNumber_Below_Or_Equal_To_Zero_Should_Throw_Exception()
    {
        Action act;

        act = () => seatNumberGenerator.GenerateSeatNumber(0, 1);
        act.Should().Throw<ArgumentOutOfRangeException>();

        act = () => seatNumberGenerator.GenerateSeatNumber(-2, 1);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Test]
    public void GenerateSeatNumber_With_ColumnNumber_Below_Or_Equal_To_Zero_Should_Throw_Exception()
    {
        Action act;

        act = () => seatNumberGenerator.GenerateSeatNumber(1, 0);
        act.Should().Throw<ArgumentOutOfRangeException>();

        act = () => seatNumberGenerator.GenerateSeatNumber(1, -2);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}
