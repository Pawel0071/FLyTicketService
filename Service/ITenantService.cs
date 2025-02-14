using FLyTicketService.Model;

namespace FLyTicketService.Service
{
    public interface ITenantService
    {
        public Task<OperationResult> AddTenantAsync(TenantDTO tenant);

        public Task<OperationResult> UpdateTenantAsync(TenantDTO tenant);

        public Task<OperationResult> DeleteTenantAsync(Guid tenantId);

        public Task<TenantDTO> GetTenantAsync(Guid tenantId);

        public Task<IEnumerable<TenantDTO>> GetTenantsAsync();

        public Task<bool> ValidateTenantAsync(TenantDTO? tenant);


    }
}
