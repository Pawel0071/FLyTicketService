using System.ComponentModel.DataAnnotations;

namespace FLyTicketService.Model
{
    public class AirPort
    {
        [Key]
        public Guid AirPortId { get; set; }

        [Required]
        [MaxLength(100)]
        public string AirPortName { get; set; }

        [Required]
        [MaxLength(100)]
        public string City { get; set; }

        [Required]
        [MaxLength(100)]
        public string Country { get; set; }

        [Required]
        [MaxLength(3)]
        public string IATA { get; set; }

        [Required]
        [MaxLength(4)]
        public string ICAO { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [Required]
        public double Altitude { get; set; }

        [Required]
        public double Timezone { get; set; }

        [Required]
        [MaxLength(1)]
        public string DST { get; set; }

        [Required]
        [MaxLength(100)]
        public string TzDatabaseTimeZone { get; set; }

        [Required]
        [MaxLength(50)]
        public string Type { get; set; }

        [Required]
        [MaxLength(100)]
        public string Source { get; set; }
    }
}
