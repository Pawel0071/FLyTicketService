using FLyTicketService.Model;
using FLyTicketService.Service;
using Microsoft.AspNetCore.Mvc;

namespace FLyTicketService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantController(ITenantService _tenantService): ControllerBase
    {
        [HttpGet("{tenantId}")]
        public async Task<ActionResult<Tenant>> GetTenant(Guid tenantId)
        {
            if (tenantId == Guid.Empty)
            {
                return BadRequest("Invalid tenant ID.");
            }

            var tenant = await _tenantService.GetTenantAsync(tenantId);
            if (tenant == null)
            {
                return NotFound();
            }

            return Ok(tenant);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tenant>>> GetTenants()
        {
            IEnumerable<Tenant> tenants = await _tenantService.GetTenantsAsync();
            return Ok(tenants);
        }

        [HttpPost]
        public async Task<ActionResult> AddTenant(Tenant tenant)
        {
            if (tenant == null)
            {
                return BadRequest("Tenant is null.");
            }
            if (!await _tenantService.ValidateTenantAsync(tenant))
            {
                return BadRequest("Tenant is invalid.");
            }
            OperationResult result = await _tenantService.AddTenantAsync(tenant);
            if (result == OperationResult.Ok)
            {
                return Ok();
            }
            return StatusCode(StatusCodes.Status500InternalServerError, result);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateTenant(Tenant tenant)
        {
            if (tenant == null)
            {
                return BadRequest("Tenant is null.");
            }
            if (!await _tenantService.ValidateTenantAsync(tenant))
            {
                return BadRequest("Tenant is invalid.");
            }
            OperationResult result = await _tenantService.UpdateTenantAsync(tenant);
            if (result == OperationResult.Add)
            {
                return Ok();
            }
            return StatusCode(StatusCodes.Status500InternalServerError, result);
        }

        [HttpDelete("{tenantId}")]
        public async Task<ActionResult> DeleteTenant(Guid tenantId)
        {
            if (tenantId == Guid.Empty)
            {
                return BadRequest("Invalid tenant ID.");
            }
            OperationResult result = await _tenantService.DeleteTenantAsync(tenantId);
            if (result == OperationResult.Ok)
            {
                return Ok();
            }
            return StatusCode(StatusCodes.Status500InternalServerError, result);
        }
    }
}
