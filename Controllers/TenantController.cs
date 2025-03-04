using FLyTicketService.DTO;
using FLyTicketService.Infrastructure;
using FLyTicketService.Model;
using FLyTicketService.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace FLyTicketService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantController(ITenantService _tenantService): ControllerBase
    {
        #region Methods

        /// <summary>
        /// Get a tenant by ID.
        /// </summary>
        /// <param name="tenantId">The ID of the tenant.</param>
        /// <returns>The tenant with the specified ID.</returns>
        [HttpGet("{tenantId}")]
        [OpenApiOperation("GetTenantById", "Get a tenant by ID.")]
        public async Task<ActionResult<Tenant>> GetTenant(Guid tenantId)
        {
            TenantDTO? tenant = await _tenantService.GetTenantAsync(tenantId);
            if (tenant is null)
            {
                return NotFound();
            }

            return Ok(tenant);
        }

        /// <summary>
        /// Get all tenants.
        /// </summary>
        /// <returns>A list of all tenants.</returns>
        [HttpGet]
        [OpenApiOperation("GetTenants", "Get all tenants.")]
        public async Task<ActionResult<IEnumerable<Tenant>>> GetTenants()
        {
            IEnumerable<Tenant> tenants = await _tenantService.GetTenantsAsync();
            return Ok(tenants);
        }

        /// <summary>
        /// Add a new tenant.
        /// </summary>
        /// <param name="tenant">The tenant to add.</param>
        /// <returns>Result of the add operation.</returns>
        [HttpPost]
        [OpenApiOperation("AddTenant", "Add a new tenant.")]
        public async Task<ActionResult> AddTenant([FromBody] Tenant tenant)
        {
            OperationResult result = await _tenantService.AddTenantAsync(tenant);
            return StatusCode(result.Status.ToInt(), result.Message);
        }

        /// <summary>
        /// Update an existing tenant.
        /// </summary>
        /// <param name="tenant">The tenant to update.</param>
        /// <returns>Result of the update operation.</returns>
        [HttpPut]
        [OpenApiOperation("UpdateTenant", "Update an existing tenant.")]
        public async Task<ActionResult> UpdateTenant([FromBody] Tenant tenant)
        {
            OperationResult result = await _tenantService.UpdateTenantAsync(tenant);
            return StatusCode(result.Status.ToInt(), result.Message);
        }

        /// <summary>
        /// Delete a tenant by ID.
        /// </summary>
        /// <param name="tenantId">The ID of the tenant to delete.</param>
        /// <returns>Result of the delete operation.</returns>
        [HttpDelete("{tenantId}")]
        [OpenApiOperation("DeleteTenant", "Delete a tenant by ID.")]
        public async Task<ActionResult> DeleteTenant(Guid tenantId)
        {
            OperationResult result = await _tenantService.DeleteTenantAsync(tenantId);
            return StatusCode(result.Status.ToInt(), result.Message);
        }

        #endregion
    }
}