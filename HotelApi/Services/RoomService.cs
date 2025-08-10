namespace HotelApi.Services;
using HotelApi.Interfaces;
using HotelApi.Models.Entities;
using Microsoft.EntityFrameworkCore;
using HotelApi.Data;

public class RoomService : IRoomService
{
    private readonly HotelDbContext _context;

    public RoomService(HotelDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Room>> GetRoomsForToday()
    {
        // Logic to get rooms available for today
        return await _context.Rooms
            .Include(r => r.RoomReservations)
            .Where(r => r.RoomReservations.Any(rr => rr.ReservationDate.Date == DateTime.Today))
            .ToListAsync();
    }

    public  async Task<IEnumerable<Room>>GetRoomsForGroup(string groupId)
    {
        // Logic to get rooms assigned to a specific travel group
        return await _context.Rooms
            .Include(r => r.RoomReservations)
            .Where(r => r.RoomReservations.Any(rr => rr.Traveller.TravelGroupId == groupId))
            .ToListAsync();
    }

    public async Task<Room> GetRoomByCode(string roomCode)
    {
        // Logic to get a room by its code
        return await _context.Rooms.FirstOrDefaultAsync(r => r.RoomCode == roomCode); 
         
    }

    public async Task<bool> AssignTravellerToRoom(Guid travellerId, string roomCode)
    {
        // Logic to assign a traveller to a room
        var room = await GetRoomByCode(roomCode);
        var traveller = await _context.Travellers.FindAsync(travellerId);
        if (traveller == null || room == null)
        {
            return false;// Traveller or room not found
        }
        // Check if the room is full
        if (room.RoomReservations?.Count >= room.BedCount)
        {
            return false; // Room is full
        }
        var reservation = new RoomReservation
        {
            RoomID = room.ID,
            TravellerId = traveller.ID,
            ReservationDate = DateTime.Today,
            Room = room,
            Traveller = traveller
        };
        _context.RoomReservations.Add(reservation);
        _context.SaveChanges();
        return true; // Successfully assigned
    }

    public async Task<bool> MoveTraveller(Guid travellerId, string fromRoomCode, string toRoomCode)
    {
        // Logic to move a traveller from one room to another
        var fromRoom = await GetRoomByCode(fromRoomCode);
        var toRoom = await GetRoomByCode(toRoomCode);
        var traveller = await _context.Travellers.FindAsync(travellerId);
        if (traveller == null || fromRoom == null || toRoom == null)
        {
            return false; // Traveller or rooms not found
        }
        // Check if the destination room is full
        if (toRoom.RoomReservations?.Count >= toRoom.BedCount)
        {
            return false; // Destination room is full
        }
        // Remove existing reservation
        var existingReservation = _context.RoomReservations
            .FirstOrDefault(rr => rr.TravellerId == traveller.ID && rr.RoomID == fromRoom.ID);
        if (existingReservation != null)
        {
            _context.RoomReservations.Remove(existingReservation);
        }

        // Create new reservation
        var newReservation = new RoomReservation
        {
            RoomID = toRoom.ID,
            TravellerId = traveller.ID,
            ReservationDate = DateTime.Today,
            Room = toRoom,
            Traveller = traveller
        };
        await _context.RoomReservations.AddAsync(newReservation);
        await _context.SaveChangesAsync();
        return true; // Successfully moved
    }
}