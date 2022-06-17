﻿using CinnamonCinemas.Models;

namespace CinnamonCinemas.Tests.Models;
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
    public void Status_Should_Return_Available_By_Default()
    {
        seat.Status.Should().Be(Status.Available);
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

        seat.Status.Should().Be(Status.Allocated);
    }

    [Test]
    public void Allocate_On_Already_Allocated_Seat_Should_Throw_Exception()
    {
        seat.Allocate();

        Action act = () => seat.Allocate();
        act.Should().Throw<InvalidOperationException>();
    }
}
