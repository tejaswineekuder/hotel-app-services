namespace HotelApi.Models.DTOs
{
    /// <summary>
    /// Response Data Transfer Object for assigning a rooom to traveller.
    /// This DTO is used to encapsulate the necessary information after a traveller has been assigned to a room.
    /// It includes the traveller's ID, the assigned room code, and a status message indicating the room they were assigned to.
    /// </summary>
    public class AssignTravellerResponseDto
    {
        public  Guid? TravellerId { get; set; }
        public  string? AssignedRoomCode { get; set; }
        public  string? StatusMessage { get; set; }
    }
}