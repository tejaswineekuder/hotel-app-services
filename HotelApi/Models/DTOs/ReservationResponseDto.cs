namespace HotelApi.Models.DTOs
{
    /// <summary>
    /// Data Transfer Object for room response.
    /// This DTO is used to encapsulate the necessary information about a room, including its code, type, and availability.
    /// </summary>
    public class ReservationResponseDto
    {
        public string RoomCode { get; set; }
        public string ReservationDate { get; set; }
        public int BedCountReserved { get; set; }
        public int BedCountAvailable { get; set; }
        public bool IsAvailable { get => BedCountAvailable > 0; }
        public string TravellerName { get; set; } = string.Empty;
        public string TravelGroupId { get; set; } = string.Empty;

        public ReservationResponseDto(string roomCode, string reservationDate, int bedCountReserved, int bedCountAvailable, string travellerName, string travelGroupId)
        {
            RoomCode = roomCode;
            ReservationDate = reservationDate;
            BedCountReserved = bedCountReserved;
            BedCountAvailable = bedCountAvailable;
            TravellerName = travellerName;
            TravelGroupId = travelGroupId;
        }

    }
}