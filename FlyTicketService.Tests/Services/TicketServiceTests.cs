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
using Xunit.Abstractions;

namespace FlyTicketService.Tests.Services
{
    public class TicketServiceTests
    {
        private readonly Mock<IGenericRepository<Ticket>> _ticketRepositoryMock;
        private readonly Mock<IGenericRepository<FlightSeat>> _flightSeatRepositoryMock;
        private readonly Mock<IGenericRepository<FlightSchedule>> _flightScheduleRepositoryMock;
        private readonly Mock<IGenericRepository<Tenant>> _tenantRepositoryMock;
        private readonly Mock<IGroupStrategy> _groupStrategyMock;
        private readonly Mock<IFlightPriceService> _flightPriceServiceMock;
        private readonly Mock<IGroupStrategyFactory> _groupStrategyFactoryMock;
        private readonly Mock<ILogger<TicketService>> _loggerMock;
        private readonly TicketService _sut;
        private readonly ITestOutputHelper _output;

        public TicketServiceTests(ITestOutputHelper output)
        {
            _output = output;
            _ticketRepositoryMock = new Mock<IGenericRepository<Ticket>>();
            _flightSeatRepositoryMock = new Mock<IGenericRepository<FlightSeat>>();
            _flightScheduleRepositoryMock = new Mock<IGenericRepository<FlightSchedule>>();
            _tenantRepositoryMock = new Mock<IGenericRepository<Tenant>>();
            _groupStrategyMock = new Mock<IGroupStrategy>();
            _flightPriceServiceMock = new Mock<IFlightPriceService>();
            _groupStrategyFactoryMock = new Mock<IGroupStrategyFactory>();
            _loggerMock = new Mock<ILogger<TicketService>>();
            
            _groupStrategyFactoryMock
                .Setup(x => x.GetStrategy(It.IsAny<TenantGroup>()))
                .Returns(_groupStrategyMock.Object);

            _groupStrategyMock
                .Setup(x => x.ApplyDiscountBasedOnTenantGroup(It.IsAny<IEnumerable<Discount>>(), It.IsAny<Ticket>()))
                .Returns((100m, 0m));
            
            _sut = new TicketService(
                _ticketRepositoryMock.Object,
                _loggerMock.Object,
                _groupStrategyMock.Object,
                _flightSeatRepositoryMock.Object,
                _tenantRepositoryMock.Object,
                _flightPriceServiceMock.Object,
                _groupStrategyFactoryMock.Object,
                _flightScheduleRepositoryMock.Object);
        }

        [Fact]
        public async Task ReserveTicketAsync_WithValidData_ReturnsTicket()
        {
            // Arrange
            var flightId = "FL123";
            var seatNo = "1A";
            var tenantId = Guid.NewGuid();

            var tenant = new Tenant
            {
                TenantId = tenantId,
                Name = "John Doe",
                Address = "123 Main St",
                Group = TenantGroup.GroupA,
                Birthday = DateTime.Now.AddYears(-30)
            };

            var flightSchedule = CreateTestFlightSchedule();
            flightSchedule.FlightId = flightId;
            
            var flightSeat = new FlightSeat
            {
                FlightSeatId = Guid.NewGuid(),
                SeatNumber = seatNo,
                IsAvailable = true,
                FlightSchedule = flightSchedule,
                FlightScheduleId = flightSchedule.FlightScheduleId
            };

            // Add seat to flight schedule
            flightSchedule.Seats.Add(flightSeat);

            _tenantRepositoryMock
                .Setup(x => x.GetByIdAsync(tenantId))
                .ReturnsAsync(tenant);

            _flightScheduleRepositoryMock
                .Setup(x => x.GetByAsync(It.IsAny<Func<FlightSchedule, bool>>()))
                .ReturnsAsync(flightSchedule);

            _flightSeatRepositoryMock
                .Setup(x => x.GetByAsync(It.IsAny<Func<FlightSeat, bool>>()))
                .ReturnsAsync(flightSeat);

            _ticketRepositoryMock
                .Setup(x => x.GetByAsync(It.IsAny<Func<Ticket, bool>>()))
                .ReturnsAsync((Ticket?)null);

            _ticketRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Ticket>()))
                .ReturnsAsync(true);

            // Act
            var result = await _sut.ReserveTicketAsync(flightId, seatNo, tenantId);

            // Assert
            _output.WriteLine($"Status: {result.Status}, Message: {result.Message}");
            result.Should().NotBeNull();
            result.Should().NotBeNull();
            result.Status.Should().BeOneOf(OperationStatus.Ok, OperationStatus.Created);
        }

        private FlightSchedule CreateTestFlightSchedule()
        {
            return new FlightSchedule
            {
                FlightScheduleId = Guid.NewGuid(),
                FlightId = "FL123",
                Price = 100,
                Departure = DateTimeOffset.Now.AddDays(7),
                Arrival = DateTimeOffset.Now.AddDays(7).AddHours(2),
                Airline = new Airline
                {
                    AirlineId = Guid.NewGuid(),
                    AirlineName = "Test Airlines",
                    IATA = "TA",
                    Country = "USA"
                },
                Aircraft = new Aircraft
                {
                    AircraftId = Guid.NewGuid(),
                    Model = "Boeing 737",
                    RegistrationNumber = "N12345"
                },
                Origin = new Airport
                {
                    AirportId = Guid.NewGuid(),
                    AirportName = "Test Airport",
                    City = "New York",
                    Country = "USA",
                    IATA = "JFK",
                    ICAO = "KJFK",
                    Continent = "North America",
                    Timezone = SimplyTimeZone.EST
                },
                Destination = new Airport
                {
                    AirportId = Guid.NewGuid(),
                    AirportName = "Test Airport 2",
                    City = "Los Angeles",
                    Country = "USA",
                    IATA = "LAX",
                    ICAO = "KLAX",
                    Continent = "North America",
                    Timezone = SimplyTimeZone.PST
                },
                Seats = new List<FlightSeat>()
            };
        }
    }
}
