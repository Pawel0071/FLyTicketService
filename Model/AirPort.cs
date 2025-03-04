using FLyTicketService.Model.Enums;

namespace FLyTicketService.Model
{
    public class Airport
    {
        #region Properties

        public Guid AirportId { get; set; } = Guid.NewGuid();
        public string AirportName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string IATA { get; set; }
        public string ICAO { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
        public SimplyTimeZone Timezone { get; set; }
        public bool DaylightSavingTime { get; set; }

        #endregion
    }
}