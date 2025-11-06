using FLyTicketService.Model;

namespace FLyTicketService.Service.Interfaces
{
    public interface IFlightPriceService
    {
        (decimal, decimal) ApplyDiscounts(IEnumerable<Discount> discounts, Ticket ticket);
        Task<List<Discount>> GetAllApplicableDiscountsAsync(Ticket ticket);
        bool IsDiscountApplicable(Discount discount, Ticket ticket);
        Task<List<Discount>> GetAllDiscountsAsync();
    }
}
