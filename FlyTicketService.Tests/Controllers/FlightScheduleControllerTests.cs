using FluentAssertions;
using FLyTicketService.Controllers;
using FLyTicketService.DTO;
using FLyTicketService.Model.Enums;
using FLyTicketService.Service.Interfaces;
using FLyTicketService.Services.Interfaces;
using FLyTicketService.Shared;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FlyTicketService.Tests.Controllers
{
    public class FlightScheduleControllerTests
    {
        private readonly Mock<IFlightScheduleService> _flightScheduleServiceMock;
        private readonly FlightScheduleController _controller;

        public FlightScheduleControllerTests()
        {
            _flightScheduleServiceMock = new Mock<IFlightScheduleService>();
            _controller = new FlightScheduleController(_flightScheduleServiceMock.Object);
        }

        [Fact]
        public async Task GetFlight_WithValidId_ReturnsOk()
        {
            // Arrange
            var flightId = "LH123";
            var flightDto = new FlightScheduleFullDTO
            {
                FlightScheduleId = Guid.NewGuid(),
                FlightId = flightId,
                Departure = DateTimeOffset.Now.AddDays(1),
                Arrival = DateTimeOffset.Now.AddDays(1).AddHours(2),
                Price = 299.99m,
                Origin = new AirportDTO(),
                Destination = new AirportDTO(),
                Airline = new AirlineDTO(),
                Aircraft = new AircraftDTO(),
                Seats = new List<FlightSeatDTO>()
            };

            var operationResult = new OperationResult<FlightScheduleFullDTO?>(
                OperationStatus.Ok,
                "Flight found",
                flightDto);

            _flightScheduleServiceMock
                .Setup(x => x.GetFlightAsync(flightId))
                .ReturnsAsync(operationResult);

            // Act
            var result = await _controller.GetFlight(flightId);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult!.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetFlight_WhenNotFound_ReturnsNotFound()
        {
            // Arrange
            var flightId = "INVALID";

            var operationResult = new OperationResult<FlightScheduleFullDTO?>(
                OperationStatus.NotFound,
                "Flight not found",
                null);

            _flightScheduleServiceMock
                .Setup(x => x.GetFlightAsync(flightId))
                .ReturnsAsync(operationResult);

            // Act
            var result = await _controller.GetFlight(flightId);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult!.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task GetAllFlights_ReturnsOkWithList()
        {
            // Arrange
            var flights = new List<FlightScheduleFullDTO>
            {
                new FlightScheduleFullDTO 
                { 
                    FlightScheduleId = Guid.NewGuid(),
                    FlightId = "LH123",
                    Price = 299.99m,
                    Departure = DateTimeOffset.Now.AddDays(1),
                    Arrival = DateTimeOffset.Now.AddDays(1).AddHours(2),
                    Origin = new AirportDTO(),
                    Destination = new AirportDTO(),
                    Airline = new AirlineDTO(),
                    Aircraft = new AircraftDTO(),
                    Seats = new List<FlightSeatDTO>()
                },
                new FlightScheduleFullDTO 
                { 
                    FlightScheduleId = Guid.NewGuid(),
                    FlightId = "BA456",
                    Price = 399.99m,
                    Departure = DateTimeOffset.Now.AddDays(2),
                    Arrival = DateTimeOffset.Now.AddDays(2).AddHours(3),
                    Origin = new AirportDTO(),
                    Destination = new AirportDTO(),
                    Airline = new AirlineDTO(),
                    Aircraft = new AircraftDTO(),
                    Seats = new List<FlightSeatDTO>()
                }
            };

            var operationResult = new OperationResult<IEnumerable<FlightScheduleFullDTO>>(
                OperationStatus.Ok,
                "Flights retrieved",
                flights);

            _flightScheduleServiceMock
                .Setup(x => x.GetAllFlightsAsync())
                .ReturnsAsync(operationResult);

            // Act
            var result = await _controller.GetAllFlights();

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult!.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task ScheduleFlight_WithValidData_ReturnsOk()
        {
            // Arrange
            var flightDto = new FlightScheduleDTO
            {
                AirlineIATA = "LH",
                AircraftRegistrationNumber = "D-ABCD",
                Departure = DateTime.Now.AddDays(1),
                Arrival = DateTime.Now.AddDays(1).AddHours(2),
                OriginIATA = "FRA",
                DestinationIATA = "MUC",
                Price = 299.99m
            };

            var operationResult = new OperationResult<bool>(
                OperationStatus.Ok,
                "Flight scheduled successfully",
                true);

            _flightScheduleServiceMock
                .Setup(x => x.ScheduleFlightAsync(flightDto))
                .ReturnsAsync(operationResult);

            // Act
            var result = await _controller.ScheduleFlight(flightDto);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult!.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task ScheduleFlight_WithInvalidData_ReturnsBadRequest()
        {
            // Arrange
            var flightDto = new FlightScheduleDTO
            {
                AirlineIATA = "LH",
                AircraftRegistrationNumber = "D-ABCD",
                Departure = DateTime.Now.AddDays(1),
                Arrival = DateTime.Now.AddDays(1).AddHours(2),
                OriginIATA = "FRA",
                DestinationIATA = "MUC",
                Price = 299.99m
            };

            var operationResult = new OperationResult<bool>(
                OperationStatus.BadRequest,
                "Invalid flight data",
                false);

            _flightScheduleServiceMock
                .Setup(x => x.ScheduleFlightAsync(flightDto))
                .ReturnsAsync(operationResult);

            // Act
            var result = await _controller.ScheduleFlight(flightDto);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult!.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task ScheduleRecurringFlight_WithValidData_ReturnsOk()
        {
            // Arrange
            var flightDto = new FlightScheduleDTO
            {
                AirlineIATA = "LH",
                AircraftRegistrationNumber = "D-ABCD",
                Departure = DateTime.Now.AddDays(1),
                Arrival = DateTime.Now.AddDays(1).AddHours(2),
                OriginIATA = "FRA",
                DestinationIATA = "MUC",
                Price = 299.99m
            };

            var dayOfWeek = DaysOfWeekMask.Monday | DaysOfWeekMask.Wednesday | DaysOfWeekMask.Friday;
            var numberOfOccurring = 10;

            var operationResult = new OperationResult<bool>(
                OperationStatus.Ok,
                "Recurring flight scheduled successfully",
                true);

            _flightScheduleServiceMock
                .Setup(x => x.ScheduleRecurringFlightAsync(flightDto, dayOfWeek, numberOfOccurring))
                .ReturnsAsync(operationResult);

            // Act
            var result = await _controller.ScheduleRecurringFlight(flightDto, dayOfWeek, numberOfOccurring);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult!.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task ScheduleRecurringFlight_WithInvalidData_ReturnsBadRequest()
        {
            // Arrange
            var flightDto = new FlightScheduleDTO
            {
                AirlineIATA = "LH",
                AircraftRegistrationNumber = "D-ABCD",
                Departure = DateTime.Now.AddDays(1),
                Arrival = DateTime.Now.AddDays(1).AddHours(2),
                OriginIATA = "FRA",
                DestinationIATA = "MUC",
                Price = 299.99m
            };

            var dayOfWeek = DaysOfWeekMask.None;
            var numberOfOccurring = 0;

            var operationResult = new OperationResult<bool>(
                OperationStatus.BadRequest,
                "Invalid recurring flight data",
                false);

            _flightScheduleServiceMock
                .Setup(x => x.ScheduleRecurringFlightAsync(flightDto, dayOfWeek, numberOfOccurring))
                .ReturnsAsync(operationResult);

            // Act
            var result = await _controller.ScheduleRecurringFlight(flightDto, dayOfWeek, numberOfOccurring);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult!.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task UpdateFlight_WithValidData_ReturnsOk()
        {
            // Arrange
            var flightScheduleId = Guid.NewGuid();
            var flightDto = new FlightScheduleDTO
            {
                AirlineIATA = "LH",
                AircraftRegistrationNumber = "D-ABCD",
                Departure = DateTime.Now.AddDays(1),
                Arrival = DateTime.Now.AddDays(1).AddHours(2),
                OriginIATA = "FRA",
                DestinationIATA = "MUC",
                Price = 349.99m
            };

            var operationResult = new OperationResult<bool>(
                OperationStatus.Ok,
                "Flight updated successfully",
                true);

            _flightScheduleServiceMock
                .Setup(x => x.UpdateFlightAsync(flightScheduleId, flightDto, false))
                .ReturnsAsync(operationResult);

            // Act
            var result = await _controller.UpdateFlight(flightScheduleId, flightDto, false);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult!.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task UpdateFlight_WhenNotFound_ReturnsNotFound()
        {
            // Arrange
            var flightScheduleId = Guid.NewGuid();
            var flightDto = new FlightScheduleDTO
            {
                AirlineIATA = "LH",
                AircraftRegistrationNumber = "D-ABCD",
                Departure = DateTime.Now.AddDays(1),
                Arrival = DateTime.Now.AddDays(1).AddHours(2),
                OriginIATA = "FRA",
                DestinationIATA = "MUC",
                Price = 349.99m
            };

            var operationResult = new OperationResult<bool>(
                OperationStatus.NotFound,
                "Flight not found",
                false);

            _flightScheduleServiceMock
                .Setup(x => x.UpdateFlightAsync(flightScheduleId, flightDto, false))
                .ReturnsAsync(operationResult);

            // Act
            var result = await _controller.UpdateFlight(flightScheduleId, flightDto, false);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult!.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task DeleteFlight_WithValidId_ReturnsOk()
        {
            // Arrange
            var flightScheduleId = Guid.NewGuid();
            var flightDto = new FlightScheduleDTO
            {
                AirlineIATA = "LH",
                AircraftRegistrationNumber = "D-ABCD",
                Departure = DateTime.Now.AddDays(1),
                Arrival = DateTime.Now.AddDays(1).AddHours(2),
                OriginIATA = "FRA",
                DestinationIATA = "MUC",
                Price = 299.99m
            };

            var operationResult = new OperationResult<bool>(
                OperationStatus.Ok,
                "Flight deleted successfully",
                true);

            _flightScheduleServiceMock
                .Setup(x => x.DeleteFlightAsync(flightScheduleId, flightDto, false))
                .ReturnsAsync(operationResult);

            // Act
            var result = await _controller.DeleteFlight(flightScheduleId, flightDto, false);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult!.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task DeleteFlight_WhenNotFound_ReturnsNotFound()
        {
            // Arrange
            var flightScheduleId = Guid.NewGuid();
            var flightDto = new FlightScheduleDTO
            {
                AirlineIATA = "LH",
                AircraftRegistrationNumber = "D-ABCD",
                Departure = DateTime.Now.AddDays(1),
                Arrival = DateTime.Now.AddDays(1).AddHours(2),
                OriginIATA = "FRA",
                DestinationIATA = "MUC",
                Price = 299.99m
            };

            var operationResult = new OperationResult<bool>(
                OperationStatus.NotFound,
                "Flight not found",
                false);

            _flightScheduleServiceMock
                .Setup(x => x.DeleteFlightAsync(flightScheduleId, flightDto, false))
                .ReturnsAsync(operationResult);

            // Act
            var result = await _controller.DeleteFlight(flightScheduleId, flightDto, false);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult!.StatusCode.Should().Be(404);
        }
    }
}
