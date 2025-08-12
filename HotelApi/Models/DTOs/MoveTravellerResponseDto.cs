namespace HotelApi.Models.DTOs
{
    /// <summary>
    /// Data Transfer Object for assigning and moving a traveller.
    /// This DTO is used to encapsulate the necessary information for assigning or moving a traveller between rooms.
    /// It includes the traveller's ID, the room code they are currently in, and the room code they are moving to.
    /// </summary>
    public class MoveTravellerResponseDto :AssignTravellerResponseDto
    {
        public string? OldRoomCode { get; set; }
    }
}