using FLyTicketService.Model;
using FLyTicketService.Service.Interfaces;

namespace FLyTicketService.Service
{
    public class GroupAStrategy : IGroupStrategy
    {
        private IFlightPriceService _flightPriceService;

        public (decimal, decimal) ApplyDiscountBasedOnTenantGroup(IEnumerable<Discount> discounts, Ticket ticket)
        {
            (decimal price, decimal discount) result = _flightPriceService.ApplyDiscounts(discounts, ticket);
            return (result.price, result.discount);
        }

        public IEnumerable<Discount> GetListBasedOnTenantGroup(IEnumerable<Discount> discounts)
        {
            return discounts;
        }

        public GroupAStrategy(IFlightPriceService flightPriceService)
        {
            _flightPriceService = flightPriceService;
        }
    }
}
