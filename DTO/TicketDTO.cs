using FLyTicketService.Model.Enums;

namespace FLyTicketService.DTO
{
    public class TicketDTO
    {
        public Guid TicketId { get; set; }
        public required string TicketNumber { get; set; }
        public required string FlightId { get; set; }
        public required string SeatNumber { get; set; }
        public required Guid TenantId { get; set; }
        public required string Tenant { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public TicketStatus Status { get; set; }
        public DateTime? ReleaseDate { get; set; }
    }
}
