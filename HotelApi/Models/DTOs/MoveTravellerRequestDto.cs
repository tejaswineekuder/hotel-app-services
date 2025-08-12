using System.ComponentModel.DataAnnotations;

namespace HotelApi.Models.DTOs
{
    /// <summary>
    /// Data Transfer Object for moving a traveller from one room to another.
    /// This DTO is used to encapsulate the necessary information for moving a traveller between rooms.
    /// It includes the traveller's ID, the room code they are currently in, and the room code they are moving to.
    /// </summary>
    /// <remarks>
    /// The room codes must be 4 digits long, and the DTO ensures that the required properties are provided.
    /// </remarks>
    public class MoveTravellerRequestDto
    {
        [Required]
        public required Guid TravellerId { get; set; }
        [Required]
        [MinLength(4, ErrorMessage = "Room code must be 4 digits.")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Room code can only have numbers.")]
        public required string FromRoomCode { get; set; }
        [Required]
        [MinLength(4, ErrorMessage = "Room code must be 4 digits.")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Room code can only have numbers.")]
        public required string ToRoomCode { get; set; }
    }
}