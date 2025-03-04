using FLyTicketService.Model;
using FLyTicketService.Repositories.Interfaces;
using FLyTicketService.Services.Interfaces;

namespace FLyTicketService.Services.Implementations
{
    public class FlightScheduleService: IFlightScheduleService
    {
        #region Fields

        private readonly IGenericRepository<FlightSchedule> _flightScheduleRepository;
        private readonly ILogger<FlightScheduleService> _logger;

        #endregion

        #region Constructors

        public FlightScheduleService(IGenericRepository<FlightSchedule> flightScheduleRepository, ILogger<FlightScheduleService> logger)
        {
            _flightScheduleRepository = flightScheduleRepository;
            _logger = logger;
        }

        #endregion

        #region Methods

        #endregion
    }
}