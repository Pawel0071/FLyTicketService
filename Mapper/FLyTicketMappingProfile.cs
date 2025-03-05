using FLyTicketService.DTO;
using FLyTicketService.Model;

namespace FLyTicketService.Mapper
{
    /// <summary>
    /// Mapowanie obiektów DTO i domenowych
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
                ReleaseDate = ticket.ReleaseDate?.DateTime // Obsługa nullable DateTimeOffset
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
                DiscountTypeId = discount.DiscountTypeId,
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
                DiscountTypeId = discountDTO.DiscountTypeId,
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
    }
}
