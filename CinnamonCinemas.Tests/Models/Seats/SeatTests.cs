using CinnamonCinemas.Models.Seats;

namespace CinnamonCinemas.Tests.Models.Seats;
internal class SeatTests
{
    Seat seat;
    [SetUp]
    public void Setup()
    {
        string seatNumber = "A1";
        seat = new(seatNumber);
    }

    [Test]
    public void Construct_With_Null_SeatNumber_Should_Throw_Exception()
    {
        Action act = () => seat = new(null);
        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Construct_With_Empty_String_SeatNumber_Should_Throw_Exception()
    {
        Action act = () => seat = new("");
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Status_Should_Return_Available_By_Default()
    {
        seat.Status.Should().Be(SeatStatus.Available);
    }

    [Test]
    public void SeatNumber_Should_Return_SeatNumber_In_Constructor()
    {
        string seatNumber = "B2";
        seat = new(seatNumber);
        seat.SeatNumber.Should().Be(seatNumber);
    }

    [Test]
    public void Allocate_Then_Status_Should_Return_Allocated()
    {
        seat.Allocate();

        seat.Status.Should().Be(SeatStatus.Allocated);
    }

    [Test]
    public void Allocate_On_Already_Allocated_Seat_Should_Throw_Exception()
    {
        seat.Allocate();

        Action act = () => seat.Allocate();
        act.Should().Throw<InvalidOperationException>();
    }
}
