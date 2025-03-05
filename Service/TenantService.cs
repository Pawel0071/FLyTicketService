using FLyTicketService.DTO;
using FLyTicketService.Mapper;
using FLyTicketService.Model;
using FLyTicketService.Repositories.Interfaces;
using FLyTicketService.Service.Interfaces;
using FLyTicketService.Shared;

namespace FLyTicketService.Services
{
    public class TenantService : ITenantService
    {
        #region Fields

        private readonly IGenericRepository<Tenant> _tenantRepository;
        private readonly ILogger<TenantService> _logger;

        #endregion

        #region Constructors

        public TenantService(IGenericRepository<Tenant> tenantRepository, ILogger<TenantService> logger)
        {
            _tenantRepository = tenantRepository;
            _logger = logger;
        }

        #endregion

        #region Methods

        public async Task<OperationResult<bool>> AddTenantAsync(TenantDTO? tenant)
        {
            _logger.LogInformation("Adding a new tenant");

            if (tenant == null)
            {
                _logger.LogWarning("Invalid tenant data");
                return new OperationResult<bool>(OperationStatus.BadRequest, "Invalid tenant data", false);
            }

            Tenant newTenant = tenant.ToDomain();

            bool success = await _tenantRepository.AddAsync(newTenant);

            _logger.LogInformation(success ? "Tenant added successfully" : "Failed to add tenant");

            return new OperationResult<bool>(
                success ? OperationStatus.Created : OperationStatus.InternalServerError,
                success ? "Tenant added successfully" : "Failed to add tenant",
                success);
        }

        public async Task<OperationResult<bool>> DeleteTenantAsync(Guid tenantId)
        {
            _logger.LogInformation($"Deleting tenant with ID {tenantId}");

            Tenant? tenant = await _tenantRepository.GetByIdAsync(tenantId);
            if (tenant == null)
            {
                _logger.LogWarning("Tenant not found");
                return new OperationResult<bool>(OperationStatus.NotFound, "Invalid tenant Id", false);
            }

            bool success = await _tenantRepository.RemoveAsync(tenantId);

            _logger.LogInformation(success ? "Tenant deleted successfully" : "Failed to delete tenant");

            return new OperationResult<bool>(
                success ? OperationStatus.Created : OperationStatus.InternalServerError,
                success ? "Tenant deleted successfully" : "Failed to delete tenant",
                success);
        }

        public async Task<OperationResult<TenantDTO>> GetTenantAsync(Guid tenantId)
        {
            _logger.LogInformation($"Getting tenant with ID {tenantId}");

            Tenant? tenant = await _tenantRepository.GetByIdAsync(tenantId);

            if (tenant == null)
            {
                _logger.LogWarning("Tenant not found");
                return new OperationResult<TenantDTO>(OperationStatus.NotFound, "Invalid tenant Id", null);
            }

            TenantDTO tenantDTO = tenant.ToDTO();

            _logger.LogInformation("Tenant retrieved successfully");

            return new OperationResult<TenantDTO>(OperationStatus.Ok, "Tenant retrieved successfully", tenantDTO);
        }

        public async Task<OperationResult<IEnumerable<TenantDTO>>> GetTenantsAsync()
        {
            _logger.LogInformation("Getting all tenants");

            IEnumerable<Tenant> tenants = await _tenantRepository.GetAllAsync();
            List<TenantDTO> tenantDTOs = tenants.Select(x => x.ToDTO()).ToList();

            _logger.LogInformation("Tenants retrieved successfully");

            return new OperationResult<IEnumerable<TenantDTO>>(OperationStatus.Ok, "Tenants retrieved successfully", tenantDTOs);
        }

        public async Task<OperationResult<bool>> UpdateTenantAsync(Guid tenantId, TenantDTO? tenant)
        {
            _logger.LogInformation($"Updating tenant with ID {tenantId}");

            if (tenant == null)
            {
                _logger.LogWarning("Invalid tenant data");
                return new OperationResult<bool>(OperationStatus.BadRequest, "Invalid tenant data", false);
            }

            Tenant? existingTenant = await _tenantRepository.GetByIdAsync(tenantId);

            if (existingTenant == null)
            {
                _logger.LogWarning("Tenant not found");
                return new OperationResult<bool>(OperationStatus.NotFound, "Invalid tenant Id", false);
            }

            existingTenant.Name = tenant.Name;
            existingTenant.Address = tenant.Address;
            existingTenant.Group = tenant.Group;
            existingTenant.BirthDate = tenant.BirthDate;
            existingTenant.Phone = tenant.Phone;
            existingTenant.Email = tenant.Email;

            bool success = await _tenantRepository.UpdateAsync(existingTenant);

            _logger.LogInformation(success ? "Tenant updated successfully" : "Failed to update tenant");

            return new OperationResult<bool>(
                success ? OperationStatus.Ok : OperationStatus.InternalServerError,
                success ? "Tenant updated successfully" : "Failed to update tenant",
                success);
        }

        #endregion
    }
}
