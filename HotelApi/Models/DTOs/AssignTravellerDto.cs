using System.ComponentModel.DataAnnotations;

namespace HotelApi.Models.DTOs
{
    /// <summary>
    /// Data Transfer Object for assigning a traveller to a room.
    /// This DTO is used to encapsulate the necessary information for assigning a traveller to a specific room.
    /// It includes the traveller's ID and the room code to which the traveller is being
    /// </summary>
    /// <remarks>
    /// The room code must be 4 digits long, and the DTO ensures that the required
    /// properties are provided.
    /// </remarks>
    public class AssignTravellerDto
    {
        [Required]
        public required Guid TravellerId { get; set; }

        [Required]
        [MinLength(4, ErrorMessage = "Room code must be 4 digits.")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Room code can only have numbers.")]
        public required string RoomCode { get; set; }
    }
}