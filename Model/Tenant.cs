namespace FLyTicketService.Model
{
    public class Tenant
    {
        #region Properties

        public Guid TenantId { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }

        #endregion
    }
}