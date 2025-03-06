using FLyTicketService.Model.Enums;

namespace FLyTicketService.DTO
{
    public class FlightSeatDTO
    {
        public Guid FlightSeatId { get; set; }
        public string SeatNumber { get; set; }
        public Guid FlightScheduleId { get; set; }
        public SeatClass Class { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime? Locked { get; set; }
        public Guid? TicketId { get; set; }
    }
}
