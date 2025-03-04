using FLyTicketService.Data;
using FLyTicketService.Model;
using FLyTicketService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FLyTicketService.Exceptions;

namespace FLyTicketService.Repositories.Implementations
{
    public class FlightsPlanRepository : IFlightsPlanRepository
    {
        #region Fields

        private readonly FLyTicketDbContext _context;
        private readonly ILogger<FlightsPlanRepository> _logger;

        #endregion

        #region Constructors

        public FlightsPlanRepository(FLyTicketDbContext context, ILogger<FlightsPlanRepository> logger)
        {
            this._context = context;
            this._logger = logger;
        }

        #endregion

        #region Methods

        public async Task<FlightsPlan?> GetFlightsPlanAsync(Guid flightsPlanId)
        {
            try
            {
                return await this._context.FlightsPlans
                                 .Include(fp => fp.Airline)
                                 .Include(fp => fp.Aircraft)
                                 .Include(fp => fp.Seats)
                                 .Include(fp => fp.Origin)
                                 .Include(fp => fp.Destination)
                                 .FirstOrDefaultAsync(fp => fp.FlightsPlanId == flightsPlanId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting FlightsPlan with ID {FlightsPlanId}", flightsPlanId);
                throw new FlightsPlanException("Error getting FlightsPlan", ex);
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
                            FlightsPlan = null
                        });
                }

                FlightsPlan flightsPlan = new FlightsPlan
                {
                    Airline = airline,
                    Aircraft = aircraft,
                    Seats = seats,
                    Origin = origin,
                    Destination = destination,
                    Departure = departureTime,
                    Arrival = arrivalTime,
                    FlyNumber = flyNumber
                };

                await this._context.FlightsPlans.AddAsync(flightsPlan);
                return await this._context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding FlightsPlan");
                throw new FlightsPlanException("Error adding FlightsPlan", ex);
            }
        }

        public async Task<bool> UpdateFlightsPlanAsync(FlightsPlan flightsPlan)
        {
            try
            {
                this._context.FlightsPlans.Update(flightsPlan);
                return await this._context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating FlightsPlan with ID {FlightsPlanId}", flightsPlan.FlightId);
                throw new FlightsPlanException("Error updating FlightsPlan", ex);
            }
        }

        public async Task<bool> DeleteFlightsPlanAsync(Guid flightsPlanId)
        {
            try
            {
                FlightsPlan? flightsPlan = await this.GetFlightsPlanAsync(flightsPlanId);
                if (flightsPlan != null)
                {
                    this._context.FlightsPlans.Remove(flightsPlan);
                    return await this._context.SaveChangesAsync() > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting FlightsPlan with ID {FlightsPlanId}", flightsPlanId);
                throw new FlightsPlanException("Error deleting FlightsPlan", ex);
            }
        }

        public async Task<IEnumerable<FlightsPlan>> GetAllFlightsPlansAsync()
        {
            try
            {
                return await this._context.FlightsPlans
                                 .Include(fp => fp.Airline)
                                 .Include(fp => fp.Aircraft)
                                 .Include(fp => fp.Seats)
                                 .Include(fp => fp.Origin)
                                 .Include(fp => fp.Destination)
                                 .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all FlightsPlans");
                throw new FlightsPlanException("Error getting all FlightsPlans", ex);
            }
        }

        #endregion
    }
}

    public class FlightsPlanException : Exception
    {
        public FlightsPlanException() { }

        public FlightsPlanException(string message) : base(message) { }

        public FlightsPlanException(string message, Exception innerException) : base(message, innerException) { }
    }

