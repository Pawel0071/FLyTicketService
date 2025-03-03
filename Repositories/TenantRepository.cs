using FLyTicketService.Data;
using FLyTicketService.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FLyTicketService.Repositories
{
    public class TenantRepository : ITenantRepository
    {
        #region Fields

        private readonly FLyTicketDbContext _context;
        private readonly ILogger<TenantRepository> _logger;

        #endregion

        #region Constructors

        public TenantRepository(FLyTicketDbContext context, ILogger<TenantRepository> logger)
        {
            this._context = context;
            this._logger = logger;
        }

        public async Task<bool> AddAsync(Tenant tenant)
        {
            if (tenant == null)
            {
                throw new ArgumentNullException(nameof(tenant), "Tenant cannot be null.");
            }

            await this._context.Set<Tenant>().AddAsync(tenant);
            return await this.SaveChangesAsync();
        }

        public async Task<IEnumerable<Tenant>> GetAllAsync()
        {
            return await this._context.Set<Tenant>().ToListAsync();
        }

        public async Task<Tenant?> GetByIdAsync(Guid tenantId)
        {
            var tenant = await this._context.Set<Tenant>().FindAsync(tenantId);
            return tenant;
        }

        public async Task<bool> RemoveAsync(Tenant tenant)
        {
            if (tenant == null)
            {
                throw new ArgumentNullException(nameof(tenant), "Tenant cannot be null.");
            }

            try
            {
                this._context.Set<Tenant>().Remove(tenant);
                return await this.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 547)
            {
                _logger.LogError(ex, "Cannot remove tenant due to key violation.");
                throw new InvalidOperationException("Cannot remove tenant due to key violation.", ex);
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await this._context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Update(Tenant tenant)
        {
            if (tenant == null)
            {
                throw new ArgumentNullException(nameof(tenant), "Tenant cannot be null.");
            }

            var existingTenant = await this._context.Set<Tenant>().FindAsync(tenant.TenantId);
            if (existingTenant == null)
            {
                _logger.LogError($"Tenant with ID {tenant.TenantId} was not found.");
                throw new KeyNotFoundException($"Tenant with ID {tenant.TenantId} was not found.");
            }

            existingTenant.Name = tenant.Name;
            existingTenant.Address = tenant.Address;
            existingTenant.Phone = tenant.Phone;
            existingTenant.Email = tenant.Email;

            this._context.Set<Tenant>().Update(existingTenant);
            return await this.SaveChangesAsync();
        }

        #endregion
    }
}