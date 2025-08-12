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
        private readonly ILogger<RoomController> _logger;

        public RoomController(IRoomService roomService, ILogger<RoomController> logger)
        {
            _roomService = roomService;
            _logger = logger;
            _logger.LogInformation("RoomController initialized.");
            _logger.LogDebug("RoomController initialized with service: {ServiceType}", roomService.GetType().Name);
        }

        /// <summary>
        /// Fetches available rooms for today.
        /// </summary>
        /// <remarks>
        /// This endpoint retrieves rooms that are available for reservation today.
        /// It checks the room reservations and returns only those rooms that have not been checked in. 
        /// Returns a list of rooms with their details including room code, reservation date, bed count reserved, and available beds.
        /// </remarks>
        /// <returns>A list of available rooms for today.</returns>
        /// <response code="200">Returns the list of available rooms.</response>
        /// <response code="404">If no rooms are available for today.</response>
        [HttpGet("rooms/today")]
        public async Task<IActionResult> GetRoomsToday()
        {
            _logger.LogInformation("Fetching available rooms for today.");
            return Ok(await _roomService.GetRoomsForToday());
        }

        /// <summary>
        /// Fetches available rooms for a specific travel group.
        /// </summary>
        /// <remarks>
        /// This endpoint retrieves rooms that are assigned to a specific travel group.
        /// It checks the room reservations and returns only those rooms that are associated with the specified travel group ID.
        /// Returns a list of rooms with their details including room code, reservation date, bed count reserved, and available beds.
        /// </remarks>
        /// <param name="groupId">The ID of the travel group.</param>
        /// <returns>A list of rooms assigned to the specified travel group.</returns>
        /// <response code="200">Returns the list of rooms for the specified travel group.</response>
        /// <response code="404">If no rooms are found for the specified travel group.</response>
        [HttpGet("rooms/group/{groupId}")]
        public async Task<IActionResult> GetRoomsByGroup(string groupId)
        {
            _logger.LogInformation("Fetching rooms for travel group: {GroupId}", groupId);
            var rooms = await _roomService.GetRoomsForGroup(groupId);
            if (rooms == null || !rooms.Any())
            {
                _logger.LogWarning("No rooms found for travel group: {GroupId}", groupId);
                return NotFound();
            }
            _logger.LogInformation("Found {RoomCount} rooms for travel group: {GroupId}", rooms.Count(), groupId);
            return Ok(rooms);
        }

        /// <summary>
        /// Fetches a room by its code.
        /// </summary>
        /// <remarks>
        /// This endpoint retrieves a room based on its unique code.
        /// It checks the room reservations and returns the reservation details if found.
        /// </remarks>
        /// <param name="roomCode">The unique code of the room.</param>
        /// <returns>The room details if found.</returns>
        /// <response code="200">Returns the room details.</response>
        /// <response code="404">If the room with the specified code is not found.</response>
        [HttpGet("room/{roomCode}")]
        public async Task<IActionResult> GetRoomByCode(string roomCode)
        {
            _logger.LogInformation("Fetching room details for room code: {RoomCode}", roomCode);
            var room = await _roomService.GetRoomByCode(roomCode);
            if (room == null)
            {
                _logger.LogWarning("Room with code {RoomCode} not found.", roomCode);
                return NotFound();
            }
            _logger.LogInformation("Room with code {RoomCode} found.", roomCode);
            return Ok(room);
        }

        /// <summary>
        /// Assigns a traveller to a room.
        /// </summary>
        /// <remarks>
        /// This endpoint assigns a traveller to a room based on the traveller's ID and the room code.
        /// It checks if the room is available and if the traveller exists before making the assignment.
        /// </remarks>
        /// <param name="request">The request containing the traveller ID and room code.</param>
        /// <returns>Returns a success message if the assignment is successful.</returns>
        /// <response code="200">Returns a success message if the assignment is successful.</response>
        /// <response code="400">If the traveller or room is not found, or if the room is full.</response>
        [HttpPost("assign")]
        public async Task<IActionResult> AssignToRoom([FromBody] AssignTravellerRequestDto request)
        {
            _logger.LogInformation("Assigning traveller {TravellerId} to room {RoomCode}", request.TravellerId, request.RoomCode);
             if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _roomService.AssignTravellerToRoom(request.TravellerId, request.RoomCode);
            if (string.IsNullOrEmpty(result.AssignedRoomCode))
            {
                _logger.LogWarning("Failed to assign traveller {TravellerId} to room {RoomCode}: {StatusMessage}", request.TravellerId, request.RoomCode, result.StatusMessage);
                return BadRequest(result.StatusMessage);
            }
            return Ok(result);
        }
        /// <summary>
        /// Moves a traveller from one room to another.
        /// </summary>
        /// <remarks>
        /// This endpoint moves a traveller from one room to another based on the traveller's ID, the current room code, and the new room code.
        /// It checks if both rooms exist and if the traveller is currently assigned to the old room before making the move.
        /// </remarks>
        /// <param name="request">The request containing the traveller ID, current room code, and new room code.</param>
        /// <returns>Returns a success message if the move is successful.</returns>
        /// <response code="200">Returns a success message if the move is successful.</response>
        /// <response code="400">If the traveller or rooms are not found, or if the move is not possible.</response>
        [HttpPost("move")]
        public async Task<IActionResult> MoveTraveller([FromBody] MoveTravellerRequestDto request)
        {
            _logger.LogInformation("Moving traveller {TravellerId} from room {FromRoomCode} to room {ToRoomCode}", request.TravellerId, request.FromRoomCode, request.ToRoomCode);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _roomService.MoveTraveller(request.TravellerId,request.FromRoomCode, request.ToRoomCode);
            if (string.IsNullOrEmpty(result.AssignedRoomCode)) 
            {
                _logger.LogWarning("Failed to move traveller {TravellerId} from room {FromRoomCode} to room {ToRoomCode}", request.TravellerId, request.FromRoomCode, request.ToRoomCode);
                return BadRequest(result.StatusMessage);
            }
            return Ok(result);
        }

        }
}
