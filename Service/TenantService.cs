using FLyTicketService.Model;
using FLyTicketService.Repositories.Interfaces;
using FLyTicketService.Service.Interfaces;

namespace FLyTicketService.Service
{
    public class TenantService: ITenantService
    {
        #region Fields

        private readonly IGenericRepository<Tenant> _tenantRepository;
        private readonly ILogger<TenantService> _logger;
        

        #endregion

        #region Constructors

        public TenantService(IGenericRepository<Tenant> tenantRepository, ILogger<TenantService> logger)
        {
            _tenantRepository = tenantRepository;
            _logger = logger;
        }


        #endregion
    }
}