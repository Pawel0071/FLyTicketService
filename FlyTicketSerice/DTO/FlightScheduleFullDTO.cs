namespace FLyTicketService.DTO
{
    public class FlightScheduleFullDTO
    {
        public Guid FlightScheduleId { get; set; }
        public AirlineDTO Airline { get; set; }
        public AircraftDTO Aircraft { get; set; }
        public ICollection<FlightSeatDTO> Seats { get; set; }
        public string FlightId { get; set; }
        public DateTimeOffset Departure { get; set; }
        public DateTimeOffset Arrival { get; set; }
        public AirportDTO Origin { get; set; }
        public AirportDTO Destination { get; set; }
        public decimal Price { get; set; }

    }
}
