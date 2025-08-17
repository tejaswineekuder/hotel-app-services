using System.ComponentModel.DataAnnotations;
using HotelApi.Models.Validators;

namespace HotelApi.Models.DTOs
{
    /// <summary>
    /// Request Data Transfer Object for getting rooms by travel group.
    /// This DTO is used to encapsulate the necessary information for retrieving rooms associated with a specific travel group.
    /// It includes the group's ID, which must not start with a leading zero and can have a maximum of two letters.
    /// </summary>
    public class GetRoomsByGroupRequestDto
    {
        [Required]
        [GroupIdValidationAttribute] // No leading 0, max 2 letters
        public required string GroupId { get; set; }
    }
}