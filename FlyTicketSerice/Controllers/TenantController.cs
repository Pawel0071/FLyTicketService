using FLyTicketService.DTO;
using FLyTicketService.Service.Interfaces;
using FLyTicketService.Shared;
using Microsoft.AspNetCore.Mvc;

namespace FLyTicketService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TenantController : ControllerBase
    {
        private readonly ITenantService _tenantService;

        public TenantController(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        [HttpPost]
        public async Task<IActionResult> AddTenant([FromBody] TenantDTO tenant)
        {
            OperationResult<bool> result = await _tenantService.AddTenantAsync(tenant);
            return result.GetResult();
        }

        [HttpPut("{tenantId}")]
        public async Task<IActionResult> UpdateTenant(Guid tenantId, [FromBody] TenantDTO tenant)
        {
            OperationResult<bool> result = await _tenantService.UpdateTenantAsync(tenantId, tenant);
            return result.GetResult();
        }

        [HttpDelete("{tenantId}")]
        public async Task<IActionResult> DeleteTenant(Guid tenantId)
        {
            OperationResult<bool> result = await _tenantService.DeleteTenantAsync(tenantId);
            return result.GetResult();
        }

        [HttpGet("{tenantId}")]
        public async Task<IActionResult> GetTenant(Guid tenantId)
        {
            OperationResult<TenantDTO?> result = await _tenantService.GetTenantAsync(tenantId);
            return result.GetResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetTenants()
        {
            OperationResult<IEnumerable<TenantDTO>> result = await _tenantService.GetTenantsAsync();
            return result.GetResult();
        }
    }
}