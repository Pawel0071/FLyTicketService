using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLyTicketService.Model
{
    public class Sales
    {
        public Sales(ICollection<Ticket> tickets)
        {
            this.Tickets = tickets;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid SaleId { get; set; }

        [ForeignKey("ReservationId")]
        public Guid? ReservationId { get; set; }

        public ICollection<Ticket> Tickets { get; set; }

        public DateTime SaleDate { get; set; }

        public decimal Amount { get; set; }
    }
}
