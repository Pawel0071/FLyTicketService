using FLyTicketService.DTO;
using FLyTicketService.Infrastructure;

namespace FLyTicketService.Service.Interfaces
{
    public interface ITicketService
    {
        public Task<OperationResult> ReserveTicketAsync(string flightId, string seatNo, Guid tenantId);

        public Task<OperationResult> SoldTicketAsync(string flightId, string seatNo, Guid tenantId, decimal discount);

        public Task<OperationResult> SoldReservedTicketAsync(string ticketNumber, decimal discount);

        public Task<OperationResult> CancelTicketAsync(string ticketNumber);

        public Task<TenantDTO?> GetTicketAsync(string ticketNumber);

        public Task<IEnumerable<TenantDTO>> GetTicketsAsync(string? flyNumber, Guid? tenantId);
    }
}
