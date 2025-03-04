using FLyTicketService.Infrastructure;
using FLyTicketService.Model;
using FLyTicketService.Repositories.Interfaces;
using FLyTicketService.Service.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FLyTicketService.Service
{
    public class TenantService: ITenantService
    {
        #region Fields

        private readonly ITenantRepository _tenantRepository;
        private readonly ILogger<TenantService> _logger;

        #endregion

        #region Constructors

        public TenantService(ITenantRepository tenantRepository, ILogger<TenantService> logger)
        {
            _tenantRepository = tenantRepository;
            _logger = logger;
        }

        #endregion

        #region Methods

        public async Task<OperationResult> AddTenantAsync(Tenant? tenant)
        {
            if (tenant == null)
            {
                _logger.LogError("AddTenantAsync: tenant is null");
                return new OperationResult { Status = OperationStatus.BadRequest, Message = "Tenant is null" };
            }

            if (!ValidateTenant(tenant))
            {
                _logger.LogError("AddTenantAsync: tenant validation failed");
                return new OperationResult { Status = OperationStatus.Conflict, Message = "Tenant validation failed" };
            }

            await _tenantRepository.AddAsync(tenant);
            _logger.LogInformation("AddTenantAsync: tenant added successfully");

            return new OperationResult { Status = OperationStatus.Created, Message = "Tenant added successfully" };
        }

        public async Task<OperationResult> DeleteTenantAsync(Guid tenantId)
        {
            Tenant? tenant = await _tenantRepository.GetByIdAsync(tenantId);
            if (tenant == null)
            {
                _logger.LogWarning($"DeleteTenantAsync: tenant with id {tenantId} not found");
                return new OperationResult { Status = OperationStatus.NotFound, Message = "Tenant not found" };
            }

            await _tenantRepository.RemoveAsync(tenant);
            _logger.LogInformation($"DeleteTenantAsync: tenant with id {tenantId} deleted successfully");

            return new OperationResult { Status = OperationStatus.Ok, Message = "Tenant deleted successfully" };
        }

        public async Task<Tenant?> GetTenantAsync(Guid tenantId)
        {
            Tenant? tenant = await _tenantRepository.GetByIdAsync(tenantId);
            if (tenant == null)
            {
                _logger.LogWarning($"GetTenantAsync: tenant with id {tenantId} not found");
            }

            return tenant;
        }

        public Task<IEnumerable<Tenant>> GetTenantsAsync()
        {
            return _tenantRepository.GetAllAsync();
        }

        public async Task<OperationResult> UpdateTenantAsync(Tenant? tenant)
        {
            if (tenant == null)
            {
                _logger.LogError("UpdateTenantAsync: tenant is null");
                return new OperationResult { Status = OperationStatus.BadRequest, Message = "Tenant is null" };
            }

            if (!ValidateTenant(tenant))
            {
                _logger.LogError("UpdateTenantAsync: tenant validation failed");
                return new OperationResult { Status = OperationStatus.Conflict, Message = "Tenant validation failed" };
            }

            await _tenantRepository.Update(tenant);
            _logger.LogInformation($"UpdateTenantAsync: tenant with id {tenant.TenantId} updated successfully");

            return new OperationResult { Status = OperationStatus.Ok, Message = "Tenant updated successfully" };
        }

        private bool ValidateTenant(Tenant? tenant)
        {
            if (tenant == null)
            {
                _logger.LogWarning("ValidateTenantAsync: tenant is null");
                return false;
            }

            if (string.IsNullOrWhiteSpace(tenant.Name) || tenant.Name.Length > 100)
            {
                _logger.LogWarning("ValidateTenantAsync: tenant name is invalid");
                return false;
            }

            if (string.IsNullOrWhiteSpace(tenant.Address) || tenant.Address.Length > 200)
            {
                _logger.LogWarning("ValidateTenantAsync: tenant address is invalid");
                return false;
            }

            if (string.IsNullOrWhiteSpace(tenant.Phone) || !Regex.IsMatch(tenant.Phone, @"^\+?[1-9]\d{1,14}$"))
            {
                _logger.LogWarning("ValidateTenantAsync: tenant phone is invalid");
                return false;
            }

            if (string.IsNullOrWhiteSpace(tenant.Email) || !new EmailAddressAttribute().IsValid(tenant.Email))
            {
                _logger.LogWarning("ValidateTenantAsync: tenant email is invalid");
                return false;
            }

            return true;
        }

        #endregion
    }
}