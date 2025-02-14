using System.ComponentModel.DataAnnotations;

namespace FLyTicketService.Model
{
    public class FlightSeat
    {
        [Key] 
        public Guid FlightSeatId { get; set; }

        [Required] 
        [MaxLength(10)] 
        public required string SeatNumber { get; set; }

        [Required] 
        public SeatClass Class { get; set; }

        [Required] 
        public bool IsAvailable { get; set; }
    }
}
