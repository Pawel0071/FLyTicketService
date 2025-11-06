using FluentAssertions;
using FLyTicketService.Controllers;
using FLyTicketService.DTO;
using FLyTicketService.Model.Enums;
using FLyTicketService.Service.Interfaces;
using FLyTicketService.Shared;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FlyTicketService.Tests.Controllers
{
    public class TicketControllerTests
    {
        private readonly Mock<ITicketService> _ticketServiceMock;
        private readonly TicketController _controller;

        public TicketControllerTests()
        {
            _ticketServiceMock = new Mock<ITicketService>();
            _controller = new TicketController(_ticketServiceMock.Object);
        }

        [Fact]
        public async Task ReserveTicket_WithValidData_ReturnsResult()
        {
            // Arrange
            var flightId = "FL123";
            var seatNo = "1A";
            var tenantId = Guid.NewGuid();

            var ticketDto = new TicketDTO
            {
                TicketId = Guid.NewGuid(),
                TicketNumber = "TICKET123",
                FlightId = flightId,
                SeatNumber = seatNo,
                TenantId = tenantId,
                Tenant = "John Doe",
                Price = 100,
                Status = TicketStatus.Reserved
            };

            var operationResult = new OperationResult<TicketDTO?>(
                OperationStatus.Ok,
                "Ticket reserved successfully",
                ticketDto);

            _ticketServiceMock
                .Setup(x => x.ReserveTicketAsync(flightId, seatNo, tenantId))
                .ReturnsAsync(operationResult);

            // Act
            var result = await _controller.ReserveTicket(flightId, seatNo, tenantId);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult!.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task ReserveTicket_WhenServiceFails_ReturnsBadRequest()
        {
            // Arrange
            var flightId = "FL123";
            var seatNo = "1A";
            var tenantId = Guid.NewGuid();

            var operationResult = new OperationResult<TicketDTO?>(
                OperationStatus.BadRequest,
                "Seat not available",
                null);

            _ticketServiceMock
                .Setup(x => x.ReserveTicketAsync(flightId, seatNo, tenantId))
                .ReturnsAsync(operationResult);

            // Act
            var result = await _controller.ReserveTicket(flightId, seatNo, tenantId);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult!.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task SellTicket_WithValidData_ReturnsResult()
        {
            // Arrange
            var flightId = "FL123";
            var seatNo = "1A";
            var tenantId = Guid.NewGuid();
            var discounts = new List<DiscountDTO>();

            var ticketDto = new TicketDTO
            {
                TicketId = Guid.NewGuid(),
                TicketNumber = "TICKET123",
                FlightId = flightId,
                SeatNumber = seatNo,
                TenantId = tenantId,
                Tenant = "John Doe",
                Price = 100,
                Status = TicketStatus.Sold
            };

            var operationResult = new OperationResult<TicketDTO?>(
                OperationStatus.Ok,
                "Ticket sold successfully",
                ticketDto);

            _ticketServiceMock
                .Setup(x => x.SaleTicketAsync(flightId, seatNo, tenantId, discounts))
                .ReturnsAsync(operationResult);

            // Act
            var result = await _controller.SellTicket(flightId, seatNo, tenantId, discounts);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult!.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task CancelTicket_WithValidTicketNumber_ReturnsResult()
        {
            // Arrange
            var ticketNumber = "TICKET123";

            var operationResult = new OperationResult<bool>(
                OperationStatus.Ok,
                "Ticket cancelled successfully",
                true);

            _ticketServiceMock
                .Setup(x => x.CancelTicketAsync(ticketNumber))
                .ReturnsAsync(operationResult);

            // Act
            var result = await _controller.CancelTicket(ticketNumber);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult!.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetTicket_WithValidTicketNumber_ReturnsResult()
        {
            // Arrange
            var ticketNumber = "TICKET123";

            var ticketDto = new TicketDTO
            {
                TicketId = Guid.NewGuid(),
                TicketNumber = ticketNumber,
                FlightId = "FL123",
                SeatNumber = "1A",
                TenantId = Guid.NewGuid(),
                Tenant = "John Doe",
                Price = 100,
                Status = TicketStatus.Reserved
            };

            var operationResult = new OperationResult<TicketDTO?>(
                OperationStatus.Ok,
                "Ticket found",
                ticketDto);

            _ticketServiceMock
                .Setup(x => x.GetTicketAsync(ticketNumber))
                .ReturnsAsync(operationResult);

            // Act
            var result = await _controller.GetTicket(ticketNumber);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult!.StatusCode.Should().Be(200);
        }
    }
}
