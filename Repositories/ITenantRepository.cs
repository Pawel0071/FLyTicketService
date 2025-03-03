using FLyTicketService.Model;

namespace FLyTicketService.Repositories
{
    public interface ITenantRepository 
    {
        Task<bool> AddAsync(Tenant tenant);
        Task<Tenant?> GetByIdAsync(Guid tenantId);
        Task<IEnumerable<Tenant>> GetAllAsync();
        Task<bool> RemoveAsync(Tenant tenant);
        Task<bool> Update(Tenant tenant);
    }
}
