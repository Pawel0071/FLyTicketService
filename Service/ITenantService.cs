using FLyTicketService.Infrastructure;
using FLyTicketService.Model;

namespace FLyTicketService.Service
{
    public interface ITenantService
    {
        public Task<OperationResult> AddTenantAsync(Tenant tenant);

        public Task<OperationResult> UpdateTenantAsync(Tenant tenant);

        public Task<OperationResult> DeleteTenantAsync(Guid tenantId);

        public Task<Tenant> GetTenantAsync(Guid tenantId);

        public Task<IEnumerable<Tenant>> GetTenantsAsync();
    }
}
