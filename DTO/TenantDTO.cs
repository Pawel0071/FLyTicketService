namespace FLyTicketService.DTO
{
    public class TenantDTO
    {
        public Guid TenantId { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required DateTime BirthDate { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}
