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

        public async Task<IEnumerable<Ticket>> GetAllTicketsAsync()
        {
            _logger.LogInformation("Getting all tickets");
            return await _tenantRepository.GetAllAsync();
        }
    }
}
