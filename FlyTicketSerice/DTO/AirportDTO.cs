using FLyTicketService.Model.Enums;

namespace FLyTicketService.DTO
{
    public class AirportDTO
    {
        public Guid AirportId { get; set; }
        public string AirportName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string IATA { get; set; }
        public string ICAO { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
        public SimplyTimeZone Timezone { get; set; }
        public DaylightSavingTime DST { get; set; }
        public string Continent { get; set; }
    }
}
