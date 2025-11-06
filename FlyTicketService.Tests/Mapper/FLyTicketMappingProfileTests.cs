using FluentAssertions;
using FLyTicketService.DTO;
using FLyTicketService.Mapper;
using FLyTicketService.Model;
using FLyTicketService.Model.Enums;
using Xunit;

namespace FlyTicketService.Tests.Mapper
{
    public class FLyTicketMappingProfileTests
    {
        [Fact]
        public void TenantToDTO_WithValidTenant_MapsAllProperties()
        {
            // Arrange
            var tenant = new Tenant
            {
                TenantId = Guid.NewGuid(),
                Name = "John Doe",
                Address = "123 Main St",
                Phone = "555-1234",
                Email = "john@example.com",
                Group = TenantGroup.GroupA,
                Birthday = new DateTime(1990, 1, 15)
            };

            // Act
            var result = tenant.ToDTO();

            // Assert
            result.Should().NotBeNull();
            result.TenantId.Should().Be(tenant.TenantId);
            result.Name.Should().Be(tenant.Name);
            result.Address.Should().Be(tenant.Address);
            result.Phone.Should().Be(tenant.Phone);
            result.Email.Should().Be(tenant.Email);
            result.Group.Should().Be(tenant.Group);
            result.Birthday.Should().Be(tenant.Birthday);
        }

        [Fact]
        public void TenantDTOToDomain_WithValidDTO_MapsAllProperties()
        {
            // Arrange
            var tenantDTO = new TenantDTO
            {
                TenantId = Guid.NewGuid(),
                Name = "Jane Smith",
                Address = "456 Oak Ave",
                Phone = "555-5678",
                Email = "jane@example.com",
                Group = TenantGroup.GroupB,
                Birthday = new DateTime(1985, 5, 20)
            };

            // Act
            var result = tenantDTO.ToDomain();

            // Assert
            result.Should().NotBeNull();
            result.TenantId.Should().Be(tenantDTO.TenantId);
            result.Name.Should().Be(tenantDTO.Name);
            result.Address.Should().Be(tenantDTO.Address);
            result.Phone.Should().Be(tenantDTO.Phone);
            result.Email.Should().Be(tenantDTO.Email);
            result.Group.Should().Be(tenantDTO.Group);
            result.Birthday.Should().Be(tenantDTO.Birthday);
        }

        [Fact]
        public void DiscountToDTO_WithValidDiscount_MapsBasicProperties()
        {
            // Arrange
            var discount = new Discount
            {
                DiscountId = Guid.NewGuid(),
                Name = "Early Bird",
                Value = 15,
                Description = "Early booking discount",
                Conditions = new List<Condition>()
            };

            // Act
            var result = discount.ToDTO();

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(discount.Name);
            result.Value.Should().Be(discount.Value);
            result.Description.Should().Be(discount.Description);
        }

        [Fact]
        public void DiscountDTOToDomain_WithValidDTO_MapsBasicProperties()
        {
            // Arrange
            var discountDTO = new DiscountDTO
            {
                Name = "Senior Discount",
                Value = 20,
                Description = "Discount for seniors",
                Conditions = new List<ConditionDTO>()
            };

            // Act
            var result = discountDTO.ToDomain();

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(discountDTO.Name);
            result.Value.Should().Be(discountDTO.Value);
            result.Description.Should().Be(discountDTO.Description);
        }

