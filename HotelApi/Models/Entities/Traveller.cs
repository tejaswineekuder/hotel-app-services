using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HotelApi.Models.Entities;

/// <summary>
/// Represents a traveller in the hotel.
/// This entity includes properties for the traveller's unique identifier, first name, last name,
/// date of birth, and the travel group to which the traveller belongs.
/// It also includes a collection of room reservations associated with the traveller.
/// </summary>
/// <remarks>
/// The traveller's first name and last name are required.
/// The date of birth must be provided, and the travel group ID is required.
/// </remarks>
public class Traveller
{
    [Key]
    public Guid ID { get; set; }

    [Required]
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }

    [Required]
    public required string TravelGroupId { get; set; }
    public required TravelGroup TravelGroup { get; set; }

    public ICollection<RoomReservation>? RoomReservations { get; set; }
}