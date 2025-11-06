using FluentAssertions;
using FLyTicketService.DTO;
using FLyTicketService.Model;
using FLyTicketService.Model.Enums;
using FLyTicketService.Repositories.Interfaces;
using FLyTicketService.Services;
using FLyTicketService.Shared;
using Microsoft.Extensions.Logging;
using Moq;

using Xunit;

namespace FlyTicketService.Tests.Services
{
    public class FlightScheduleServiceTests
    {
        private readonly Mock<IGenericRepository<FlightSchedule>> _flightScheduleRepositoryMock;
        private readonly Mock<IGenericRepository<Aircraft>> _aircraftRepositoryMock;
        private readonly Mock<IGenericRepository<Airline>> _airlineRepositoryMock;
        private readonly Mock<IGenericRepository<Airport>> _airportRepositoryMock;
        private readonly Mock<ILogger<FlightScheduleService>> _loggerMock;
        private readonly FlightScheduleService _sut;

        public FlightScheduleServiceTests()
        {
            _flightScheduleRepositoryMock = new Mock<IGenericRepository<FlightSchedule>>();
            _aircraftRepositoryMock = new Mock<IGenericRepository<Aircraft>>();
            _airlineRepositoryMock = new Mock<IGenericRepository<Airline>>();
            _airportRepositoryMock = new Mock<IGenericRepository<Airport>>();
            _loggerMock = new Mock<ILogger<FlightScheduleService>>();
            
            _sut = new FlightScheduleService(
                _flightScheduleRepositoryMock.Object,
                _aircraftRepositoryMock.Object,
                _airlineRepositoryMock.Object,
                _airportRepositoryMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task GetFlightAsync_WhenFlightExists_ReturnsFlight()
        {
            // Arrange
            var flightId = "FL123";
            var flight = CreateTestFlightSchedule();
            flight.FlightId = flightId;

            _flightScheduleRepositoryMock
                .Setup(x => x.GetByAsync(It.IsAny<Func<FlightSchedule, bool>>()))
                .ReturnsAsync(flight);

            // Act
            var result = await _sut.GetFlightAsync(flightId);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(OperationStatus.Ok);
            result.Result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetAllFlightsAsync_ReturnsAllFlights()
        {
            // Arrange
            var flights = new List<FlightSchedule>
            {
                CreateTestFlightSchedule(),
                CreateTestFlightSchedule()
            };

            _flightScheduleRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(flights);

            // Act
            var result = await _sut.GetAllFlightsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(OperationStatus.Ok);
            result.Result.Should().HaveCount(2);
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
