using FLyTicketService.DTO;
using FLyTicketService.Infrastructure;
using FLyTicketService.Model.Enums;
using FLyTicketService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FLyTicketService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiscountTypeController : ControllerBase
    {
        private readonly IDiscountTypeService _discountTypeService;

        public DiscountTypeController(IDiscountTypeService discountTypeService)
        {
            _discountTypeService = discountTypeService;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetDiscountType(string name)
        {
            OperationResult<DiscountTypeDTO?> result = await _discountTypeService.GetDiscountTypeAsync(name);
            return result.GetResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDiscountType([FromQuery] DiscountCategory? category)
        {
            OperationResult<IEnumerable<DiscountTypeDTO>> result = await _discountTypeService.GetAllDiscountTypeAsync(category);
            return result.GetResult();
        }

        [HttpPost]
        public async Task<IActionResult> AddDiscountType([FromBody] DiscountTypeDTO discountType)
        {
            OperationResult<bool> result = await _discountTypeService.AddDiscountTypeAsync(discountType);
            return StatusCode(result.Status.ToInt(), result.Message);
        }

        [HttpPut("{discountId}")]
        public async Task<IActionResult> UpdateDiscountType(Guid discountId, [FromBody] DiscountTypeDTO discountType)
        {
            OperationResult<bool> result = await _discountTypeService.UpdateDiscountTypeAsync(discountId, discountType);
            return result.GetResult();
        }

        [HttpDelete("{discountId}")]
        public async Task<IActionResult> DeleteDiscountType(Guid discountId)
        {
            OperationResult<bool> result = await _discountTypeService.DeleteDiscountTypeAsync(discountId);
            return result.GetResult();
        }
    }
}
