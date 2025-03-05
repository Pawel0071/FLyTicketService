namespace FLyTicketService.Model
{
    public class Discount
    {
        public Guid DiscountTypeId { get; set; } = Guid.NewGuid();
        public required string Name { get; set; }
        public decimal Value { get; set; }
        public string? Description { get; set; }
        public IEnumerable<Condition> Conditions { get; set; }
    }
}
