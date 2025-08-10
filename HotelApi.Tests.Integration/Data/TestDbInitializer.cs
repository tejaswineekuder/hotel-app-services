
 using HotelApi.Data;
    using HotelApi.Models.Entities;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;

namespace HotelApi.Tests.Integration.Data;
public static class TestDbInitializer
{
    public static void Initialize(HotelDbContext dbContext, Guid travellerId)
    {
        // Arrange
        var travelGroup1 = new TravelGroup
        {
            ID = "AB1234",
            ArrivalDate = DateTime.Today,
            Travellers = new List<Traveller>()
        };

        var room1 = new Room
        {
            ID = 1,
            RoomCode = "0101",
            BedCount = 1
        };

        var room2 = new Room
        {
            ID = 2,
            RoomCode = "0102",
            BedCount = 2
        };

        // Create Traveller and link to travel group
        var traveller1 = new Traveller
        {
            ID = travellerId,
            FirstName = "John",
            LastName = "Doe",
            TravelGroupId = travelGroup1.ID,
            TravelGroup = travelGroup1,
            DateOfBirth = new DateTime(1990, 5, 1)
        };

        travelGroup1.Travellers.Add(traveller1);

        // Create RoomReservation for today
        var reservation1 = new RoomReservation
        {
            ReservationDate = DateTime.Today,
            RoomID = room1.ID,
            Room = room1,
            TravellerId = travellerId,
            Traveller = traveller1
        };

        room1.RoomReservations = new List<RoomReservation> { reservation1 };

        // Final room list
        var rooms = new List<Room> { room1, room2 };


        // Ensure database is created
        dbContext.Database.EnsureCreated();

        // Add entities to the context
        dbContext.TravelGroups.Add(travelGroup1);
        dbContext.Rooms.AddRange(rooms);
        dbContext.Travellers.Add(traveller1);
        dbContext.RoomReservations.Add(reservation1);

        // Save changes to the database
        dbContext.SaveChanges();

    }
}