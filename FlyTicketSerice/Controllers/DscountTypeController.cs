using FLyTicketService.DTO;
using FLyTicketService.Services.Interfaces;
using FLyTicketService.Shared;
using Microsoft.AspNetCore.Mvc;

namespace FLyTicketService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiscountTypeController : ControllerBase
    {
        private readonly IDiscountService _discountService;

        public DiscountTypeController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetDiscountType(string name)
        {
            OperationResult<DiscountDTO?> result = await _discountService.GetDiscountAsync(name);
            return result.GetResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDiscountType()
        {
            OperationResult<IEnumerable<DiscountDTO>> result = await _discountService.GetAllDiscountAsync();
            return result.GetResult();
        }

        [HttpPost]
        public async Task<IActionResult> AddDiscountType([FromBody] DiscountDTO discountType)
        {
            OperationResult<bool> result = await _discountService.AddDiscountAsync(discountType);
            return StatusCode(result.Status.ToInt(), result.Message);
        }

        [HttpPut("{discountId}")]
        public async Task<IActionResult> UpdateDiscountType(Guid discountId, [FromBody] DiscountDTO discountType)
        {
            OperationResult<bool> result = await _discountService.UpdateDiscountAsync(discountId, discountType);
            return result.GetResult();
        }

        [HttpDelete("{discountId}")]
        public async Task<IActionResult> DeleteDiscountType(Guid discountId)
        {
            OperationResult<bool> result = await _discountService.DeleteDiscountAsync(discountId);
            return result.GetResult();
        }
    }
}
