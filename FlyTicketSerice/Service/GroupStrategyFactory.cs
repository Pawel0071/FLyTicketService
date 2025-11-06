using FLyTicketService.Model.Enums;
using FLyTicketService.Service.Interfaces;

namespace FLyTicketService.Service
{
    public class GroupStrategyFactory: IGroupStrategyFactory
    {
        private readonly GroupAStrategy _groupAStrategy;
        private readonly GroupBStrategy _groupBStrategy;

        public GroupStrategyFactory(GroupAStrategy groupAStrategy, GroupBStrategy groupBStrategy)
        {
            _groupAStrategy = groupAStrategy;
            _groupBStrategy = groupBStrategy;
        }

        public IGroupStrategy GetStrategy(TenantGroup group)
        {
            return group switch
            {
                TenantGroup.GroupA => _groupAStrategy,
                TenantGroup.GroupB => _groupBStrategy,
                _ => throw new ArgumentException("Invalid group specified")
            };

        }
    }
}
