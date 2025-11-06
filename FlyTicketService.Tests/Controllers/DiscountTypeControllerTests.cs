using FluentAssertions;
using FLyTicketService.Controllers;
using FLyTicketService.DTO;
using FLyTicketService.Services.Interfaces;
using FLyTicketService.Shared;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FlyTicketService.Tests.Controllers
{
    public class DiscountTypeControllerTests
    {
        private readonly Mock<IDiscountService> _discountServiceMock;
        private readonly DiscountTypeController _controller;

        public DiscountTypeControllerTests()
        {
            _discountServiceMock = new Mock<IDiscountService>();
            _controller = new DiscountTypeController(_discountServiceMock.Object);
        }

        [Fact]
        public async Task GetDiscountType_WithValidName_ReturnsOk()
        {
            // Arrange
            var discountName = "Senior";
            var discountDto = new DiscountDTO
            {
                DiscountTypeId = Guid.NewGuid(),
                Name = discountName,
                Value = 10
            };

            var operationResult = new OperationResult<DiscountDTO?>(
                OperationStatus.Ok,
                "Discount found",
                discountDto);

            _discountServiceMock
                .Setup(x => x.GetDiscountAsync(discountName))
                .ReturnsAsync(operationResult);

            // Act
            var result = await _controller.GetDiscountType(discountName);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult!.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetDiscountType_WhenNotFound_ReturnsNotFound()
        {
            // Arrange
            var discountName = "NonExistent";

            var operationResult = new OperationResult<DiscountDTO?>(
                OperationStatus.NotFound,
                "Discount not found",
                null);

            _discountServiceMock
                .Setup(x => x.GetDiscountAsync(discountName))
                .ReturnsAsync(operationResult);

            // Act
            var result = await _controller.GetDiscountType(discountName);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult!.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task GetAllDiscountType_ReturnsOkWithList()
        {
            // Arrange
            var discounts = new List<DiscountDTO>
            {
                new DiscountDTO { DiscountTypeId = Guid.NewGuid(), Name = "Senior", Value = 10 },
                new DiscountDTO { DiscountTypeId = Guid.NewGuid(), Name = "Student", Value = 15 }
            };

            var operationResult = new OperationResult<IEnumerable<DiscountDTO>>(
                OperationStatus.Ok,
                "Discounts retrieved",
                discounts);

            _discountServiceMock
                .Setup(x => x.GetAllDiscountAsync())
                .ReturnsAsync(operationResult);

            // Act
            var result = await _controller.GetAllDiscountType();

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult!.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task AddDiscountType_WithValidData_ReturnsOk()
        {
            // Arrange
            var discountDto = new DiscountDTO
            {
                Name = "New Discount",
                Value = 20
            };

            var operationResult = new OperationResult<bool>(
                OperationStatus.Ok,
                "Discount added successfully",
                true);

            _discountServiceMock
                .Setup(x => x.AddDiscountAsync(discountDto))
                .ReturnsAsync(operationResult);

            // Act
            var result = await _controller.AddDiscountType(discountDto);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult!.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task UpdateDiscountType_WithValidData_ReturnsOk()
        {
            // Arrange
            var discountId = Guid.NewGuid();
            var discountDto = new DiscountDTO
            {
                Name = "Updated Discount",
                Value = 25
            };

            var operationResult = new OperationResult<bool>(
                OperationStatus.Ok,
                "Discount updated successfully",
                true);

            _discountServiceMock
                .Setup(x => x.UpdateDiscountAsync(discountId, discountDto))
                .ReturnsAsync(operationResult);

            // Act
            var result = await _controller.UpdateDiscountType(discountId, discountDto);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult!.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task UpdateDiscountType_WhenNotFound_ReturnsNotFound()
        {
            // Arrange
            var discountId = Guid.NewGuid();
            var discountDto = new DiscountDTO
            {
                Name = "Updated Discount",
                Value = 25
            };

            var operationResult = new OperationResult<bool>(
                OperationStatus.NotFound,
                "Discount not found",
                false);

            _discountServiceMock
                .Setup(x => x.UpdateDiscountAsync(discountId, discountDto))
                .ReturnsAsync(operationResult);

            // Act
            var result = await _controller.UpdateDiscountType(discountId, discountDto);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult!.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task DeleteDiscountType_WithValidId_ReturnsOk()
        {
            // Arrange
            var discountId = Guid.NewGuid();

            var operationResult = new OperationResult<bool>(
                OperationStatus.Ok,
                "Discount deleted successfully",
                true);

            _discountServiceMock
                .Setup(x => x.DeleteDiscountAsync(discountId))
                .ReturnsAsync(operationResult);

            // Act
            var result = await _controller.DeleteDiscountType(discountId);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult!.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task DeleteDiscountType_WhenNotFound_ReturnsNotFound()
        {
            // Arrange
            var discountId = Guid.NewGuid();

            var operationResult = new OperationResult<bool>(
                OperationStatus.NotFound,
                "Discount not found",
                false);

            _discountServiceMock
                .Setup(x => x.DeleteDiscountAsync(discountId))
                .ReturnsAsync(operationResult);

            // Act
            var result = await _controller.DeleteDiscountType(discountId);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult!.StatusCode.Should().Be(404);
        }
    }
}
