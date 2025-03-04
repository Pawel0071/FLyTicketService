namespace FLyTicketService.DTO
{
    public class FlightScheduleDTO
    {
        public Guid FlightScheduleId { get; set; }
        public required string AirlineIATA { get; set; }
        public string? Number { get; set; }
        public string? NumberSuffix { get; set; }
        public required string AircraftRegistrationNumber { get; set; }
        public required string FlightType { get; set; }
        public required DateTime Departure { get; set; }
        public required DateTime Arrival { get; set; }
        public required string OriginIATA { get; set; }
        public required string DestinationIATA { get; set; }
        public required decimal Price { get; set; }
        public int DaysOfWeek { get; set; }
        public int Occurrence { get; set; }
    }
}
