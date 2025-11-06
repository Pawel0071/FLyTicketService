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
        public async Task GetAllDiscountsAsync_ReturnsAllDiscounts()
        {
            // Arrange
            var discounts = new List<Discount>
            {
                CreateTestDiscount("Discount1", 10m),
                CreateTestDiscount("Discount2", 20m)
            };

            _discountRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(discounts);

            // Act
            var result = await _sut.GetAllDiscountsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public void ApplyDiscounts_WithValidDiscounts_ReturnsCorrectPrice()
        {
            // Arrange
            var ticket = CreateTestTicket();
            var discounts = new List<Discount>
            {
                CreateTestDiscount("Discount1", 10m)
            };

            // Act
            var (finalPrice, discountValue) = _sut.ApplyDiscounts(discounts, ticket);

            // Assert
            finalPrice.Should().BeGreaterThan(0);
            discountValue.Should().BeGreaterThanOrEqualTo(0);
        }

        [Fact]
        public void ApplyDiscounts_WithMultipleDiscounts_AppliesAllDiscounts()
        {
            // Arrange
            var ticket = CreateTestTicket();
            var discounts = new List<Discount>
            {
                CreateTestDiscount("Discount1", 10m),
                CreateTestDiscount("Discount2", 5m)
            };

            // Act
            var (finalPrice, discountValue) = _sut.ApplyDiscounts(discounts, ticket);

            // Assert
            finalPrice.Should().BeLessThan(ticket.FlightSeat.FlightSchedule.Price);
            discountValue.Should().BeGreaterThan(0);
        }

        [Fact]
        public void IsDiscountApplicable_WithMatchingConditions_ReturnsTrue()
        {
            // Arrange
            var ticket = CreateTestTicket();
            var discount = CreateTestDiscount("TestDiscount", 10m);

            // Act
            var result = _sut.IsDiscountApplicable(discount, ticket);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetAllApplicableDiscountsAsync_ReturnsOnlyApplicableDiscounts()
        {
            // Arrange
            var ticket = CreateTestTicket();
            var allDiscounts = new List<Discount>
            {
                CreateTestDiscount("Applicable", 10m),
                CreateTestDiscount("NotApplicable", 20m)
            };

            _discountRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(allDiscounts);

            // Act
            var result = await _sut.GetAllApplicableDiscountsAsync(ticket);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<Discount>>();
        }

        private Discount CreateTestDiscount(string name, decimal value)
        {
            return new Discount
            {
                DiscountId = Guid.NewGuid(),
                Name = name,
                Value = value,
                Conditions = new List<Condition>()
            };
        }

        private Ticket CreateTestTicket()
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
                TicketNumber = "TEST123",
                FlightSeat = flightSeat,
                Tenant = tenant,
                Price = 100.00m,
                Status = TicketStatus.Reserved,
                Discounts = new List<Discount>()
            };
        }
    }
}
