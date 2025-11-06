using FLyTicketService.Model;

namespace FLyTicketService.Service.Interfaces
{
    public interface IGroupStrategy
    {
        (decimal, decimal) ApplyDiscountBasedOnTenantGroup(IEnumerable<Discount> discounts, Ticket ticket);

        IEnumerable<Discount> GetListBasedOnTenantGroup(IEnumerable<Discount> discounts);
    }
}
