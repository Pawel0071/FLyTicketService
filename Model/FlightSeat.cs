using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLyTicketService.Model
{
    public class FlightSeat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid FlightSeatId { get; set; } 

        [Required]
        [MaxLength(10)]
        public required string SeatNumber { get; set; }

        [ForeignKey("Flight")]
        public Flight Flight { get; set; }

        [Required]
        public SeatClass Class { get; set; }

        [Required]
        public bool IsAvailable { get; set; }

        [ForeignKey("TicketId")]
        public Ticket? Ticket { get; set; }

        [ForeignKey("ReservationId")]
        public Reservation? Reservation { get; set; }
    }
}
