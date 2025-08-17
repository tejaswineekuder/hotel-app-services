using System.ComponentModel.DataAnnotations;

namespace HotelApi.Models.DTOs
{
  /// <summary>
 /// Request Data Transfer Object for getting rooms by room code.
 /// This DTO is used to encapsulate the necessary information for retrieving rooms associated with a specific room code.
 /// It includes the room code, which must not start with a leading zero and can have
 /// </summary>
    public class GetRoomsByRoomCodeRequestDto
    {
        [Required]
        [MinLength(4, ErrorMessage = "Room code must be 4 digits.")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Room code must be 4 digits.")]
        public required string RoomCode { get; set; }
    }
}