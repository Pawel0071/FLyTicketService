using FluentAssertions;
using FLyTicketService.DTO;
using FLyTicketService.Model;
using FLyTicketService.Model.Enums;
using FLyTicketService.Repositories.Interfaces;
using FLyTicketService.Service;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FlyTicketService.Tests.Services
{
    public class FlightScheduleServiceTests
    {
        private readonly Mock<IGenericRepository<FlightSchedule>> _flightScheduleRepositoryMock;
        private readonly Mock<ILogger<FlightScheduleService>> _loggerMock;
        private readonly FlightScheduleService _sut;

        public FlightScheduleServiceTests()
        {
            _flightScheduleRepositoryMock = new Mock<IGenericRepository<FlightSchedule>>();
            _loggerMock = new Mock<ILogger<FlightScheduleService>>();
            _sut = new FlightScheduleService(_flightScheduleRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetFlightScheduleAsync_WhenFlightExists_ReturnsFlight()
        {
            // Arrange
            var flightNumber = "FL123";
            var flightSchedule = CreateTestFlightSchedule(flightNumber);

            _flightScheduleRepositoryMock
                .Setup(x => x.GetByExpressionAsync(It.IsAny<System.Linq.Expressions.Expression<Func<FlightSchedule, bool>>>()))
                .ReturnsAsync(new[] { flightSchedule });

            // Act
            var result = await _sut.GetFlightScheduleAsync(flightNumber);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(FLyTicketService.Shared.OperationStatus.Ok);
            result.Data.Should().NotBeNull();
            result.Data!.FlightNumber.Should().Be(flightNumber);
        }

        [Fact]
        public async Task GetFlightScheduleAsync_WhenFlightDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var flightNumber = "FL999";

            _flightScheduleRepositoryMock
                .Setup(x => x.GetByExpressionAsync(It.IsAny<System.Linq.Expressions.Expression<Func<FlightSchedule, bool>>>()))
                .ReturnsAsync(new List<FlightSchedule>());

            // Act
            var result = await _sut.GetFlightScheduleAsync(flightNumber);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(FLyTicketService.Shared.OperationStatus.NotFound);
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task GetFlightScheduleListAsync_WithFilters_ReturnsFilteredFlights()
        {
            // Arrange
            var originIATA = "WAW";
            var destinationIATA = "JFK";
            var departureDate = DateTimeOffset.UtcNow.AddDays(7);
            var flights = new List<FlightSchedule>
            {
                CreateTestFlightSchedule("FL123"),
                CreateTestFlightSchedule("FL456")
            };

            _flightScheduleRepositoryMock
                .Setup(x => x.GetByExpressionAsync(It.IsAny<System.Linq.Expressions.Expression<Func<FlightSchedule, bool>>>()))
                .ReturnsAsync(flights);

            // Act
            var result = await _sut.GetFlightScheduleListAsync(originIATA, destinationIATA, departureDate);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(FLyTicketService.Shared.OperationStatus.Ok);
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task GetFlightScheduleListAsync_WithNoFilters_ReturnsAllFlights()
        {
            // Arrange
            var flights = new List<FlightSchedule>
            {
                CreateTestFlightSchedule("FL123"),
                CreateTestFlightSchedule("FL456"),
                CreateTestFlightSchedule("FL789")
            };

            _flightScheduleRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(flights);

            // Act
            var result = await _sut.GetFlightScheduleListAsync(null, null, null);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(FLyTicketService.Shared.OperationStatus.Ok);
            result.Data.Should().HaveCount(3);
        }

        [Fact]
        public async Task CreateFlightScheduleAsync_WithValidData_CreatesSuccessfully()
        {
            // Arrange
            var flightDto = CreateTestFlightScheduleDto();

            _flightScheduleRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<FlightSchedule>()))
                .ReturnsAsync(true);

            // Act
            var result = await _sut.CreateFlightScheduleAsync(flightDto);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(FLyTicketService.Shared.OperationStatus.Ok);
            _flightScheduleRepositoryMock.Verify(x => x.AddAsync(It.IsAny<FlightSchedule>()), Times.Once);
        }

        [Fact]
        public async Task UpdateFlightScheduleAsync_WithValidData_UpdatesSuccessfully()
        {
            // Arrange
            var flightId = Guid.NewGuid();
            var flightDto = CreateTestFlightScheduleDto();
            var existingFlight = CreateTestFlightSchedule("FL123");
            existingFlight.FlightScheduleId = flightId;

            _flightScheduleRepositoryMock
                .Setup(x => x.GetByIdAsync(flightId))
                .ReturnsAsync(existingFlight);

            _flightScheduleRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<FlightSchedule>()))
                .ReturnsAsync(true);

            // Act
            var result = await _sut.UpdateFlightScheduleAsync(flightId, flightDto);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(FLyTicketService.Shared.OperationStatus.Ok);
        }

        [Fact]
        public async Task DeleteFlightScheduleAsync_WhenFlightExists_DeletesSuccessfully()
        {
            // Arrange
            var flightId = Guid.NewGuid();
            var flight = CreateTestFlightSchedule("FL123");
            flight.FlightScheduleId = flightId;

            _flightScheduleRepositoryMock
                .Setup(x => x.GetByIdAsync(flightId))
                .ReturnsAsync(flight);

            _flightScheduleRepositoryMock
                .Setup(x => x.DeleteAsync(flight))
                .ReturnsAsync(true);

            // Act
            var result = await _sut.DeleteFlightScheduleAsync(flightId);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(FLyTicketService.Shared.OperationStatus.Ok);
            result.Data.Should().BeTrue();
        }

        [Fact]
        public async Task GetAvailableSeatsAsync_ReturnsOnlyAvailableSeats()
        {
            // Arrange
            var flightNumber = "FL123";
            var flightSchedule = CreateTestFlightSchedule(flightNumber);
            flightSchedule.FlightSeats = new List<FlightSeat>
            {
                new FlightSeat { FlightSeatId = Guid.NewGuid(), SeatNumber = "1A", IsAvailable = true, FlightSchedule = flightSchedule },
                new FlightSeat { FlightSeatId = Guid.NewGuid(), SeatNumber = "1B", IsAvailable = false, FlightSchedule = flightSchedule },
                new FlightSeat { FlightSeatId = Guid.NewGuid(), SeatNumber = "1C", IsAvailable = true, FlightSchedule = flightSchedule }
            };

            _flightScheduleRepositoryMock
                .Setup(x => x.GetByExpressionAsync(It.IsAny<System.Linq.Expressions.Expression<Func<FlightSchedule, bool>>>()))
                .ReturnsAsync(new[] { flightSchedule });

            // Act
            var result = await _sut.GetAvailableSeatsAsync(flightNumber);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(FLyTicketService.Shared.OperationStatus.Ok);
            result.Data.Should().HaveCount(2);
        }

        private FlightSchedule CreateTestFlightSchedule(string flightNumber)
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
                IATA = "WAW",
                City = "Warsaw",
                Country = "Poland",
                TimeZone = new SimplyTimeZoneInfo { TimeZone = SimplyTimeZone.UTC }
            };

            var destinationAirport = new Airport
            {
                AirportId = Guid.NewGuid(),
                Name = "Destination Airport",
                IATA = "JFK",
                City = "New York",
                Country = "USA",
                TimeZone = new SimplyTimeZoneInfo { TimeZone = SimplyTimeZone.UTC }
            };

            return new FlightSchedule
            {
                FlightScheduleId = Guid.NewGuid(),
                FlightNumber = flightNumber,
                Airline = airline,
                Aircraft = aircraft,
                Origin = originAirport,
                Destination = destinationAirport,
                Departure = DateTimeOffset.UtcNow.AddDays(7),
                Arrival = DateTimeOffset.UtcNow.AddDays(7).AddHours(2),
                Price = 100.00m,
                FlightSeats = new List<FlightSeat>()
            };
        }

        private FlightScheduleDTO CreateTestFlightScheduleDto()
        {
            return new FlightScheduleDTO
            {
                FlightScheduleId = Guid.NewGuid(),
                FlightNumber = "FL123",
                Departure = DateTimeOffset.UtcNow.AddDays(7),
                Arrival = DateTimeOffset.UtcNow.AddDays(7).AddHours(2),
                Price = 100.00m
            };
        }
    }
}
