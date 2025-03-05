using FLyTicketService.Model.Enums;

namespace FLyTicketService.Model
{
    public class Ticket
    {
        public Guid TicketId { get; set; }
        public required string TicketNumber { get; set; }
        public required FlightSeat FlightSeat { get; set; }
        public required Tenant Tenant { get; set; }
        public required decimal Price { get; set; }
        public decimal Discount => Discounts.Sum(s => s.Value); 
        public required TicketStatus Status { get; set; }
        public DateTimeOffset? ReleaseDate { get; set; }
        public IEnumerable<Discount> Discounts { get; set; } = new List<Discount>();
    }
}