        [Fact]
        public void FlightScheduleToDTO_WithValidFlightSchedule_MapsAllProperties()
        {
            // Arrange
            var flightSchedule = new FlightSchedule
            {
                FlightScheduleId = Guid.NewGuid(),
                FlightId = "LH123",
                Departure = DateTimeOffset.Now.AddDays(7),
                Arrival = DateTimeOffset.Now.AddDays(7).AddHours(2),
                Price = 299.99m,
                Airline = new Airline
                {
                    AirlineId = Guid.NewGuid(),
                    AirlineName = "Lufthansa",
                    IATA = "LH",
                    Country = "Germany"
                },
                Aircraft = new Aircraft
                {
                    AircraftId = Guid.NewGuid(),
                    Model = "Boeing 737",
                    RegistrationNumber = "D-ABCD"
                },
                Origin = new Airport
                {
                    AirportId = Guid.NewGuid(),
                    AirportName = "Frankfurt Airport",
                    City = "Frankfurt",
                    Country = "Germany",
                    IATA = "FRA",
                    ICAO = "EDDF",
                    Timezone = SimplyTimeZone.CET,
                    Continent = "Europe"
                },
                Destination = new Airport
                {
                    AirportId = Guid.NewGuid(),
                    AirportName = "Munich Airport",
                    City = "Munich",
                    Country = "Germany",
                    IATA = "MUC",
                    ICAO = "EDDM",
                    Timezone = SimplyTimeZone.CET,
                    Continent = "Europe"
                },
                Seats = new List<FlightSeat>
                {
                    new FlightSeat
                    {
                        FlightSeatId = Guid.NewGuid(),
                        SeatNumber = "1A",
                        Class = SeatClass.Business,
                        IsAvailable = true
                    }
                }
            };

            // Act
            var result = flightSchedule.ToDTO();

            // Assert
            result.Should().NotBeNull();
            result.FlightScheduleId.Should().Be(flightSchedule.FlightScheduleId);
            result.FlightId.Should().Be(flightSchedule.FlightId);
            result.Departure.Should().Be(flightSchedule.Departure);
            result.Arrival.Should().Be(flightSchedule.Arrival);
            result.Price.Should().Be(flightSchedule.Price);
            
            result.Airline.Should().NotBeNull();
            result.Airline.AirlineName.Should().Be("Lufthansa");
            result.Airline.IATA.Should().Be("LH");
            
            result.Aircraft.Should().NotBeNull();
            result.Aircraft.Model.Should().Be("Boeing 737");
            
            result.Origin.Should().NotBeNull();
            result.Origin.IATA.Should().Be("FRA");
            result.Origin.City.Should().Be("Frankfurt");
            
            result.Destination.Should().NotBeNull();
            result.Destination.IATA.Should().Be("MUC");
            result.Destination.City.Should().Be("Munich");
            
            result.Seats.Should().HaveCount(1);
            result.Seats.First().SeatNumber.Should().Be("1A");
            result.Seats.First().Class.Should().Be(SeatClass.Business);
        }

        [Fact]
        public void TicketToDTO_WithValidTicket_MapsAllProperties()
        {
            // Arrange
            var tenant = new Tenant
            {
                TenantId = Guid.NewGuid(),
                Name = "Test User",
                Address = "Test Address",
                Group = TenantGroup.GroupA,
                Birthday = DateTime.Now.AddYears(-30)
            };

            var flightSchedule = new FlightSchedule
            {
                FlightScheduleId = Guid.NewGuid(),
                FlightId = "BA456",
                Departure = DateTimeOffset.Now.AddDays(10),
                Arrival = DateTimeOffset.Now.AddDays(10).AddHours(3),
                Price = 499.99m,
                Airline = new Airline
                {
                    AirlineId = Guid.NewGuid(),
                    AirlineName = "British Airways",
                    IATA = "BA",
                    Country = "UK"
                },
                Aircraft = new Aircraft
                {
                    AircraftId = Guid.NewGuid(),
                    Model = "Airbus A320",
                    RegistrationNumber = "G-EUUU"
                },
                Origin = new Airport
                {
                    AirportId = Guid.NewGuid(),
                    AirportName = "London Heathrow",
                    City = "London",
                    Country = "UK",
                    IATA = "LHR",
                    ICAO = "EGLL",
                    Timezone = SimplyTimeZone.GMT,
                    Continent = "Europe"
                },
                Destination = new Airport
                {
                    AirportId = Guid.NewGuid(),
                    AirportName = "Paris CDG",
                    City = "Paris",
                    Country = "France",
                    IATA = "CDG",
                    ICAO = "LFPG",
                    Timezone = SimplyTimeZone.CET,
                    Continent = "Europe"
                },
                Seats = new List<FlightSeat>()
            };

            var flightSeat = new FlightSeat
            {
                FlightSeatId = Guid.NewGuid(),
                SeatNumber = "12C",
                Class = SeatClass.Economy,
                IsAvailable = false,
                FlightSchedule = flightSchedule,
                FlightScheduleId = flightSchedule.FlightScheduleId
            };

            var ticket = new Ticket
            {
                TicketId = Guid.NewGuid(),
                TicketNumber = "TKT123456",
                Status = TicketStatus.Reserved,
                Price = 499.99m,
                Discounts = new List<Discount>(),
                ReleaseDate = DateTimeOffset.Now,
                FlightSeat = flightSeat,
                Tenant = tenant
            };

            // Act
            var result = ticket.ToDTO();

            // Assert
            result.Should().NotBeNull();
            result.TicketId.Should().Be(ticket.TicketId);
            result.TicketNumber.Should().Be(ticket.TicketNumber);
            result.FlightId.Should().Be("BA456");
            result.SeatNumber.Should().Be("12C");
            result.TenantId.Should().Be(tenant.TenantId);
            result.Tenant.Should().Be(tenant.Name);
            result.Price.Should().Be(ticket.Price);
            result.Status.Should().Be(TicketStatus.Reserved);
            result.FlightSeat.Should().NotBeNull();
            result.FlightSchedule.Should().NotBeNull();
        }

