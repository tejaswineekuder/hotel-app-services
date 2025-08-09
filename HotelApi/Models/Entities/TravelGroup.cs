using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HotelApi.Models.Validators;

namespace HotelApi.Models.Entities;
/// <summary>
/// Represents a travel group in the hotel.
/// This entity includes properties for the group's unique identifier, arrival date,
/// and the collection of travellers associated with the group.
/// </summary>
/// <remarks>
/// The group ID must not start with a leading zero and can have a maximum of two letters.
/// </remarks>
public class TravelGroup
{
    [Key]
    [GroupIdValidationAttribute] // No leading 0, max 2 letters
    public required string ID { get; set; }

    [Required]
    public DateTime ArrivalDate { get; set; }

    public ICollection<Traveller> Travellers { get; set; } = new List<Traveller>();
}