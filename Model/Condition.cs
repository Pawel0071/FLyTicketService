using FLyTicketService.Model.Enums;

namespace FLyTicketService.Model
{
    public class Condition
    {
        public Guid ConditionId { get; set; } = Guid.NewGuid();
        public Guid DiscountId { get; set; }
        public DiscountCategory Category { get; set; }
        public required string Property { get; set; }

        public DiscountCondition ConditionType { get; set; }
        public string? ConditionValue { get; set; }
    }
}
