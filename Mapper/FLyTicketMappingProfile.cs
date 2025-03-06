using FLyTicketService.DTO;
using FLyTicketService.Model;

namespace FLyTicketService.Mapper
{
    /// <summary>
    /// Mapowanie obiektów DTO i domenowych
    /// zamienić na autpo mappera
    /// </summary>
    public static class FLyTicketMappingProfile
    {
        // Mapowanie Ticket na TicketDTO
        public static TicketDTO ToDTO(this Ticket ticket)
        {
            return new TicketDTO
            {
                TicketId = ticket.TicketId,
                TicketNumber = ticket.TicketNumber,
                FlightId = ticket.FlightSeat.FlightSchedule.FlightId, // Wyodrębnienie FlightId
                SeatNumber = ticket.FlightSeat.SeatNumber,
                TenantId = ticket.Tenant.TenantId,
                Tenant = ticket.Tenant.Name,
                Price = ticket.Price,
                Discount = ticket.Discount,
                Status = ticket.Status,
                ReleaseDate = ticket.ReleaseDate?.DateTime, // Obsługa nullable DateTimeOffset
                FlightSeat = new FlightSeatDTO
                {
                    FlightSeatId = ticket.FlightSeat.FlightSeatId,
                    SeatNumber = ticket.FlightSeat.SeatNumber,
                    Class = ticket.FlightSeat.Class,
                }, // Mapowanie FlightSeat na FlightSeatDTO
                FlightSchedule = ticket.FlightSeat.FlightSchedule.ToDTO() // Mapowanie FlightSchedule na FlightScheduleDTO
            };
        }

        // Mapowanie TenantDTO na Tenant
        public static Tenant ToDomain(this TenantDTO tenantDTO)
        {
            return new Tenant
            {
                TenantId = tenantDTO.TenantId,
                Name = tenantDTO.Name,
                Address = tenantDTO.Address,
                Group = tenantDTO.Group,
                BirthDate = tenantDTO.BirthDate,
                Phone = tenantDTO.Phone,
                Email = tenantDTO.Email
            };
        }

        // Mapowanie Tenant na TenantDTO
        public static TenantDTO ToDTO(this Tenant tenant)
        {
            return new TenantDTO
            {
                TenantId = tenant.TenantId,
                Name = tenant.Name,
                Address = tenant.Address,
                Group = tenant.Group,
                BirthDate = tenant.BirthDate,
                Phone = tenant.Phone,
                Email = tenant.Email
            };
        }

        // Mapowanie Discount na DiscountDTO
        public static DiscountDTO ToDTO(this Discount discount)
        {
            return new DiscountDTO
            {
                DiscountTypeId = discount.DiscountId,
                Name = discount.Name,
                Value = discount.Value,
                Description = discount.Description,
                Conditions = discount.Conditions.Select(c => c.ToDTO()).ToList() // Poprawiono mapowanie Conditions
            };
        }

        // Mapowanie DiscountDTO na Discount
        public static Discount ToDomain(this DiscountDTO discountDTO)
        {
            return new Discount
            {
                DiscountId = discountDTO.DiscountTypeId,
                Name = discountDTO.Name,
                Value = discountDTO.Value,
                Description = discountDTO.Description,
                Conditions = discountDTO.Conditions.Select(c => c.ToDomain()).ToList() // Poprawiono mapowanie Conditions
            };
        }

        // Mapowanie Condition na ConditionDTO
        public static ConditionDTO ToDTO(this Condition condition)
        {
            return new ConditionDTO
            {
                ConditionId = condition.ConditionId,
                Category = condition.Category,
                Property = condition.Property,
                Condition = condition.ConditionType,
                ConditionValue = condition.ConditionValue
            };
        }

        // Mapowanie ConditionDTO na Condition
        public static Condition ToDomain(this ConditionDTO conditionDTO)
        {
            return new Condition
            {
                ConditionId = conditionDTO.ConditionId,
                Category = conditionDTO.Category,
                Property = conditionDTO.Property,
                ConditionType = conditionDTO.Condition,
                ConditionValue = conditionDTO.ConditionValue
            };
        }

        public static FlightScheduleFullDTO ToDTO(this FlightSchedule flightSchedule)
        {
            return new FlightScheduleFullDTO
            {
                FlightScheduleId = flightSchedule.FlightScheduleId,
                Airline = new AirlineDTO
                {
                    AirlineId = flightSchedule.Airline.AirlineId,
                    Country = flightSchedule.Airline.Country,
                    IATA = flightSchedule.Airline.IATA,
                    AirlineName = flightSchedule.Airline.AirlineName
                },
                Aircraft = new AircraftDTO
                {
                    AircraftId = flightSchedule.Aircraft.AircraftId,
                    Model = flightSchedule.Aircraft.Model,
                    RegistrationNumber = flightSchedule.Aircraft.RegistrationNumber
                },
                Seats = flightSchedule.Seats.Select(seat => new FlightSeatDTO
                {
                    FlightSeatId = seat.FlightSeatId,
                    SeatNumber = seat.SeatNumber,
                    FlightScheduleId = seat.FlightScheduleId,
                    Class = seat.Class,
                    IsAvailable = seat.IsAvailable,
                    Locked = seat.Locked,
                    TicketId = seat.TicketId
                }).ToList(),
                FlightId = flightSchedule.FlightId,
                Departure = flightSchedule.Departure,
                Arrival = flightSchedule.Arrival,
                Origin = new AirportDTO
                {
                    AirportId = flightSchedule.Origin.AirportId,
                    AirportName = flightSchedule.Origin.AirportName,
                    City = flightSchedule.Origin.City,
                    Country = flightSchedule.Origin.Country,
                    IATA = flightSchedule.Origin.IATA,
                    ICAO = flightSchedule.Origin.ICAO,
                    Latitude = flightSchedule.Origin.Latitude,
                    Longitude = flightSchedule.Origin.Longitude,
                    Altitude = flightSchedule.Origin.Altitude,
                    Timezone = flightSchedule.Origin.Timezone,
                    DST = flightSchedule.Origin.DST,
                    Continent = flightSchedule.Origin.Continent
                },
                Destination = new AirportDTO
                {
                    AirportId = flightSchedule.Destination.AirportId,
                    AirportName = flightSchedule.Destination.AirportName,
                    City = flightSchedule.Destination.City,
                    Country = flightSchedule.Destination.Country,
                    IATA = flightSchedule.Destination.IATA,
                    ICAO = flightSchedule.Destination.ICAO,
                    Latitude = flightSchedule.Destination.Latitude,
                    Longitude = flightSchedule.Destination.Longitude,
                    Altitude = flightSchedule.Destination.Altitude,
                    Timezone = flightSchedule.Destination.Timezone,
                    DST = flightSchedule.Destination.DST,
                    Continent = flightSchedule.Destination.Continent
                },
                Price = flightSchedule.Price
            };
        }


    }
}
