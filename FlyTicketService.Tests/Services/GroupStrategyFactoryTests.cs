using FluentAssertions;
using FLyTicketService.Model.Enums;
using FLyTicketService.Service;
using FLyTicketService.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace FlyTicketService.Tests.Services
{
    public class GroupStrategyFactoryTests
    {
        [Fact]
        public void GetStrategy_ForGroupA_ReturnsGroupAStrategy()
        {
            // Arrange
            var serviceProvider = CreateServiceProvider();
            var factory = new GroupStrategyFactory(serviceProvider);

            // Act
            var strategy = factory.GetStrategy(TenantGroup.GroupA);

            // Assert
            strategy.Should().NotBeNull();
            strategy.Should().BeAssignableTo<IGroupStrategy>();
        }

        [Fact]
        public void GetStrategy_ForGroupB_ReturnsGroupBStrategy()
        {
            // Arrange
            var serviceProvider = CreateServiceProvider();
            var factory = new GroupStrategyFactory(serviceProvider);

            // Act
            var strategy = factory.GetStrategy(TenantGroup.GroupB);

            // Assert
            strategy.Should().NotBeNull();
            strategy.Should().BeAssignableTo<IGroupStrategy>();
        }

        private IServiceProvider CreateServiceProvider()
        {
            var services = new ServiceCollection();
            
            var flightPriceServiceMock = new Mock<IFlightPriceService>();
            services.AddSingleton(flightPriceServiceMock.Object);
            
            services.AddTransient<GroupAStrategy>();
            services.AddTransient<GroupBStrategy>();
            
            // Register keyed services
            services.AddKeyedTransient<IGroupStrategy, GroupAStrategy>(TenantGroup.GroupA);
            services.AddKeyedTransient<IGroupStrategy, GroupBStrategy>(TenantGroup.GroupB);

            return services.BuildServiceProvider();
        }
    }
}
