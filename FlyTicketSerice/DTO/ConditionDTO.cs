using FLyTicketService.Model.Enums;

namespace FLyTicketService.DTO
{
    public class ConditionDTO
    {
        #region Properties
        public Guid ConditionId { get; set; }
        public DiscountCategory Category { get; set; }
        public required string Property { get; set; }
        public DiscountCondition Condition { get; set; }
        public string? ConditionValue { get; set; }

        #endregion
    }
}