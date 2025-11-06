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
            var flightPriceServiceMock = new Mock<IFlightPriceService>();
            var groupAStrategy = new GroupAStrategy(flightPriceServiceMock.Object);
            var groupBStrategy = new GroupBStrategy(flightPriceServiceMock.Object);
            var factory = new GroupStrategyFactory(groupAStrategy, groupBStrategy);

            // Act
            var strategy = factory.GetStrategy(TenantGroup.GroupA);

            // Assert
            strategy.Should().NotBeNull();
            strategy.Should().BeAssignableTo<IGroupStrategy>();
            strategy.Should().BeOfType<GroupAStrategy>();
        }

        [Fact]
        public void GetStrategy_ForGroupB_ReturnsGroupBStrategy()
        {
            // Arrange
            var flightPriceServiceMock = new Mock<IFlightPriceService>();
            var groupAStrategy = new GroupAStrategy(flightPriceServiceMock.Object);
            var groupBStrategy = new GroupBStrategy(flightPriceServiceMock.Object);
            var factory = new GroupStrategyFactory(groupAStrategy, groupBStrategy);

            // Act
            var strategy = factory.GetStrategy(TenantGroup.GroupB);

            // Assert
            strategy.Should().NotBeNull();
            strategy.Should().BeAssignableTo<IGroupStrategy>();
            strategy.Should().BeOfType<GroupBStrategy>();
        }
    }
}
