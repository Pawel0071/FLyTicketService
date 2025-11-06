using FluentAssertions;
using FLyTicketService.DTO;
using FLyTicketService.Model;
using FLyTicketService.Model.Enums;
using FLyTicketService.Repositories.Interfaces;
using FLyTicketService.Service;
using FLyTicketService.Service.Interfaces;
using FLyTicketService.Shared;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FlyTicketService.Tests.Services
{
    public class TicketServiceTests
    {
        private readonly Mock<IGenericRepository<Ticket>> _ticketRepositoryMock;
        private readonly Mock<IGenericRepository<Tenant>> _tenantRepositoryMock;
        private readonly Mock<IGenericRepository<FlightSeat>> _flightSeatRepositoryMock;
        private readonly Mock<IGenericRepository<FlightSchedule>> _flightScheduleRepositoryMock;
        private readonly Mock<IFlightPriceService> _flightPriceServiceMock;
        private readonly Mock<IGroupStrategyFactory> _strategyFactoryMock;
        private readonly Mock<IGroupStrategy> _groupStrategyMock;
        private readonly Mock<ILogger<TicketService>> _loggerMock;
        private readonly TicketService _sut;

        public TicketServiceTests()
        {
            _ticketRepositoryMock = new Mock<IGenericRepository<Ticket>>();
            _tenantRepositoryMock = new Mock<IGenericRepository<Tenant>>();
            _flightSeatRepositoryMock = new Mock<IGenericRepository<FlightSeat>>();
            _flightScheduleRepositoryMock = new Mock<IGenericRepository<FlightSchedule>>();
            _flightPriceServiceMock = new Mock<IFlightPriceService>();
            _strategyFactoryMock = new Mock<IGroupStrategyFactory>();
            _groupStrategyMock = new Mock<IGroupStrategy>();
            _loggerMock = new Mock<ILogger<TicketService>>();

            _sut = new TicketService(
                _ticketRepositoryMock.Object,
                _loggerMock.Object,
                _groupStrategyMock.Object,
                _flightSeatRepositoryMock.Object,
                _tenantRepositoryMock.Object,
                _flightPriceServiceMock.Object,
                _strategyFactoryMock.Object,
                _flightScheduleRepositoryMock.Object
            );
        }

        [Fact]
        public async Task GetTicketAsync_WhenTicketExists_ReturnsTicket()
        {
            // Arrange
            var ticketNumber = "TEST123";
            var ticket = CreateTestTicket(ticketNumber);

            _ticketRepositoryMock
                .Setup(x => x.GetByExpressionAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Ticket, bool>>>()))
                .ReturnsAsync(new[] { ticket });

            // Act
            var result = await _sut.GetTicketAsync(ticketNumber);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(OperationStatus.Ok);
            result.Data.Should().NotBeNull();
            result.Data!.TicketNumber.Should().Be(ticketNumber);
        }

        [Fact]
        public async Task GetTicketAsync_WhenTicketDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var ticketNumber = "NONEXISTENT";

            _ticketRepositoryMock
                .Setup(x => x.GetByExpressionAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Ticket, bool>>>()))
                .ReturnsAsync(new List<Ticket>());

            // Act
            var result = await _sut.GetTicketAsync(ticketNumber);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(OperationStatus.NotFound);
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task CancelTicketAsync_WhenTicketIsReserved_CancelsSuccessfully()
        {
            // Arrange
            var ticketNumber = "TEST123";
            var ticket = CreateTestTicket(ticketNumber);
            ticket.Status = TicketStatus.Reserved;

            _ticketRepositoryMock
                .Setup(x => x.GetByExpressionAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Ticket, bool>>>()))
                .ReturnsAsync(new[] { ticket });

            _ticketRepositoryMock
                .Setup(x => x.DeleteAsync(It.IsAny<Ticket>()))
                .ReturnsAsync(true);

            // Act
            var result = await _sut.CancelTicketAsync(ticketNumber);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(OperationStatus.Ok);
            result.Data.Should().BeTrue();
        }

        [Fact]
        public async Task CancelTicketAsync_WhenTicketIsSold_ReturnsConflict()
        {
            // Arrange
            var ticketNumber = "TEST123";
            var ticket = CreateTestTicket(ticketNumber);
            ticket.Status = TicketStatus.Sold;

            _ticketRepositoryMock
                .Setup(x => x.GetByExpressionAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Ticket, bool>>>()))
                .ReturnsAsync(new[] { ticket });

            // Act
            var result = await _sut.CancelTicketAsync(ticketNumber);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(OperationStatus.Conflict);
            result.Data.Should().BeFalse();
        }

        private Ticket CreateTestTicket(string ticketNumber)
        {
            var airline = new Airline
            {
                AirlineId = Guid.NewGuid(),
                Name = "Test Airlines",
                IATA = "TA",
                Country = "Test Country"
            };

            var aircraft = new Aircraft
            {
                AircraftId = Guid.NewGuid(),
                Model = "Test Model",
                Airline = airline
            };

            var originAirport = new Airport
            {
                AirportId = Guid.NewGuid(),
                Name = "Origin Airport",
                IATA = "ORG",
                City = "Origin City",
                Country = "Origin Country",
                TimeZone = new SimplyTimeZoneInfo { TimeZone = SimplyTimeZone.UTC }
            };

            var destinationAirport = new Airport
            {
                AirportId = Guid.NewGuid(),
                Name = "Destination Airport",
                IATA = "DST",
                City = "Destination City",
                Country = "Destination Country",
                TimeZone = new SimplyTimeZoneInfo { TimeZone = SimplyTimeZone.UTC }
            };

            var flightSchedule = new FlightSchedule
            {
                FlightScheduleId = Guid.NewGuid(),
                FlightNumber = "TEST001",
                Airline = airline,
                Aircraft = aircraft,
                Origin = originAirport,
                Destination = destinationAirport,
                Departure = DateTimeOffset.UtcNow.AddDays(7),
                Arrival = DateTimeOffset.UtcNow.AddDays(7).AddHours(2),
                Price = 100.00m,
                FlightSeats = new List<FlightSeat>()
            };

            var flightSeat = new FlightSeat
            {
                FlightSeatId = Guid.NewGuid(),
                SeatNumber = "1A",
                FlightSchedule = flightSchedule,
                IsAvailable = true
            };

            var tenant = new Tenant
            {
                TenantId = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@test.com",
                PhoneNumber = "1234567890",
                Group = TenantGroup.GroupA
            };

            return new Ticket
            {
                TicketId = Guid.NewGuid(),
                TicketNumber = ticketNumber,
                FlightSeat = flightSeat,
                Tenant = tenant,
                Price = 100.00m,
                Status = TicketStatus.Reserved,
                Discounts = new List<Discount>()
            };
        }
    }
}
