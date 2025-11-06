using FluentAssertions;
using FLyTicketService.Model;
using FLyTicketService.Model.Enums;
using FLyTicketService.Repositories.Interfaces;
using FLyTicketService.Service;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FlyTicketService.Tests.Services
{
    public class DiscountServiceTests
    {
        private readonly Mock<IGenericRepository<Discount>> _discountRepositoryMock;
        private readonly Mock<ILogger<DiscountService>> _loggerMock;
        private readonly DiscountService _sut;

        public DiscountServiceTests()
        {
            _discountRepositoryMock = new Mock<IGenericRepository<Discount>>();
            _loggerMock = new Mock<ILogger<DiscountService>>();
            _sut = new DiscountService(_discountRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetDiscountAsync_WhenDiscountExists_ReturnsDiscount()
        {
            // Arrange
            var discountId = Guid.NewGuid();
            var discount = CreateTestDiscount(discountId, "Test Discount", 10m);

            _discountRepositoryMock
                .Setup(x => x.GetByIdAsync(discountId))
                .ReturnsAsync(discount);

            // Act
            var result = await _sut.GetDiscountAsync(discountId);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(FLyTicketService.Shared.OperationStatus.Ok);
            result.Data.Should().NotBeNull();
            result.Data!.DiscountId.Should().Be(discountId);
        }

        [Fact]
        public async Task GetDiscountAsync_WhenDiscountDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var discountId = Guid.NewGuid();

            _discountRepositoryMock
                .Setup(x => x.GetByIdAsync(discountId))
                .ReturnsAsync((Discount?)null);

            // Act
            var result = await _sut.GetDiscountAsync(discountId);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(FLyTicketService.Shared.OperationStatus.NotFound);
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task GetAllDiscountsAsync_ReturnsAllDiscounts()
        {
            // Arrange
            var discounts = new List<Discount>
            {
                CreateTestDiscount(Guid.NewGuid(), "Discount 1", 10m),
                CreateTestDiscount(Guid.NewGuid(), "Discount 2", 20m),
                CreateTestDiscount(Guid.NewGuid(), "Discount 3", 15m)
            };

            _discountRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(discounts);

            // Act
            var result = await _sut.GetAllDiscountsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(FLyTicketService.Shared.OperationStatus.Ok);
            result.Data.Should().HaveCount(3);
        }

        [Fact]
        public async Task CreateDiscountAsync_WithValidData_CreatesSuccessfully()
        {
            // Arrange
            var discount = CreateTestDiscount(Guid.NewGuid(), "New Discount", 25m);

            _discountRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Discount>()))
                .ReturnsAsync(true);

            // Act
            var result = await _sut.CreateDiscountAsync(discount);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(FLyTicketService.Shared.OperationStatus.Ok);
            _discountRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Discount>()), Times.Once);
        }

        [Fact]
        public async Task CreateDiscountAsync_WhenRepositoryFails_ReturnsError()
        {
            // Arrange
            var discount = CreateTestDiscount(Guid.NewGuid(), "New Discount", 25m);

            _discountRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Discount>()))
                .ReturnsAsync(false);

            // Act
            var result = await _sut.CreateDiscountAsync(discount);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(FLyTicketService.Shared.OperationStatus.InternalServerError);
        }

        [Fact]
        public async Task UpdateDiscountAsync_WithValidData_UpdatesSuccessfully()
        {
            // Arrange
            var discountId = Guid.NewGuid();
            var existingDiscount = CreateTestDiscount(discountId, "Old Discount", 10m);
            var updatedDiscount = CreateTestDiscount(discountId, "Updated Discount", 15m);

            _discountRepositoryMock
                .Setup(x => x.GetByIdAsync(discountId))
                .ReturnsAsync(existingDiscount);

            _discountRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<Discount>()))
                .ReturnsAsync(true);

            // Act
            var result = await _sut.UpdateDiscountAsync(discountId, updatedDiscount);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(FLyTicketService.Shared.OperationStatus.Ok);
            _discountRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Discount>()), Times.Once);
        }

        [Fact]
        public async Task UpdateDiscountAsync_WhenDiscountNotFound_ReturnsNotFound()
        {
            // Arrange
            var discountId = Guid.NewGuid();
            var updatedDiscount = CreateTestDiscount(discountId, "Updated Discount", 15m);

            _discountRepositoryMock
                .Setup(x => x.GetByIdAsync(discountId))
                .ReturnsAsync((Discount?)null);

            // Act
            var result = await _sut.UpdateDiscountAsync(discountId, updatedDiscount);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(FLyTicketService.Shared.OperationStatus.NotFound);
        }

        [Fact]
        public async Task DeleteDiscountAsync_WhenDiscountExists_DeletesSuccessfully()
        {
            // Arrange
            var discountId = Guid.NewGuid();
            var discount = CreateTestDiscount(discountId, "Test Discount", 10m);

            _discountRepositoryMock
                .Setup(x => x.GetByIdAsync(discountId))
                .ReturnsAsync(discount);

            _discountRepositoryMock
                .Setup(x => x.DeleteAsync(discount))
                .ReturnsAsync(true);

            // Act
            var result = await _sut.DeleteDiscountAsync(discountId);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(FLyTicketService.Shared.OperationStatus.Ok);
            result.Data.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteDiscountAsync_WhenDiscountNotFound_ReturnsNotFound()
        {
            // Arrange
            var discountId = Guid.NewGuid();

            _discountRepositoryMock
                .Setup(x => x.GetByIdAsync(discountId))
                .ReturnsAsync((Discount?)null);

            // Act
            var result = await _sut.DeleteDiscountAsync(discountId);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(FLyTicketService.Shared.OperationStatus.NotFound);
        }

        [Fact]
        public async Task GetDiscountsByConditionAsync_WithCategory_ReturnsFilteredDiscounts()
        {
            // Arrange
            var category = DiscountCategory.Destination;
            var discounts = new List<Discount>
            {
                CreateTestDiscount(Guid.NewGuid(), "Destination Discount", 10m),
                CreateTestDiscount(Guid.NewGuid(), "Airline Discount", 15m)
            };

            _discountRepositoryMock
                .Setup(x => x.GetByExpressionAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Discount, bool>>>()))
                .ReturnsAsync(discounts.Where(d => d.Conditions.Any(c => c.Category == category)));

            // Act
            var result = await _sut.GetDiscountsByConditionAsync(category);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(FLyTicketService.Shared.OperationStatus.Ok);
        }

        private Discount CreateTestDiscount(Guid discountId, string name, decimal value)
        {
            return new Discount
            {
                DiscountId = discountId,
                Name = name,
                Value = value,
                Conditions = new List<Condition>
                {
                    new Condition
                    {
                        ConditionId = Guid.NewGuid(),
                        Category = DiscountCategory.Destination,
                        ConditionType = DiscountCondition.IATA,
                        Value = "WAW"
                    }
                }
            };
        }
    }
}
