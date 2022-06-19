using CinnamonCinemas.Controllers;
using CinnamonCinemas.Models;
using CinnamonCinemas.Models.SeatNumberGenerators;
using CinnamonCinemas.Models.Seats;
using System.Collections.ObjectModel;

namespace CinnamonCinemas.Tests.Controllers;
internal class CinemasControllerTests
{
    int rowCount;
    int columnCount;
    string theatreInfo;
    ISeatNumberGenerator seatNumberGenerator;

    CinemasController cinemasController;
    [SetUp]
    public void Setup()
    {
        rowCount = 3;
        columnCount = 5;
        theatreInfo = "Doctor Strange in the Multiverse of Madness, Theatre 1, 22:30 18th June 2022";
        seatNumberGenerator = new SeatNumberGenerator();

        cinemasController = new CinemasController(seatNumberGenerator);
    }

    [Test]
    public void Theatres_Should_Return_Empty_List_Of_Theatres_By_Default()
    {
        ReadOnlyCollection<Theatre> theatres = cinemasController.Theatres;
        theatres.Count.Should().Be(0);
        theatres.Should().Equal(new List<Theatre>() { }.AsReadOnly());
    }

    [Test]
    public void SelectedTheatre_Should_Return_Null_By_Default()
    {
        Theatre? theatres = cinemasController.SelectedTheatre;
        theatres.Should().Be(null);
    }

    [Test]
    public void RecentlyAllocatedSeats_Should_Return_Empty_List_Of_Seats_By_Default()
    {
        ReadOnlyCollection<Seat> recentlyAllocatedSeats = cinemasController.RecentlyAllocatedSeats;
        recentlyAllocatedSeats.Count.Should().Be(0);
        recentlyAllocatedSeats.Should().Equal(new List<Seat>() { }.AsReadOnly());
    }

    [Test]
    public void AddTheatre_With_Valid_Inputs_Then_Theatres_Should_Contain_Added_Theatre()
    {
        cinemasController.AddTheatre(rowCount, columnCount, theatreInfo);
        cinemasController.AddTheatre(4, 6, "some info");

        ReadOnlyCollection<Theatre> theatres = cinemasController.Theatres;

        theatres.Count.Should().Be(2);
        theatres.Should().BeEquivalentTo(new List<Theatre>() {
            new Theatre(rowCount, columnCount, theatreInfo, seatNumberGenerator),
            new Theatre(4, 6, "some info", seatNumberGenerator),
        }.AsReadOnly());
    }

    [Test]
    public void AddTheatre_With_Valid_Inputs_Then_SelectedTheatre_Should_Return_Last_Added_Theatre()
    {
        cinemasController.AddTheatre(rowCount, columnCount, theatreInfo);
        cinemasController.AddTheatre(4, 6, "some info");

        Theatre? selectedTheatre = cinemasController.SelectedTheatre;
        selectedTheatre.Should().NotBeNull();
        selectedTheatre.RowCount.Should().Be(4);
        selectedTheatre.ColumnCount.Should().Be(6);
        selectedTheatre.TheatreInfo.Should().Be("some info");
    }

