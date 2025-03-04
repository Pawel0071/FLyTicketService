using FLyTicketService.DTO;
using FLyTicketService.Infrastructure;
using FLyTicketService.Model;
using FLyTicketService.Model.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FLyTicketService.Services.Interfaces
{
    public interface IDiscountTypeService
    {
        Task<DiscountType?> GetDiscountTypeAsync(string name);
        Task<IEnumerable<DiscountType>> GetAllDiscountTypeAsync();
        Task<OperationResult> AddDiscountTypeAsync(DiscountTypeDTO discountType);
        Task<OperationResult> UpdateDiscountTypeAsync(DiscountTypeDTO discountType);
        Task<OperationResult> DeleteDiscountTypeAsync(DiscountTypeDTO discountType);
    }
}