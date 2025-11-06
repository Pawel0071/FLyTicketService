using FLyTicketService.DTO;
using FLyTicketService.Shared;

namespace FLyTicketService.Services.Interfaces
{
    public interface IDiscountService
    {
        Task<OperationResult<DiscountDTO?>> GetDiscountAsync(string name);
        Task<OperationResult<IEnumerable<DiscountDTO>>> GetAllDiscountAsync();
        Task<OperationResult<bool>> AddDiscountAsync(DiscountDTO discountType);
        Task<OperationResult<bool>> UpdateDiscountAsync(Guid discountId, DiscountDTO discountType);
        Task<OperationResult<bool>> DeleteDiscountAsync(Guid discountId);
    }
}
