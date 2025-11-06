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
        private readonly Mock<IFlightPriceService> _flightPriceServiceMock;

        public GroupStrategyTests()
        {
            _flightPriceServiceMock = new Mock<IFlightPriceService>();
        }

        [Fact]
        public void GroupAStrategy_AppliesFullDiscounts()
        {
            // Arrange
            var strategy = new GroupAStrategy(_flightPriceServiceMock.Object);
            var ticket = CreateTestTicket();
            var discounts = CreateTestDiscounts();

            _flightPriceServiceMock
                .Setup(x => x.ApplyDiscounts(discounts, ticket))
                .Returns((90m, 10m));

            // Act
            var (price, discount) = strategy.ApplyDiscountBasedOnTenantGroup(discounts, ticket);

            // Assert
            price.Should().Be(90m);
            discount.Should().Be(10m);
        }

        [Fact]
        public void GroupAStrategy_ReturnsAllDiscounts()
        {
            // Arrange
            var strategy = new GroupAStrategy(_flightPriceServiceMock.Object);
            var discounts = CreateTestDiscounts();

            // Act
            var result = strategy.GetListBasedOnTenantGroup(discounts);

            // Assert
            result.Should().BeEquivalentTo(discounts);
            result.Should().HaveCount(discounts.Count());
        }

        [Fact]
        public void GroupBStrategy_AppliesPriceButNoDiscount()
        {
            // Arrange
            var strategy = new GroupBStrategy(_flightPriceServiceMock.Object);
            var ticket = CreateTestTicket();
            var discounts = CreateTestDiscounts();

            _flightPriceServiceMock
                .Setup(x => x.ApplyDiscounts(discounts, ticket))
                .Returns((90m, 10m));

            // Act
            var (price, discount) = strategy.ApplyDiscountBasedOnTenantGroup(discounts, ticket);

            // Assert
            price.Should().Be(90m);
            discount.Should().Be(0m); // GroupB nie pokazuje rabatu
        }

        [Fact]
        public void GroupBStrategy_ReturnsEmptyDiscountList()
        {
            // Arrange
            var strategy = new GroupBStrategy(_flightPriceServiceMock.Object);
            var discounts = CreateTestDiscounts();

            // Act
            var result = strategy.GetListBasedOnTenantGroup(discounts);

            // Assert
            result.Should().BeEmpty();
        }

        private List<Discount> CreateTestDiscounts()
        {
            return new List<Discount>
            {
                new Discount
                {
                    DiscountId = Guid.NewGuid(),
                    Name = "Test Discount 1",
                    Value = 10m,
                    Conditions = new List<Condition>()
                },
                new Discount
                {
                    DiscountId = Guid.NewGuid(),
                    Name = "Test Discount 2",
                    Value = 5m,
                    Conditions = new List<Condition>()
                }
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
