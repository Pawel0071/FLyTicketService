using FLyTicketService.DTO;
using FLyTicketService.Infrastructure;

namespace FLyTicketService.Service.Interfaces
{
    public interface ITicketService
    {
        public Task<OperationResult<bool>> ReserveTicketAsync(string flightId, string seatNo, Guid tenantId);

        public Task<OperationResult<bool>> SoldTicketAsync(string flightId, string seatNo, Guid tenantId, decimal discount);

        public Task<OperationResult<bool>> SoldReservedTicketAsync(string ticketNumber, decimal discount);

        public Task<OperationResult<bool>> CancelTicketAsync(string ticketNumber);

        public Task<OperationResult<TicketDTO?>> GetTicketAsync(string ticketNumber);

        public Task<OperationResult<IEnumerable<TicketDTO>>> GetTicketsAsync(string? flyNumber, Guid? tenantId);
    }
}
