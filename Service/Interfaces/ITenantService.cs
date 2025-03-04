using FLyTicketService.DTO;
using FLyTicketService.Infrastructure;

namespace FLyTicketService.Service.Interfaces
{
    public interface ITenantService
    {
        public Task<OperationResult> AddTenantAsync(TenantDTO? tenant);

        public Task<OperationResult> UpdateTenantAsync(TenantDTO? tenant);

        public Task<OperationResult> DeleteTenantAsync(Guid tenantId);

        public Task<TenantDTO?> GetTenantAsync(Guid tenantId);

        public Task<IEnumerable<TenantDTO>> GetTenantsAsync();
    }
}
