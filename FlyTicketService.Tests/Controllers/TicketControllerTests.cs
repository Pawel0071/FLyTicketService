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
        public async Task ReserveTicket_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var flightId = "FL123";
            var seatNo = "1A";
            var tenantId = Guid.NewGuid();
            var ticketDto = CreateTestTicketDto();

            _ticketServiceMock
                .Setup(x => x.ReserveTicketAsync(flightId, seatNo, tenantId))
                .ReturnsAsync(new OperationResult<TicketDTO?>(OperationStatus.Ok, "", ticketDto));

            // Act
            var result = await _controller.ReserveTicket(flightId, seatNo, tenantId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            _ticketServiceMock.Verify(x => x.ReserveTicketAsync(flightId, seatNo, tenantId), Times.Once);
        }

        [Fact]
        public async Task ReserveTicket_WhenServiceFails_ReturnsBadRequest()
        {
            // Arrange
            var flightId = "FL123";
            var seatNo = "1A";
            var tenantId = Guid.NewGuid();

            _ticketServiceMock
                .Setup(x => x.ReserveTicketAsync(flightId, seatNo, tenantId))
                .ReturnsAsync(new OperationResult<TicketDTO?>(OperationStatus.BadRequest, "Invalid data", null));

            // Act
            var result = await _controller.ReserveTicket(flightId, seatNo, tenantId);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task SellTicket_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var flightId = "FL123";
            var seatNo = "1A";
            var tenantId = Guid.NewGuid();
            var discounts = new List<DiscountDTO>
            {
                new DiscountDTO { DiscountId = Guid.NewGuid(), Name = "Test Discount", Value = 10m }
            };
            var ticketDto = CreateTestTicketDto();

            _ticketServiceMock
                .Setup(x => x.SaleTicketAsync(flightId, seatNo, tenantId, discounts))
                .ReturnsAsync(new OperationResult<TicketDTO?>(OperationStatus.Ok, "", ticketDto));

            // Act
            var result = await _controller.SellTicket(flightId, seatNo, tenantId, discounts);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            _ticketServiceMock.Verify(x => x.SaleTicketAsync(flightId, seatNo, tenantId, discounts), Times.Once);
        }

        [Fact]
        public async Task SellReservedTicket_WithValidTicketNumber_ReturnsOkResult()
        {
            // Arrange
            var ticketNumber = "TKT123";
            var ticketDto = CreateTestTicketDto();

            _ticketServiceMock
                .Setup(x => x.SaleReservedTicketAsync(ticketNumber))
                .ReturnsAsync(new OperationResult<TicketDTO?>(OperationStatus.Ok, "", ticketDto));

            // Act
            var result = await _controller.SellReservedTicket(ticketNumber);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            _ticketServiceMock.Verify(x => x.SaleReservedTicketAsync(ticketNumber), Times.Once);
        }

        [Fact]
        public async Task SellReservedTicket_WhenTicketNotFound_ReturnsNotFound()
        {
            // Arrange
            var ticketNumber = "TKT123";

            _ticketServiceMock
                .Setup(x => x.SaleReservedTicketAsync(ticketNumber))
                .ReturnsAsync(new OperationResult<TicketDTO?>(OperationStatus.NotFound, "Ticket not found", null));

            // Act
            var result = await _controller.SellReservedTicket(ticketNumber);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task CancelTicket_WithValidTicketNumber_ReturnsOkResult()
        {
            // Arrange
            var ticketNumber = "TKT123";

            _ticketServiceMock
                .Setup(x => x.CancelTicketAsync(ticketNumber))
                .ReturnsAsync(new OperationResult<bool>(OperationStatus.Ok, "", true));

            // Act
            var result = await _controller.CancelTicket(ticketNumber);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            _ticketServiceMock.Verify(x => x.CancelTicketAsync(ticketNumber), Times.Once);
        }

        [Fact]
        public async Task CancelTicket_WhenTicketNotFound_ReturnsNotFound()
        {
            // Arrange
            var ticketNumber = "TKT123";

            _ticketServiceMock
                .Setup(x => x.CancelTicketAsync(ticketNumber))
                .ReturnsAsync(new OperationResult<bool>(OperationStatus.NotFound, "Ticket not found", false));

            // Act
            var result = await _controller.CancelTicket(ticketNumber);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task GetTicket_WithValidTicketNumber_ReturnsOkResult()
        {
            // Arrange
            var ticketNumber = "TKT123";
            var ticketDto = CreateTestTicketDto();

            _ticketServiceMock
                .Setup(x => x.GetTicketAsync(ticketNumber))
                .ReturnsAsync(new OperationResult<TicketDTO?>(OperationStatus.Ok, "", ticketDto));

            // Act
            var result = await _controller.GetTicket(ticketNumber);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetTickets_WithFilters_ReturnsOkResult()
        {
            // Arrange
            var flyNumber = "FL123";
            var tenantId = Guid.NewGuid();
            var departure = DateTime.Now.AddDays(7);
            var originIATA = "WAW";
            var destinationIATA = "JFK";
            var tickets = new List<TicketDTO> { CreateTestTicketDto() };

            _ticketServiceMock
                .Setup(x => x.GetTicketListByAsync(flyNumber, tenantId, departure, originIATA, destinationIATA))
                .ReturnsAsync(new OperationResult<IEnumerable<TicketDTO>>(OperationStatus.Ok, "", tickets));

            // Act
            var result = await _controller.GetTickets(flyNumber, tenantId, departure, originIATA, destinationIATA);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetAllApplicableDiscounts_WithValidTicketNumber_ReturnsOkResult()
        {
            // Arrange
            var ticketNumber = "TKT123";
            var discounts = new List<DiscountDTO>
            {
                new DiscountDTO { DiscountId = Guid.NewGuid(), Name = "Test Discount", Value = 10m }
            };

            _ticketServiceMock
                .Setup(x => x.GetAllApplicableDiscountsAsync(ticketNumber))
                .ReturnsAsync(new OperationResult<IEnumerable<DiscountDTO>>(OperationStatus.Ok, "", discounts));

            // Act
            var result = await _controller.GetAllApplicableDiscounts(ticketNumber);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetAllDiscounts_ReturnsOkResult()
        {
            // Arrange
            var discounts = new List<DiscountDTO>
            {
                new DiscountDTO { DiscountId = Guid.NewGuid(), Name = "Test Discount", Value = 10m }
            };

            _ticketServiceMock
                .Setup(x => x.GetAllDiscountsAsync())
                .ReturnsAsync(new OperationResult<IEnumerable<DiscountDTO>>(OperationStatus.Ok, "", discounts));

            // Act
            var result = await _controller.GetAllDiscounts();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        private TicketDTO CreateTestTicketDto()
        {
            return new TicketDTO
            {
                TicketId = Guid.NewGuid(),
                TicketNumber = "TKT123",
                Price = 100.00m,
                Discount = 0m,
                Status = TicketStatus.Reserved,
                FlightSeat = new FlightSeatDTO
                {
                    FlightSeatId = Guid.NewGuid(),
                    SeatNumber = "1A",
                    IsAvailable = false
                },
                Tenant = new TenantDTO
                {
                    TenantId = Guid.NewGuid(),
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@test.com",
                    PhoneNumber = "1234567890",
                    Group = TenantGroup.GroupA
                }
            };
        }
    }
}
