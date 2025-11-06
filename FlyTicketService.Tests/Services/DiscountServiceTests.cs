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
                DiscountTypeId = Guid.NewGuid(),
                Name = "TestDiscount",
                Value = 10,
                Description = "Test"
            };

            _discountRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Discount>()))
                .ReturnsAsync(true);

            // Act
            var result = await _sut.AddDiscountAsync(discountDto);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(OperationStatus.Ok);
        }
    }
}
