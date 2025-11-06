namespace FLyTicketService.DTO
{
    public class AirlineDTO
    {
        public Guid AirlineId { get; set; }
        public string Country { get; set; }
        public string IATA { get; set; }
        public string AirlineName { get; set; }
    }
}
