using FLyTicketService.Model.Enums;

namespace FLyTicketService.Model
{
    public class AircraftSeat
    {
        #region Properties

        public Guid AircraftSeatId { get; set; } = Guid.NewGuid();
        public Guid AircraftId { get; set; }
        public Aircraft Aircraft { get; set; }
        public required string SeatNumber { get; set; }
        public required SeatClass Class { get; set; }
        public required bool OutOfService { get; set; } = false;

        #endregion
    }
}