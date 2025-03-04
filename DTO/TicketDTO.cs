using FLyTicketService.Model.Enums;

namespace FLyTicketService.DTO
{
    public class TicketDTO
    {
        public required decimal FlightNumber { get; set; }
        public required string SeatNumber { get; set; }
        public Guid TenantId { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public TicketStatus Status { get; set; }
        public DateTime? ReleaseDate { get; set; }
    }
}
