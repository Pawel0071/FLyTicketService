using FLyTicketService.DTO;
using FLyTicketService.Infrastructure;
using FLyTicketService.Model;
using FLyTicketService.Repositories.Interfaces;
using FLyTicketService.Service.Interfaces;

namespace FLyTicketService.Service
{
    public class TicketService : ITicketService
    {


        private readonly IGenericRepository<Ticket> _tenantRepository;
        private readonly ILogger<TicketService> _logger;

        public TicketService(IGenericRepository<Ticket> tenantRepository, ILogger<TicketService> logger)
        {
            _tenantRepository = tenantRepository;
            _logger = logger;
        }

        public Task<OperationResult<bool>> CancelTicketAsync(string ticketNumber)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<TicketDTO?>> GetTicketAsync(string ticketNumber)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<IEnumerable<TicketDTO>>> GetTicketsAsync(string? flyNumber, Guid? tenantId)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<bool>> ReserveTicketAsync(string flightId, string seatNo, Guid tenantId)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<bool>> SoldReservedTicketAsync(string ticketNumber, decimal discount)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<bool>> SoldTicketAsync(string flightId, string seatNo, Guid tenantId, decimal discount)
        {
            throw new NotImplementedException();
        }
    }
}
