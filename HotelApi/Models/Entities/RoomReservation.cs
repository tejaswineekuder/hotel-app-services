using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HotelApi.Models.Entities;
/// <summary>
/// Represents a room reservation in the hotel.
/// This entity includes properties for the reservation's unique identifier, the traveller associated with the reservation,
/// the room being reserved, and the reservation date.
/// Each reservation is linked to a traveller, so multiple reservations on a room correspond to different traveller of the same group.
/// It also includes a property to indicate whether the traveller has checked in.
/// A reservation is considered checked in if the `CheckIn` property is set to true.
/// If the `CheckIn` property is false, it indicates that the reservation is still pending
/// </summary>
/// <remarks>
/// The reservation date is used as the "start date" for the reservation.
/// </remarks>
[Table("RoomReservations")]
public class RoomReservation
{
	[Key]
	public Guid ID { get; set; }

	[Required]
	public Guid TravellerId { get; set; }
	public required Traveller Traveller { get; set; }

	[Required]
	public int RoomID { get; set; }
	public required Room Room { get; set; }

	[Required]
	public DateTime ReservationDate { get; set; } // Use as "start date" for the reservation

	public bool CheckIn { get; set; } = false;
}
