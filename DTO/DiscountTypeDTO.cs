using FLyTicketService.Model.Enums;

namespace FLyTicketService.DTO
{
    public class DiscountTypeDTO
    {
        public Guid DiscountTypeId { get; set; }
        public DiscountCategory Type { get; set; }
        public required string Name { get; set; }
        public decimal Discount { get; set; }
        public string? Description { get; set; }
        public string? Condition { get; set; }
        public string? ConditionValue { get; set; }
    }
}
