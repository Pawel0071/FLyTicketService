using FLyTicketService.Model.Enums;

namespace FLyTicketService.Model
{
    public class FlightSeat
    {
        #region Properties

        public Guid FlightSeatId { get; set; }
        public required string SeatNumber { get; set; }
        public Guid FlightScheduleId { get; set; }
        public FlightSchedule FlightSchedule { get; set; }
        public SeatClass Class { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime? Locked { get; set; }
        public Guid? TicketId { get; set; }
        public Ticket? Ticket { get; set; }

        #endregion
    }
}