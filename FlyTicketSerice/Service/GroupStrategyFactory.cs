using FLyTicketService.Model.Enums;
using FLyTicketService.Service.Interfaces;

namespace FLyTicketService.Service
{
    public class GroupStrategyFactory: IGroupStrategyFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public GroupStrategyFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IGroupStrategy GetStrategy(TenantGroup group)
        {
            return group switch
            {
                TenantGroup.GroupA => _serviceProvider.GetRequiredService<GroupAStrategy>(),
                TenantGroup.GroupB => _serviceProvider.GetRequiredService<GroupBStrategy>(),
                _ => throw new ArgumentException("Invalid group specified")
            };

        }
    }
}
