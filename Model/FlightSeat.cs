using FLyTicketService.Model.Enums;

namespace FLyTicketService.Model
{
    public class FlightSeat
    {
        #region Properties

        public Guid FlightSeatId { get; set; }
        public required string SeatNumber { get; set; }
        public Guid FlightsPlanId { get; set; }
        public required FlightsPlan FlightsPlan { get; set; }
        public SeatClass Class { get; set; }
        public bool IsAvailable { get; set; }
        public Ticket? Ticket { get; set; }
        public Reservation? Reservation { get; set; }

        #endregion
    }
}