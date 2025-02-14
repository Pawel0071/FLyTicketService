using System.ComponentModel.DataAnnotations;

namespace FLyTicketService.Model
{
    public class AircraftSeat
    {
        [Key]
        public Guid AircraftSeatId { get; set; }

        [Required]
        [MaxLength(10)]
        public required string SeatNumber { get; set; }

        [Required]
        public SeatClass Class { get; set; }

        [Required]
        public bool IsAvailable { get; set; }
    }
}
