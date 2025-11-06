using FLyTicketService.Model;
using FLyTicketService.Service.Interfaces;

namespace FLyTicketService.Service
{

    public class GroupBStrategy : IGroupStrategy
    {
        private IFlightPriceService _flightPriceService;

        public (decimal, decimal) ApplyDiscountBasedOnTenantGroup(IEnumerable<Discount> discounts, Ticket ticket)
        {
            (decimal price, decimal dicount) result = _flightPriceService.ApplyDiscounts(discounts, ticket);
            return (result.price, 0);
        }

        public IEnumerable<Discount> GetListBasedOnTenantGroup(IEnumerable<Discount> discounts)
        {
            return new List<Discount>();
        }

        public GroupBStrategy(IFlightPriceService flightPriceService)
        {
            _flightPriceService = flightPriceService;
        }
    }
}
