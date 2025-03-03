using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLyTicketService.Model
{
    public class Airport
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AirportId { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string AirportName { get; set; }

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
        public string Timezone { get; set; }

        [Required]
        [MaxLength(1)]
        public string DST { get; set; }
    }
}
