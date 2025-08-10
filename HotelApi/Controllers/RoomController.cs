using Microsoft.AspNetCore.Mvc;
using HotelApi.Models.Entities;
using HotelApi.Interfaces;
using HotelApi.Models.DTOs;

namespace HotelApi.Controllers
{
    /// <summary>
    /// Manages room reservations for travellers.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        /// <summary>
        /// Fetches available rooms for today.
        /// </summary>
        [HttpGet("rooms/today")]
        public async Task<IActionResult> GetRoomsToday()
        {
            return Ok(await _roomService.GetRoomsForToday());
        }

        /// <summary>
        /// Fetches available rooms for a specific travel group.
        /// </summary>
        [HttpGet("rooms/group/{groupId}")]
        public async Task<IActionResult> GetRoomsByGroup(string groupId)
        {
            var rooms = await _roomService.GetRoomsForGroup(groupId);
            return rooms.ToList().Count == 0 ? NotFound() : Ok(rooms);
        }

        /// <summary>
        /// Fetches a room by its code.
        /// </summary>
        [HttpGet("room/{roomCode}")]
        public async Task<IActionResult> GetRoomByCode(string roomCode)
        {
            var room = await _roomService.GetRoomByCode(roomCode);
            return room == null ? NotFound() : Ok(room);
        }

        /// <summary>
        /// Assigns a traveller to a room.
        /// </summary>
        [HttpPost("assign")]
        public async Task<IActionResult> AssignToRoom([FromBody] AssignTravellerDto request)
        {
             if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _roomService.AssignTravellerToRoom(request.TravellerId, request.RoomCode);
            return result ? Ok() : BadRequest("Traveller or Room not found.");
        }
        /// <summary>
        /// Moves a traveller from one room to another.
        /// </summary>
        [HttpPost("move")]
        public async Task<IActionResult> MoveTraveller([FromBody] MoveTravellerDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _roomService.MoveTraveller(request.TravellerId,request.FromRoomCode, request.ToRoomCode);
            return result ? Ok() : BadRequest("Traveller Or Rooms not found..");
        }

        }
}
