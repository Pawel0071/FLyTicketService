using FLyTicketService.DTO;
using FLyTicketService.Shared;

namespace FLyTicketService.Service.Interfaces
{
    public interface ITicketService
    {
        public Task<OperationResult<TicketDTO?>> ReserveTicketAsync(string flightId, string seatNo, Guid tenantId);

        public Task<OperationResult<TicketDTO?>> SaleTicketAsync(string flightId, string seatNo, Guid tenantId, IEnumerable<DiscountDTO> discounts);

        public Task<OperationResult<TicketDTO?>> SaleReservedTicketAsync(string ticketNumber);

        public Task<OperationResult<IEnumerable<DiscountDTO>>> GetAllApplicableDiscountsAsync(string ticketNumber);

        public Task<OperationResult<IEnumerable<DiscountDTO>>> GetAllDiscountsAsync();

        public Task<OperationResult<bool>> ApplyDiscountAsync(string ticketNumber, IEnumerable<DiscountDTO> discounts);

        public Task<OperationResult<bool>> CanDiscountAppliedAsync(string ticketNumber, DiscountDTO discounts);

        public Task<OperationResult<bool>> CancelTicketAsync(string ticketNumber);

        public Task<OperationResult<TicketDTO?>> GetTicketAsync(string ticketNumber);

        public Task<OperationResult<IEnumerable<TicketDTO>>> GetTicketListByAsync(
            string? flyNumber, 
            Guid? tenantId,
            DateTime? Departure,
            string? OriginIATA,
            string? DestinationIATA);
    }
}
