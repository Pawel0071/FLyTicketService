namespace FLyTicketService.Model
{
    public class Airline
    {
        #region Properties

        public Guid AirlineId { get; set; } = Guid.NewGuid();
        public required string Country { get; set; }
        public required string IATA { get; set; }
        public required string AirlineName { get; set; }

        #endregion
    }
}