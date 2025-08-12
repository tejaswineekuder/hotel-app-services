using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HotelApi.Models.Entities;

/// <summary>
/// Represents a room in the hotel.
/// This entity includes properties for the room's unique identifier, code, bed count,
/// and the collection of room reservations associated with it.
/// </summary>
/// <remarks>
/// The room code must be 4 digits long, and the bed count must be between 1 and 10.
/// </remarks>
[Table("Rooms")]
public class Room
{
    [Key]
    required
    public int ID
    { get; set; }

    [Required]
    [RegularExpression(@"^\d{4}$", ErrorMessage = "Room code must be 4 digits.")]
    public required string RoomCode
    { get; set; }

    [Range(1, 10)]
    public int BedCount { get; set; }

    public bool IsReserved
    {
        get
        {
            return RoomReservations?.Any(rr => rr.CheckIn == false) ?? false;
        }
    }

    public ICollection<RoomReservation>? RoomReservations { get; set; }
}