        [Fact]
        public void TenantRoundTrip_PreservesAllData()
        {
            // Arrange
            var originalTenant = new Tenant
            {
                TenantId = Guid.NewGuid(),
                Name = "Round Trip Test",
                Address = "Test Address",
                Phone = "123-456-7890",
                Email = "test@test.com",
                Group = TenantGroup.GroupB,
                Birthday = new DateTime(2000, 6, 15)
            };

            // Act
            var dto = originalTenant.ToDTO();
            var result = dto.ToDomain();

            // Assert
            result.TenantId.Should().Be(originalTenant.TenantId);
            result.Name.Should().Be(originalTenant.Name);
            result.Address.Should().Be(originalTenant.Address);
            result.Phone.Should().Be(originalTenant.Phone);
            result.Email.Should().Be(originalTenant.Email);
            result.Group.Should().Be(originalTenant.Group);
            result.Birthday.Should().Be(originalTenant.Birthday);
        }

        [Fact]
        public void DiscountRoundTrip_PreservesBasicData()
        {
            // Arrange
            var originalDiscount = new Discount
            {
                DiscountId = Guid.NewGuid(),
                Name = "Round Trip Discount",
                Value = 25,
                Description = "Test Description",
                Conditions = new List<Condition>()
            };

            // Act
            var dto = originalDiscount.ToDTO();
            var result = dto.ToDomain();

            // Assert
            result.Name.Should().Be(originalDiscount.Name);
            result.Value.Should().Be(originalDiscount.Value);
            result.Description.Should().Be(originalDiscount.Description);
        }

        [Fact]
        public void ConditionToDTO_WithValidCondition_MapsAllProperties()
        {
            // Arrange
            var condition = new Condition
            {
                ConditionId = Guid.NewGuid(),
                Category = DiscountCategory.Tenant,
                Property = "Group",
                ConditionType = DiscountCondition.Equal,
                ConditionValue = "GroupA"
            };

            // Act
            var result = condition.ToDTO();

            // Assert
            result.Should().NotBeNull();
            result.ConditionId.Should().Be(condition.ConditionId);
            result.Category.Should().Be(condition.Category);
            result.Property.Should().Be(condition.Property);
            result.Condition.Should().Be(condition.ConditionType);
            result.ConditionValue.Should().Be(condition.ConditionValue);
        }

        [Fact]
        public void ConditionDTOToDomain_WithValidDTO_MapsAllProperties()
        {
            // Arrange
            var conditionDTO = new ConditionDTO
            {
                ConditionId = Guid.NewGuid(),
                Category = DiscountCategory.Destination,
                Property = "City",
                Condition = DiscountCondition.Equal,
                ConditionValue = "Warsaw"
            };

            // Act
            var result = conditionDTO.ToDomain();

            // Assert
            result.Should().NotBeNull();
            result.ConditionId.Should().Be(conditionDTO.ConditionId);
            result.Category.Should().Be(conditionDTO.Category);
            result.Property.Should().Be(conditionDTO.Property);
            result.ConditionType.Should().Be(conditionDTO.Condition);
            result.ConditionValue.Should().Be(conditionDTO.ConditionValue);
        }

