using FluentAssertions;
using FLyTicketService.DTO;
using FLyTicketService.Model;
using FLyTicketService.Model.Enums;
using FLyTicketService.Repositories.Interfaces;
using FLyTicketService.Services;
using FLyTicketService.Service;
using FLyTicketService.Service.Interfaces;
using FLyTicketService.Shared;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FlyTicketService.Tests.Services
{
    public class DiscountServiceTests
    {
        private readonly Mock<IGenericRepository<Discount>> _discountRepositoryMock;
        private readonly Mock<IGenericRepository<Condition>> _conditionRepositoryMock;
        private readonly Mock<ILogger<DiscountService>> _loggerMock;
        private readonly Mock<IFlightPriceService> _flightPriceServiceMock;
        private readonly DiscountService _sut;

        public DiscountServiceTests()
        {
            _discountRepositoryMock = new Mock<IGenericRepository<Discount>>();
            _conditionRepositoryMock = new Mock<IGenericRepository<Condition>>();
            _loggerMock = new Mock<ILogger<DiscountService>>();
            _flightPriceServiceMock = new Mock<IFlightPriceService>();
            _sut = new DiscountService(
                _discountRepositoryMock.Object,
                _loggerMock.Object,
                _conditionRepositoryMock.Object,
                _flightPriceServiceMock.Object);
        }

        [Fact]
        public async Task GetDiscountAsync_WhenDiscountExists_ReturnsDiscount()
        {
            // Arrange
            var discountName = "TestDiscount";
            var discount = new Discount
            {
                DiscountId = Guid.NewGuid(),
                Name = discountName,
                Value = 10,
                Description = "Test",
                Conditions = new List<Condition>()
            };

            _discountRepositoryMock
                .Setup(x => x.GetByAsync(It.IsAny<Func<Discount, bool>>()))
                .ReturnsAsync(discount);

            // Act
            var result = await _sut.GetDiscountAsync(discountName);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(OperationStatus.Ok);
            result.Result.Should().NotBeNull();
            result.Result!.Name.Should().Be(discountName);
        }

                [Fact]
        public async Task AddDiscountAsync_WithValidDiscount_ReturnsSuccess()
        {
            // Arrange
            var discountDto = new DiscountDTO
            {
                Name = "EarlyBird",
                Value = 10,
                Description = "Early booking discount",
                Conditions = new List<ConditionDTO>()
            };

            _discountRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Discount>()))
                .ReturnsAsync(true);

            // Act
            var result = await _sut.AddDiscountAsync(discountDto);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(OperationStatus.Ok);
            result.Result.Should().BeTrue();
        }

        [Fact]
        public async Task GetDiscountAsync_WhenDiscountDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var discountName = "NonExistent";

            _discountRepositoryMock
                .Setup(x => x.GetByAsync(It.IsAny<Func<Discount, bool>>()))
                .ReturnsAsync((Discount?)null);

            // Act
            var result = await _sut.GetDiscountAsync(discountName);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(OperationStatus.NotFound);
            result.Result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllDiscountAsync_ReturnsAllDiscounts()
        {
            // Arrange
            var discounts = new List<Discount>
            {
                CreateTestDiscount("Discount1"),
                CreateTestDiscount("Discount2"),
                CreateTestDiscount("Discount3")
            };

            _discountRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(discounts);

            // Act
            var result = await _sut.GetAllDiscountAsync();

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(OperationStatus.Ok);
            result.Result.Should().HaveCount(3);
        }

        [Fact]
        public async Task GetAllDiscountAsync_WhenNoDiscounts_ReturnsEmptyList()
        {
            // Arrange
            _discountRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Discount>());

            // Act
            var result = await _sut.GetAllDiscountAsync();

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(OperationStatus.Ok);
            result.Result.Should().BeEmpty();
        }

        [Fact]
        public async Task UpdateDiscountAsync_WithValidData_ReturnsSuccess()
        {
            // Arrange
            var discountId = Guid.NewGuid();
            var existingDiscount = CreateTestDiscount("OldName");
            existingDiscount.DiscountId = discountId;

            var updateDto = new DiscountDTO
            {
                Name = "UpdatedName",
                Value = 20,
                Description = "Updated description",
                Conditions = new List<ConditionDTO>()
            };

            _discountRepositoryMock
                .Setup(x => x.GetByAsync(It.IsAny<Func<Discount, bool>>()))
                .ReturnsAsync(existingDiscount);

            _discountRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<Discount>()))
                .ReturnsAsync(true);

            // Act
            var result = await _sut.UpdateDiscountAsync(discountId, updateDto);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(OperationStatus.Ok);
            result.Result.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateDiscountAsync_WhenDiscountDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var discountId = Guid.NewGuid();
            var updateDto = new DiscountDTO
            {
                Name = "UpdatedName",
                Value = 20,
                Description = "Updated description",
                Conditions = new List<ConditionDTO>()
            };

            _discountRepositoryMock
                .Setup(x => x.GetByAsync(It.IsAny<Func<Discount, bool>>()))
                .ReturnsAsync((Discount?)null);

            // Act
            var result = await _sut.UpdateDiscountAsync(discountId, updateDto);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(OperationStatus.NotFound);
            result.Result.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteDiscountAsync_WithValidId_ReturnsSuccess()
        {
            // Arrange
            var discountId = Guid.NewGuid();
            var existingDiscount = CreateTestDiscount();
            existingDiscount.DiscountId = discountId;

            _discountRepositoryMock
                .Setup(x => x.GetByAsync(It.IsAny<Func<Discount, bool>>()))
                .ReturnsAsync(existingDiscount);

            _discountRepositoryMock
                .Setup(x => x.RemoveAsync(discountId))
                .ReturnsAsync(true);

            // Act
            var result = await _sut.DeleteDiscountAsync(discountId);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(OperationStatus.Ok);
            result.Result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteDiscountAsync_WhenDiscountDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var discountId = Guid.NewGuid();

            _discountRepositoryMock
                .Setup(x => x.GetByAsync(It.IsAny<Func<Discount, bool>>()))
                .ReturnsAsync((Discount?)null);

            // Act
            var result = await _sut.DeleteDiscountAsync(discountId);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(OperationStatus.NotFound);
            result.Result.Should().BeFalse();
        }

        private Discount CreateTestDiscount(string name = "TestDiscount")
        {
            return new Discount
            {
                DiscountId = Guid.NewGuid(),
                Name = name,
                Value = 10,
                Description = "Test discount",
                Conditions = new List<Condition>()
            };
        }
    }
}
