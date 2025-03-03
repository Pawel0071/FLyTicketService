using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLyTicketService.Model
{
    public class Reservation

    {
        [Key]
        public Guid ReservationId { get; set; }

        [ForeignKey("FlightSeatId")]
        public Guid FlightSeatId { get; set; }

        [ForeignKey("TenantId")]
        public Guid TenantId { get; set; }

        public decimal Price { get; set; }
    }
}
