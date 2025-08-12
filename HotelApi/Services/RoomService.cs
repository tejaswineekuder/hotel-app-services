namespace HotelApi.Services;
using HotelApi.Interfaces;
using HotelApi.Models.Entities;
using Microsoft.EntityFrameworkCore;
using HotelApi.Data;
using HotelApi.Models.DTOs;

public class RoomService : IRoomService
{
    private readonly HotelDbContext _context;

    public RoomService(HotelDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ReservationResponseDto>>GetRoomsForToday()
    {
        // Logic to get rooms available for today
        return await _context.Rooms
            .Include(r => r.RoomReservations)
            .Where(r => r.RoomReservations.Any(rr => rr.ReservationDate.Date == DateTime.Today))
            .Select(r => new ReservationResponseDto(
                r.RoomCode,
                DateTime.Today.ToString("yyyy-MM-dd"),
                r.RoomReservations.Count(rr => rr.ReservationDate.Date == DateTime.Today),
                r.BedCount - r.RoomReservations.Count(rr => rr.ReservationDate.Date == DateTime.Today),
                string.Join(", ", r.RoomReservations.Select(rr => rr.Traveller.FirstName)),
                string.Join(", ", r.RoomReservations.Select(rr => rr.Traveller.TravelGroupId))
            ))
            .ToListAsync();
    }

    public  async Task<IEnumerable<ReservationResponseDto>>GetRoomsForGroup(string groupId)
    {
        // Logic to get rooms assigned to a specific travel group
        return await _context.Rooms
            .Include(r => r.RoomReservations)
            .Where(r => r.RoomReservations.Any(rr => rr.Traveller.TravelGroupId == groupId))
            .Select(r=> new ReservationResponseDto(
                r.RoomCode,
                DateTime.Today.ToString("yyyy-MM-dd"),
                r.RoomReservations.Count(rr => rr.Traveller.TravelGroupId == groupId),
                r.BedCount - r.RoomReservations.Count(rr => rr.Traveller.TravelGroupId == groupId),
                string.Join(", ", r.RoomReservations.Select(rr => rr.Traveller.FirstName)),
                string.Join(", ", r.RoomReservations.Select(rr => rr.Traveller.TravelGroupId))
            ))
            .ToListAsync();
    }

    public async Task<Room> GetRoomByCode(string roomCode)
    {
        // Logic to get a room by its code
        return await _context.Rooms
        .Include(r=>r.RoomReservations).FirstOrDefaultAsync(r => r.RoomCode == roomCode);
    }

    public async Task<AssignTravellerResponseDto> AssignTravellerToRoom(Guid travellerId, string roomCode)
    {
        // Logic to assign a traveller to a room
        var room = await GetRoomByCode(roomCode);
        var traveller = await _context.Travellers.FindAsync(travellerId);
        if (traveller == null || room == null)
        {
            return new AssignTravellerResponseDto { StatusMessage = "Traveller or rooms not found"}; 
        }
        // Check if the room is full
        if (room.RoomReservations?.Count >= room.BedCount)
        {
             return new AssignTravellerResponseDto { StatusMessage = "Room is full"};  
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
        return new AssignTravellerResponseDto
        {
            TravellerId = traveller.ID,
            AssignedRoomCode = room.RoomCode,
            StatusMessage = $"Traveller {traveller.FirstName} assigned to room {room.RoomCode}"
        }; 
    }

    public async Task<MoveTravellerResponseDto> MoveTraveller(Guid travellerId, string fromRoomCode, string toRoomCode)
    {
        // Logic to move a traveller from one room to another
        var fromRoom = await GetRoomByCode(fromRoomCode);
        var toRoom = await GetRoomByCode(toRoomCode);
        var traveller = await _context.Travellers.FindAsync(travellerId);
        if (traveller == null || fromRoom == null || toRoom == null)
        {
            return new MoveTravellerResponseDto { StatusMessage = "Traveller or rooms not found"}; 
        }
        // Check if the destination room is full
        if (toRoom.RoomReservations?.Count >= toRoom.BedCount)
        {
            return new MoveTravellerResponseDto { StatusMessage = "Destination room is full"};   
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
        return new MoveTravellerResponseDto
        {
            TravellerId = traveller.ID,
            OldRoomCode = fromRoom.RoomCode,
            AssignedRoomCode = toRoom.RoomCode,
            StatusMessage = $"Traveller {traveller.FirstName} is moved from {fromRoom.RoomCode} to room {toRoom.RoomCode}"
        }; 
    }
}