using FLyTicketService.Data;
using FLyTicketService.Model;
using FLyTicketService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FLyTicketService.Repositories.Implementations
{
    public class FlightScheduleRepository : IFlightScheduleRepository
    {
        #region Fields

        private readonly FLyTicketDbContext _context;
        private readonly ILogger<FlightsPlanRepository> _logger;

        #endregion

        #region Constructors

        public FlightScheduleRepository(FLyTicketDbContext context, ILogger<FlightsPlanRepository> logger)
        {
            this._context = context;
            this._logger = logger;
        }

        #endregion

        #region Methods

        public async Task<FlightSchedule?> GetFlightsPlanAsync(Guid flightsPlanId)
        {
            try
            {
                return await this._context.FlightScheduler
                                 .Include(fp => fp.Airline)
                                 .Include(fp => fp.Aircraft)
                                 .Include(fp => fp.Seats)
                                 .Include(fp => fp.Origin)
                                 .Include(fp => fp.Destination)
                                 .FirstOrDefaultAsync(fp => fp.FlightScheduleId == flightsPlanId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting FlightsPlan with ID {FlightsPlanId}", flightsPlanId);
                throw new Exception($"Error occurred while getting FlightsPlan with ID {flightsPlanId}", ex);
            }
        }

        public async Task<bool> AddFlightsPlanAsync(
            string flyNumber,
            Airline airline,
            Aircraft aircraft,
            Airport origin,
            Airport destination,
            DateTime departureTime,
            DateTime arrivalTime
        )
        {
            try
            {
                List<FlightSeat> seats = new List<FlightSeat>();

                foreach (AircraftSeat seat in aircraft.Seats)
                {
                    seats.Add(
                        new FlightSeat
                        {
                            SeatNumber = seat.SeatNumber,
                            Class = seat.Class,
                            IsAvailable = !seat.OutOfService,
                            FlightSchedule = null
                        });
                }

                FlightSchedule flightSchedule = new FlightSchedule
                {
                    Airline = airline,
                    Aircraft = aircraft,
                    Seats = seats,
                    Origin = origin,
                    Destination = destination,
                    Departure = departureTime,
                    Arrival = arrivalTime,
                    FlightId = flyNumber
                };

                await this._context.FlightScheduler.AddAsync(flightSchedule);
                return await this._context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a new FlightsPlan");
                throw new Exception("Error occurred while adding a new FlightsPlan", ex);
            }
        }

        public async Task<bool> UpdateFlightsPlanAsync(FlightSchedule flightSchedule)
        {
            try
            {
                this._context.FlightScheduler.Update(flightSchedule);
                return await this._context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating FlightsPlan with ID {FlightsPlanId}", flightSchedule.FlightId);
                throw new Exception($"Error occurred while updating FlightsPlan with ID {flightSchedule.FlightId}", ex);
            }
        }

        public async Task<bool> DeleteFlightsPlanAsync(Guid flightsPlanId)
        {
            try
            {
                FlightSchedule? flightsPlan = await this.GetFlightsPlanAsync(flightsPlanId);
                if (flightsPlan != null)
                {
                    this._context.FlightScheduler.Remove(flightsPlan);
                    return await this._context.SaveChangesAsync() > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting FlightsPlan with ID {FlightsPlanId}", flightsPlanId);
                throw new Exception($"Error occurred while deleting FlightsPlan with ID {flightsPlanId}", ex);
            }
        }

        public async Task<IEnumerable<FlightSchedule>> GetAllFlightsPlansAsync()
        {
            try
            {
                return await this._context.FlightScheduler
                                 .Include(fp => fp.Airline)
                                 .Include(fp => fp.Aircraft)
                                 .Include(fp => fp.Seats)
                                 .Include(fp => fp.Origin)
                                 .Include(fp => fp.Destination)
                                 .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all FlightsPlans");
                throw new Exception("Error occurred while getting all FlightsPlans", ex);
            }
        }

        #endregion
    }
}
