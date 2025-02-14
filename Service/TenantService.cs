using FLyTicketService.Model;

namespace FLyTicketService.Service
{
    public class TenantService : ITenantService
    {
        public async Task<OperationResult> AddTenantAsync(Tenant tenant)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult> DeleteTenantAsync(Guid tenantId)
        {
            throw new NotImplementedException();
        }

        public async Task<Tenant> GetTenantAsync(Guid tenantId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Tenant>> GetTenantsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult> UpdateTenantAsync(Tenant tenant)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ValidateTenantAsync(Tenant? tenant)
        {
            if (tenant == null)
            {
                return false;
            }

            return true;
        }
    }
}