        [Fact]
        public void FlightScheduleFullDTO_WithValidFlightSchedule_MapsAllComplexProperties()
        {
            // Arrange
            var flightSchedule = new FlightSchedule
            {
                FlightScheduleId = Guid.NewGuid(),
                FlightId = "BA789",
                Departure = DateTimeOffset.Now.AddDays(5),
                Arrival = DateTimeOffset.Now.AddDays(5).AddHours(4),
                Price = 599.99m,
                Airline = new Airline
                {
                    AirlineId = Guid.NewGuid(),
                    AirlineName = "British Airways",
                    IATA = "BA",
                    Country = "United Kingdom"
                },
                Aircraft = new Aircraft
                {
                    AircraftId = Guid.NewGuid(),
                    Model = "Airbus A380",
                    RegistrationNumber = "G-XLEB"
                },
                Origin = new Airport
                {
                    AirportId = Guid.NewGuid(),
                    AirportName = "London Heathrow",
                    City = "London",
                    Country = "UK",
                    IATA = "LHR",
                    ICAO = "EGLL",
                    Latitude = 51.4700,
                    Longitude = -0.4543,
                    Altitude = 25,
                    Timezone = SimplyTimeZone.GMT,
                    DST = DaylightSavingTime.E,
                    Continent = "Europe"
                },
                Destination = new Airport
                {
                    AirportId = Guid.NewGuid(),
                    AirportName = "Dubai International",
                    City = "Dubai",
                    Country = "UAE",
                    IATA = "DXB",
                    ICAO = "OMDB",
                    Latitude = 25.2532,
                    Longitude = 55.3657,
                    Altitude = 62,
                    Timezone = SimplyTimeZone.UTC,
                    DST = DaylightSavingTime.U,
                    Continent = "Asia"
                },
                Seats = new List<FlightSeat>
                {
                    new FlightSeat
                    {
                        FlightSeatId = Guid.NewGuid(),
                        SeatNumber = "1A",
                        FlightScheduleId = Guid.NewGuid(),
                        Class = SeatClass.First,
                        IsAvailable = true,
                        Locked = null,
                        TicketId = null
                    },
                    new FlightSeat
                    {
                        FlightSeatId = Guid.NewGuid(),
                        SeatNumber = "12B",
                        FlightScheduleId = Guid.NewGuid(),
                        Class = SeatClass.Economy,
                        IsAvailable = false,
                        Locked = DateTime.UtcNow,
                        TicketId = Guid.NewGuid()
                    }
                }
            };

            // Act
            var result = flightSchedule.ToDTO();

            // Assert
            result.Should().NotBeNull();
            result.FlightScheduleId.Should().Be(flightSchedule.FlightScheduleId);
            result.FlightId.Should().Be(flightSchedule.FlightId);
            result.Departure.Should().Be(flightSchedule.Departure);
            result.Arrival.Should().Be(flightSchedule.Arrival);
            result.Price.Should().Be(flightSchedule.Price);

            // Airline
            result.Airline.Should().NotBeNull();
            result.Airline.AirlineId.Should().Be(flightSchedule.Airline.AirlineId);
            result.Airline.AirlineName.Should().Be(flightSchedule.Airline.AirlineName);
            result.Airline.IATA.Should().Be(flightSchedule.Airline.IATA);
            result.Airline.Country.Should().Be(flightSchedule.Airline.Country);

            // Aircraft
            result.Aircraft.Should().NotBeNull();
            result.Aircraft.AircraftId.Should().Be(flightSchedule.Aircraft.AircraftId);
            result.Aircraft.Model.Should().Be(flightSchedule.Aircraft.Model);
            result.Aircraft.RegistrationNumber.Should().Be(flightSchedule.Aircraft.RegistrationNumber);

            // Origin
            result.Origin.Should().NotBeNull();
            result.Origin.AirportId.Should().Be(flightSchedule.Origin.AirportId);
            result.Origin.AirportName.Should().Be(flightSchedule.Origin.AirportName);
            result.Origin.City.Should().Be(flightSchedule.Origin.City);
            result.Origin.IATA.Should().Be(flightSchedule.Origin.IATA);
            result.Origin.ICAO.Should().Be(flightSchedule.Origin.ICAO);
            result.Origin.Latitude.Should().Be(flightSchedule.Origin.Latitude);
            result.Origin.Longitude.Should().Be(flightSchedule.Origin.Longitude);
            result.Origin.Altitude.Should().Be(flightSchedule.Origin.Altitude);
            result.Origin.Timezone.Should().Be(flightSchedule.Origin.Timezone);
            result.Origin.DST.Should().Be(flightSchedule.Origin.DST);
            result.Origin.Continent.Should().Be(flightSchedule.Origin.Continent);

            // Destination
            result.Destination.Should().NotBeNull();
            result.Destination.AirportId.Should().Be(flightSchedule.Destination.AirportId);
            result.Destination.AirportName.Should().Be(flightSchedule.Destination.AirportName);
            result.Destination.City.Should().Be(flightSchedule.Destination.City);
            result.Destination.IATA.Should().Be(flightSchedule.Destination.IATA);
            result.Destination.ICAO.Should().Be(flightSchedule.Destination.ICAO);

            // Seats
            result.Seats.Should().HaveCount(2);
            result.Seats.First().SeatNumber.Should().Be("1A");
            result.Seats.First().Class.Should().Be(SeatClass.First);
            result.Seats.First().IsAvailable.Should().BeTrue();
            result.Seats.Last().SeatNumber.Should().Be("12B");
            result.Seats.Last().Class.Should().Be(SeatClass.Economy);
            result.Seats.Last().IsAvailable.Should().BeFalse();
        }
    }
}
