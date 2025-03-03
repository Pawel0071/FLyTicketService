using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLyTicketService.Model
{
    public class Airline
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AirlineId { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public required string Country { get; set; }

        [Required]
        [MaxLength(3)]
        public required string IATA { get; set; }

        [Required]
        [MaxLength(200)]
        public required string AirlineName { get; set; }
    }
}