    [Test]
    public void AddTheatre_With_Invalid_Inputs_Based_On_Theatre_Constructor_Should_Throw_Exception()
    {
        Action act;

        act = () => cinemasController.AddTheatre(0, columnCount, theatreInfo);
        act.Should().Throw<ArgumentOutOfRangeException>();

        act = () => cinemasController.AddTheatre(-2, columnCount, theatreInfo);
        act.Should().Throw<ArgumentOutOfRangeException>();

        act = () => cinemasController.AddTheatre(rowCount, 0, theatreInfo);
        act.Should().Throw<ArgumentOutOfRangeException>();

        act = () => cinemasController.AddTheatre(rowCount, -1, theatreInfo);
        act.Should().Throw<ArgumentOutOfRangeException>();

        act = () => cinemasController.AddTheatre(rowCount, columnCount, null);
        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void SelectTheatre_With_Null_Input_Theatre_Should_Throw_Exception()
    {
        Action act;

        act = () => cinemasController.SelectTheatre(null);
        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void SelectTheatre_With_Input_Theatre_Not_In_Theatres_List_Should_Throw_Exception()
    {
        cinemasController.AddTheatre(rowCount, columnCount, theatreInfo);
        Theatre theatre = new(3, 3, "some info", seatNumberGenerator);

        Action act;

        act = () => cinemasController.SelectTheatre(theatre);
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void SelectTheatre_With_Input_Theatre_In_Theatres_Then_SelectedTheatre_Should_Return_Input_Theatre()
    {
        cinemasController.AddTheatre(rowCount, columnCount, theatreInfo);
        cinemasController.AddTheatre(4, 6, "some info");

        Theatre theatre = cinemasController.Theatres[0];
        cinemasController.SelectTheatre(theatre);

        cinemasController.SelectedTheatre.Should().Be(theatre);
    }

    [Test]
    public void SelectTheatre_With_Input_Theatre_In_Theatres_Then_RecentlyAllocatedSeats_Should_Return_Empty_List_Of_Seats()
    {
        cinemasController.AddTheatre(rowCount, columnCount, theatreInfo);
        cinemasController.AddTheatre(4, 6, "some info");

        cinemasController.AllocateSeatsOnSelectedTheatre(3);

        Theatre theatre = cinemasController.Theatres[0];
        cinemasController.SelectTheatre(theatre);

        ReadOnlyCollection<Seat> recentlyAllocatedSeats = cinemasController.RecentlyAllocatedSeats;
        recentlyAllocatedSeats.Count.Should().Be(0);
        recentlyAllocatedSeats.Should().Equal(new List<Seat>() { }.AsReadOnly());
    }

    [Test]
    public void AllocateSeatsOnSelectedTheatre_Before_Any_Theatre_Selected_Should_Throw_Exception()
    {
        int numberOfSeats = 3;
        Action act;

        act = () => cinemasController.AllocateSeatsOnSelectedTheatre(numberOfSeats);
        act.Should().Throw<InvalidOperationException>();
    }

    [Test]
    public void AllocateSeatsOnSelectedTheatre_Should_Allocate_Seats_On_Selected_Theatre()
    {
        cinemasController.AddTheatre(rowCount, columnCount, theatreInfo);
        cinemasController.AddTheatre(4, 6, "some info");

        cinemasController.AllocateSeatsOnSelectedTheatre(3);
        cinemasController.SelectedTheatre.GetAvailableSeatsCount().Should().Be(21);

        cinemasController.AllocateSeatsOnSelectedTheatre(2);
        cinemasController.SelectedTheatre.GetAvailableSeatsCount().Should().Be(19);
    }

    [Test]
    public void AllocateSeatsOnSelectedTheatre_Successful_Then_RecentlyAllocatedSeats_Should_Return_List_Of_Recently_Allocated_Seats()
    {
        cinemasController.AddTheatre(rowCount, columnCount, theatreInfo);
        cinemasController.AddTheatre(4, 6, "some info");

        cinemasController.AllocateSeatsOnSelectedTheatre(3);

        cinemasController.RecentlyAllocatedSeats.Count.Should().Be(3);
        cinemasController.RecentlyAllocatedSeats[0].SeatNumber.Should().Be("A1");
        cinemasController.RecentlyAllocatedSeats[1].SeatNumber.Should().Be("A2");
        cinemasController.RecentlyAllocatedSeats[2].SeatNumber.Should().Be("A3");
        cinemasController.RecentlyAllocatedSeats[0].Status.Should().Be(SeatStatus.Allocated);
        cinemasController.RecentlyAllocatedSeats[1].Status.Should().Be(SeatStatus.Allocated);
        cinemasController.RecentlyAllocatedSeats[2].Status.Should().Be(SeatStatus.Allocated);

        cinemasController.AllocateSeatsOnSelectedTheatre(2);

        cinemasController.RecentlyAllocatedSeats.Count.Should().Be(2);
        cinemasController.RecentlyAllocatedSeats[0].SeatNumber.Should().Be("A4");
        cinemasController.RecentlyAllocatedSeats[1].SeatNumber.Should().Be("A5");
        cinemasController.RecentlyAllocatedSeats[0].Status.Should().Be(SeatStatus.Allocated);
        cinemasController.RecentlyAllocatedSeats[1].Status.Should().Be(SeatStatus.Allocated);
    }

    [Test]
    public void AllocateSeatsOnSelectedTheatre_On_Theatre_With_Not_Enough_Seats_Should_Throw_Exception()
    {
        cinemasController.AddTheatre(rowCount, columnCount, theatreInfo);
        cinemasController.AddTheatre(1, 2, "some info");

        Action act = () => cinemasController.AllocateSeatsOnSelectedTheatre(3);
        act.Should().Throw<InvalidOperationException>();
    }

    [Test]
    public void DeleteTheatre_With_Null_Input_Theatre_Should_Throw_Exception()
    {
        Action act;

        act = () => cinemasController.DeleteTheatre(null);
        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void DeleteTheatre_With_Input_Theatre_Not_In_Theatres_List_Should_Throw_Exception()
    {
        cinemasController.AddTheatre(rowCount, columnCount, theatreInfo);
        Theatre theatre = new(3, 3, "some info", seatNumberGenerator);

        Action act;

        act = () => cinemasController.DeleteTheatre(theatre);
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void DeleteTheatre_With_Input_Theatre_In_Theatres_List_Then_Theatres_Should_Not_Include_Deleted_Theatre()
    {
        cinemasController.AddTheatre(rowCount, columnCount, theatreInfo);
        cinemasController.AddTheatre(4, 6, "some info");

        Theatre theatre = cinemasController.Theatres[0];
        cinemasController.DeleteTheatre(theatre);

        cinemasController.Theatres.Should().NotContain(theatre);
    }

    [Test]
    public void DeleteTheatre_With_Input_Theatre_Same_As_SelectedTheatre_Then_SelectedTheatre_Should_Be_Null()
    {
        cinemasController.AddTheatre(rowCount, columnCount, theatreInfo);
        cinemasController.AddTheatre(4, 6, "some info");

        Theatre theatre = cinemasController.Theatres[1];
        cinemasController.DeleteTheatre(theatre);

        cinemasController.SelectedTheatre.Should().Be(null);
    }

    [Test]
    public void DeleteTheatre_With_Input_Theatre_Same_As_SelectedTheatre_Then_RecentlyAllocatedSeats_Should_Return_Empty_List_Of_Seats()
    {
        cinemasController.AddTheatre(rowCount, columnCount, theatreInfo);
        cinemasController.AddTheatre(4, 6, "some info");

        cinemasController.AllocateSeatsOnSelectedTheatre(3);

        Theatre theatre = cinemasController.Theatres[1];
        cinemasController.DeleteTheatre(theatre);

        ReadOnlyCollection<Seat> recentlyAllocatedSeats = cinemasController.RecentlyAllocatedSeats;
        recentlyAllocatedSeats.Count.Should().Be(0);
        recentlyAllocatedSeats.Should().Equal(new List<Seat>() { }.AsReadOnly());
    }
}
