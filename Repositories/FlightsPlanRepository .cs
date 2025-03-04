using FLyTicketService.Data;
using FLyTicketService.Model;
using FLyTicketService.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FLyTicketService.Repositories.Implementations
{
    public class FlightsPlanRepository: IFlightsPlanRepository
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
            return await this._context.FlightsPlans.FindAsync(flightsPlanId);
        }

        public async Task<bool> AddFlightsPlanAsync(FlightsPlan flightsPlan)

        {
            if (flightsPlan == null)
            {
                throw new ArgumentNullException(nameof(flightsPlan), "Flights Plan cannot be null.");
            }

            await this._context.FlightsPlans.AddAsync(flightsPlan);
            return await this.SaveChangesAsync();
        }

        public async Task<bool> UpdateFlightsPlanAsync(FlightsPlan flightsPlan)
        {
            if (flightsPlan == null)
            {
                throw new ArgumentNullException(nameof(flightsPlan), "Flights Plan cannot be null.");
            }

            FlightsPlan? existingFlightsPlan = await this._context.FlightsPlans.FindAsync(flightsPlan.FlightsPlanId);
            if (existingFlightsPlan == null)
            {
                this._logger.LogError($"Flights Plan with ID {flightsPlan.FlightsPlanId} was not found.");
                throw new KeyNotFoundException($"Flights Plan with ID {flightsPlan.FlightsPlanId} was not found.");
            }

            existingFlightsPlan.Airline = flightsPlan.Airline;
            existingFlightsPlan.Aircraft = flightsPlan.Aircraft;
            existingFlightsPlan.Seats = flightsPlan.Seats;
            existingFlightsPlan.Origin = flightsPlan.Origin;
            existingFlightsPlan.Destination = flightsPlan.Destination;
            existingFlightsPlan.Departure = flightsPlan.Departure;
            existingFlightsPlan.Arrival = flightsPlan.Arrival;
            existingFlightsPlan.FlyNumber = flightsPlan.FlyNumber;
            existingFlightsPlan.Price = flightsPlan.Price;

            this._context.FlightsPlans.Update(existingFlightsPlan);
            return await this.SaveChangesAsync();
        }

        public async Task<bool> DeleteFlightsPlanAsync(FlightsPlan flightsPlan)
        {
            if (flightsPlan == null)
            {
                throw new ArgumentNullException(nameof(flightsPlan), "Flights Plan cannot be null.");
            }

            try
            {
                this._context.FlightsPlans.Remove(flightsPlan);
                return await this.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 547)
            {
                this._logger.LogError(ex, "Cannot remove Flight Plan due to key violation.");
                throw new InvalidOperationException("Cannot remove Flight Plan due to key violation.", ex);
            }
        }

        public async Task<IEnumerable<FlightsPlan>> GetAllFlightsPlansAsync()
        {
            return await this._context.FlightsPlans
                             .Include(fp => fp.Airline)
                             .Include(fp => fp.Aircraft)
                             .Include(fp => fp.Seats)
                             .Include(fp => fp.Origin)
                             .Include(fp => fp.Destination)
                             .ToListAsync();
        }

        private async Task<bool> SaveChangesAsync()
        {
            return await this._context.SaveChangesAsync() > 0;
        }

        #endregion
    }
}