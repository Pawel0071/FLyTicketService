namespace FLyTicketService.DTO
{
    public class DiscountDTO
    {
        #region Properties

        public Guid DiscountTypeId { get; set; }
        public required string Name { get; set; }
        public decimal Value { get; set; }
        public string? Description { get; set; }
        public IEnumerable<ConditionDTO> Conditions { get; set; } = new List<ConditionDTO>();

        #endregion
    }
}