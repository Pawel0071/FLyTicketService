using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLyTicketService.Model
{
    public class Ticket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TicketId { get; set; }

        [ForeignKey("FlightSeatId")]
        public required FlightSeat FlightSeat { get; set; }

        [ForeignKey("TenantId")]
        public required Tenant Tenant { get; set; }

        public decimal Price { get; set; }
    }
}
