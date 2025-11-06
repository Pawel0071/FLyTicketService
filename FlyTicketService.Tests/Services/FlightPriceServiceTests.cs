using FluentAssertions;
using FLyTicketService.Model;
using FLyTicketService.Model.Enums;
using FLyTicketService.Repositories.Interfaces;
using FLyTicketService.Service;
using Moq;
using Xunit;

namespace FlyTicketService.Tests.Services
{
    public class FlightPriceServiceTests
    {
        private readonly Mock<IGenericRepository<Discount>> _discountRepositoryMock;
        private readonly FlightPriceService _sut;

        public FlightPriceServiceTests()
        {
            _discountRepositoryMock = new Mock<IGenericRepository<Discount>>();
            _sut = new FlightPriceService(_discountRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllDiscountsAsync_ReturnsDiscounts()
        {
            // Arrange
            var discounts = new List<Discount>
            {
                new Discount
                {
                    DiscountId = Guid.NewGuid(),
                    Name = "EarlyBird",
                    Value = 10,
                    Description = "Early booking discount"
                }
            };

            _discountRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(discounts);

            // Act
            var result = await _sut.GetAllDiscountsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }

        [Fact]
        public void ApplyDiscounts_WithValidTicket_ReturnsPrice()
        {
            // Arrange
            var ticket = CreateTestTicket();
            var discounts = new List<Discount>();

            // Act
            var (price, discount) = _sut.ApplyDiscounts(discounts, ticket);

            // Assert
            price.Should().BeGreaterThanOrEqualTo(0);
        }

        private Ticket CreateTestTicket()
        {
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
                    FlightSchedule = CreateTestFlightSchedule()
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
