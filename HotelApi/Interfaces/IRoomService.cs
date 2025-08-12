using System.Collections.Generic;
using HotelApi.Models.DTOs;
using HotelApi.Models.Entities;
namespace HotelApi.Interfaces;
public interface IRoomService
{
    /// <summary>
    /// Fetches available rooms for today.
    /// </summary>
    /// <returns>A list of available rooms for today.</returns>
    Task<IEnumerable<ReservationResponseDto>> GetRoomsForToday();

    /// <summary>
    /// Fetches available rooms for a specific travel group.
    /// </summary>
    /// <param name="groupId">The ID of the travel group.</param>
    /// <returns>A list of available rooms for the specified group.</returns>
    Task<IEnumerable<ReservationResponseDto>> GetRoomsForGroup(string groupId);

    /// <summary>
    /// Fetches a room by its code.
    /// </summary>
    /// <param name="roomCode">The code of the room to fetch.</param>
    /// <returns>The room entity if found, otherwise null.</returns>
    Task<Room> GetRoomByCode(string roomCode);

    /// <summary>
    /// Assigns a traveller to a room.
    /// </summary>
    /// <param name="travellerId">The ID of the traveller to assign.</param>
    /// <param name="roomCode">The code of the room to assign the traveller to.</param>
    /// <returns>True if the assignment was successful, otherwise false.</returns>
    Task<AssignTravellerResponseDto> AssignTravellerToRoom(Guid travellerId, string roomCode);

    /// <summary>
    /// Moves a traveller from one room to another.
    /// </summary>
    /// <param name="travellerId">The ID of the traveller to move.</param>
    /// <param name="fromRoomCode">The code of the room the traveller is currently in.</param>
    /// <param name="toRoomCode">The code of the room to move the traveller to.</param>
    /// <returns>True if the move was successful, otherwise false.</returns>
    Task<MoveTravellerResponseDto> MoveTraveller(Guid travellerId, string fromRoomCode, string toRoomCode);
}