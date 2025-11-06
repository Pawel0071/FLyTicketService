using FLyTicketService.Model.Enums;

namespace FLyTicketService.Model
{
    public class Tenant
    {
        #region Properties

        public Guid TenantId { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required TenantGroup Group { get; set; }
        public required DateTime Birthday { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public IEnumerable<Ticket> Tickets { get; set; }

        #endregion
    }
}