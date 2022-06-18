﻿using CinnamonCinemas.Models;
using CinnamonCinemas.Models.SeatNumberGenerators;

SeatNumberGenerator seatNumberGenerator = new SeatNumberGenerator();
Theatre theatre = new Theatre(rowCount: 3, columnCount: 5, seatNumberGenerator);

while (true)
{
    Console.WriteLine($"Available Seats Count: {theatre.GetAvailableSeatsCount()}");
    int numberOfSeats = new Random().Next(1, 4);
    var allocatedSeats = theatre.AllocateSeats(numberOfSeats);
    
    if (allocatedSeats is null)
    {
        Console.WriteLine($"Customer wanted {numberOfSeats} seats, but we don't have enough available seats.");
        break;
    }

    var allocatedSeatNumbers = allocatedSeats.Select(seat => seat.SeatNumber).ToList();
    Console.WriteLine($"Customer wants {numberOfSeats} seats, allocated at {string.Join(", ", allocatedSeatNumbers)}");

}

Console.ReadLine();