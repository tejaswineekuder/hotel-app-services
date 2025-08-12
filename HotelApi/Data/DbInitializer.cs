using HotelApi.Models.Entities;
using HotelApi.Data;
public static class DbInitializer
{
    public static void Seed(HotelDbContext context)
    {

        //Always update the databse to have reservation for today
            var reservations = context.RoomReservations.ToList();
            foreach (var res in reservations)
            {
                res.ReservationDate = DateTime.Today;
            }
            context.SaveChanges(); 

        if (context.Rooms.Any())
        {
            // DB has been seeded already
            return;
        }

        // Seed Rooms
        var rooms = new Room[]
        {
            new Room { ID = 1, RoomCode = "0101", BedCount = 2 },
            new Room { ID = 2, RoomCode = "0102", BedCount = 3 },
            new Room { ID = 3, RoomCode = "0201", BedCount = 2 },
            new Room { ID = 4, RoomCode = "0202", BedCount = 1 }
        };
        context.Rooms.AddRange(rooms);
        context.SaveChanges();

        // Seed Travel Groups
        var travelGroups = new TravelGroup[]
        {
            new TravelGroup
            {
                ID = "1AB123",
                ArrivalDate = DateTime.Today
            },
            new TravelGroup
            {
                ID = "2XY456",
                ArrivalDate = DateTime.Today.AddDays(1)
            }
        };
        context.TravelGroups.AddRange(travelGroups);
        context.SaveChanges();

        // Seed Travellers
        var travellers = new Traveller[]
        {
            new Traveller { FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 5, 1), TravelGroupId = travelGroups[0].ID , TravelGroup = travelGroups[0]},
            new Traveller { FirstName = "Jane", LastName = "Smith", DateOfBirth = new DateTime(1988, 10, 15), TravelGroupId = travelGroups[0].ID, TravelGroup = travelGroups[0]},
            new Traveller { FirstName = "Sarah", LastName = "Connor", DateOfBirth = new DateTime(1992, 9, 20), TravelGroupId = travelGroups[0].ID, TravelGroup = travelGroups[0]},
            new Traveller { FirstName = "Alice", LastName = "Brown", DateOfBirth = new DateTime(1995, 3, 20), TravelGroupId = travelGroups[1].ID, TravelGroup = travelGroups[1]}
        };
        context.Travellers.AddRange(travellers);
        context.SaveChanges();

        // Seed Room Assignments
        var assignments = new RoomReservation[]
        {
            new RoomReservation { RoomID = rooms[0].ID, TravellerId = travellers[0].ID, ReservationDate = DateTime.Today, Room = rooms[0], Traveller = travellers[0] },
            new RoomReservation { RoomID = rooms[0].ID, TravellerId = travellers[1].ID, ReservationDate = DateTime.Today, Room = rooms[0], Traveller = travellers[1] },
            new RoomReservation { RoomID = rooms[1].ID, TravellerId = travellers[2].ID, ReservationDate = DateTime.Today, Room = rooms[1], Traveller = travellers[2] },
            new RoomReservation { RoomID = rooms[2].ID, TravellerId = travellers[3].ID, ReservationDate = DateTime.Today, Room = rooms[2], Traveller = travellers[3]}
        };
        context.RoomReservations.AddRange(assignments);
        context.SaveChanges();
    }
}
