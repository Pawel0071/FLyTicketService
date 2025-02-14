using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLyTicketService.Model
{
    public class Ticket
    {
        [Key]
        public Guid TicketId { get; set; }

        [ForeignKey("FlightSeat")]
        public Guid FlightSeatId { get; set; }

        [ForeignKey("Tenant")]
        public Guid TenantId { get; set; }

        public decimal Price { get; set; }
    }
}
