using FLyTicketService.DTO;
using FLyTicketService.Infrastructure;

namespace FLyTicketService.Service.Interfaces
{
    public interface ITenantService
    {
        public Task<OperationResult<bool>> AddTenantAsync(TenantDTO? tenant);

        public Task<OperationResult<bool>> UpdateTenantAsync(Guid tenantId, TenantDTO? tenant);

        public Task<OperationResult<bool>> DeleteTenantAsync(Guid tenantId);

        public Task<OperationResult<TenantDTO>> GetTenantAsync(Guid tenantId);

        public Task<OperationResult<IEnumerable<TenantDTO>>> GetTenantsAsync();
    }
}
