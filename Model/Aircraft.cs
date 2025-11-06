namespace FLyTicketService.Model
{
    public class Aircraft
    {
        #region Properties

        public Guid AircraftId { get; init; } = Guid.NewGuid();
        public required string Model { get; set; }
        public required string RegistrationNumber { get; set; }
        public ICollection<AircraftSeat> Seats { get; set; } = new List<AircraftSeat>();

        public int TotalSeats
        {
            get
            {
                return Seats.Count(x => !x.OutOfService);
            }
        }

        #endregion
    }
}