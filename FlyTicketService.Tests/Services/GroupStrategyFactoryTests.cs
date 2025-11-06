using FluentAssertions;
using FLyTicketService.Model.Enums;
using FLyTicketService.Service;
using FLyTicketService.Service.Interfaces;
using Moq;
using Xunit;

namespace FlyTicketService.Tests.Services
{
    public class GroupStrategyFactoryTests
    {
        private readonly Mock<IGroupStrategy> _groupAStrategyMock;
        private readonly Mock<IGroupStrategy> _groupBStrategyMock;
        private readonly GroupStrategyFactory _sut;

        public GroupStrategyFactoryTests()
        {
            _groupAStrategyMock = new Mock<IGroupStrategy>();
            _groupBStrategyMock = new Mock<IGroupStrategy>();
            
            var strategies = new List<IGroupStrategy>
            {
                _groupAStrategyMock.Object,
                _groupBStrategyMock.Object
            };

            _sut = new GroupStrategyFactory(strategies);
        }

        [Fact]
        public void GetStrategy_ForGroupA_ReturnsGroupAStrategy()
        {
            // Arrange
            var groupAStrategy = new GroupAStrategy(Mock.Of<IFlightPriceService>());
            var groupBStrategy = new GroupBStrategy(Mock.Of<IFlightPriceService>());
            var strategies = new List<IGroupStrategy> { groupAStrategy, groupBStrategy };
            var factory = new GroupStrategyFactory(strategies);

            // Act
            var result = factory.GetStrategy(TenantGroup.GroupA);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<GroupAStrategy>();
        }

        [Fact]
        public void GetStrategy_ForGroupB_ReturnsGroupBStrategy()
        {
            // Arrange
            var groupAStrategy = new GroupAStrategy(Mock.Of<IFlightPriceService>());
            var groupBStrategy = new GroupBStrategy(Mock.Of<IFlightPriceService>());
            var strategies = new List<IGroupStrategy> { groupAStrategy, groupBStrategy };
            var factory = new GroupStrategyFactory(strategies);

            // Act
            var result = factory.GetStrategy(TenantGroup.GroupB);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<GroupBStrategy>();
        }

        [Fact]
        public void GetStrategy_WithInvalidGroup_ThrowsException()
        {
            // Arrange
            var groupAStrategy = new GroupAStrategy(Mock.Of<IFlightPriceService>());
            var strategies = new List<IGroupStrategy> { groupAStrategy };
            var factory = new GroupStrategyFactory(strategies);

            // Act
            Action act = () => factory.GetStrategy((TenantGroup)999);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Constructor_WithNoStrategies_ThrowsException()
        {
            // Arrange
            var emptyStrategies = new List<IGroupStrategy>();

            // Act
            Action act = () => new GroupStrategyFactory(emptyStrategies);

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Constructor_WithNullStrategies_ThrowsException()
        {
            // Arrange & Act
            Action act = () => new GroupStrategyFactory(null!);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
