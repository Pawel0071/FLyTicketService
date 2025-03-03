using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLyTicketService.Model
{
    public class AircraftSeat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AircraftSeatId { get; set; } = Guid.NewGuid();

        [ForeignKey(nameof(Aircraft))]
        public Guid AircraftId { get; set; }
        public Aircraft Aircraft { get; set; }

        [Required]
        [MaxLength(10)]
        public required string SeatNumber { get; set; }

        [Required]
        public required SeatClass Class { get; set; }

        [Required]
        public required bool OutOfService { get; set; } = false;
    }
}
