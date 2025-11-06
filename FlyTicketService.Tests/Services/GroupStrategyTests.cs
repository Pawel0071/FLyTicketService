using FluentAssertions;
using FLyTicketService.Model;
using FLyTicketService.Model.Enums;
using FLyTicketService.Service;
using FLyTicketService.Service.Interfaces;
using Moq;
using Xunit;

namespace FlyTicketService.Tests.Services
{
    public class GroupStrategyTests
    {
        [Fact]
        public void GroupAStrategy_AppliesCorrectDiscount()
        {
            // Arrange
            var flightPriceServiceMock = new Mock<IFlightPriceService>();
            flightPriceServiceMock
                .Setup(x => x.ApplyDiscounts(It.IsAny<IEnumerable<Discount>>(), It.IsAny<Ticket>()))
                .Returns((100m, 0m));
            
            var strategy = new GroupAStrategy(flightPriceServiceMock.Object);
            var ticket = CreateTestTicket();
            var discounts = new List<Discount>();

            // Act
            var (price, discount) = strategy.ApplyDiscountBasedOnTenantGroup(discounts, ticket);

            // Assert
            price.Should().Be(100m);
            discount.Should().Be(0m);
        }

        [Fact]
        public void GroupBStrategy_AppliesCorrectDiscount()
        {
            // Arrange
            var flightPriceServiceMock = new Mock<IFlightPriceService>();
            flightPriceServiceMock
                .Setup(x => x.ApplyDiscounts(It.IsAny<IEnumerable<Discount>>(), It.IsAny<Ticket>()))
                .Returns((100m, 0m));
            
            var strategy = new GroupBStrategy(flightPriceServiceMock.Object);
            var ticket = CreateTestTicket();
            var discounts = new List<Discount>();

            // Act
            var (price, discount) = strategy.ApplyDiscountBasedOnTenantGroup(discounts, ticket);

            // Assert
            price.Should().Be(100m);
            discount.Should().Be(0m);
        }

        private Ticket CreateTestTicket()
        {
            var flightSchedule = CreateTestFlightSchedule();
            
            return new Ticket
            {
                TicketId = Guid.NewGuid(),
                TicketNumber = "TEST123",
                Price = 100,
                Status = TicketStatus.Reserved,
                Tenant = new Tenant
                {
                    TenantId = Guid.NewGuid(),
                    Name = "John Doe",
                    Address = "123 Main St",
                    Group = TenantGroup.GroupA,
                    Birthday = DateTime.Now.AddYears(-30)
                },
                FlightSeat = new FlightSeat
                {
                    FlightSeatId = Guid.NewGuid(),
                    SeatNumber = "1A",
                    IsAvailable = true,
                    FlightSchedule = flightSchedule
                }
            };
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
