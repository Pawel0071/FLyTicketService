using FLyTicketService.Model.Enums;

namespace FLyTicketService.Service.Interfaces
{
    public interface IGroupStrategyFactory
    {
        IGroupStrategy GetStrategy(TenantGroup group);
    }
}
