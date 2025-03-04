using FLyTicketService.DTO;
using FLyTicketService.Infrastructure;
using FLyTicketService.Model.Enums;

namespace FLyTicketService.Services.Interfaces
{
    public interface IDiscountTypeService
    {
        Task<OperationResult<DiscountTypeDTO?>> GetDiscountTypeAsync(string name);
        Task<OperationResult<IEnumerable<DiscountTypeDTO>>> GetAllDiscountTypeAsync(DiscountCategory? category);
        Task<OperationResult<bool>> AddDiscountTypeAsync(DiscountTypeDTO discountType);
        Task<OperationResult<bool>> UpdateDiscountTypeAsync(Guid discountId, DiscountTypeDTO discountType);
        Task<OperationResult<bool>> DeleteDiscountTypeAsync(Guid discountId);
    }
}
