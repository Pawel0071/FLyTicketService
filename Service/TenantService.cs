using FLyTicketService.Model;

namespace FLyTicketService.Service
{
    public class TenantService : ITenantService
    {
        public async Task<OperationResult> AddTenantAsync(TenantDTO tenant)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult> DeleteTenantAsync(Guid tenantId)
        {
            throw new NotImplementedException();
        }

        public async Task<TenantDTO> GetTenantAsync(Guid tenantId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TenantDTO>> GetTenantsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult> UpdateTenantAsync(TenantDTO tenant)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ValidateTenantAsync(TenantDTO? tenant)
        {
            if (tenant == null)
            {
                return false;
            }

            return true;
        }
    }
}
