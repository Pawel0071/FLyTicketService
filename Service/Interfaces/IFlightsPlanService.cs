using FLyTicketService.Infrastructure;
using FLyTicketService.Model;

namespace FLyTicketService.Services.Interfaces
{
    public interface IFlightsPlanService
    {
        Task<FlightsPlan?> GetFlightsPlanAsync(Guid flightsPlanId);
        Task<IEnumerable<FlightsPlan>> GetAllFlightsPlansAsync();

        Task<OperationResult> AddFlightsPlanAsync(
            string flyNumber,
            Airline airline,
            Aircraft aircraft,
            Airport origin,
            Airport destination,
            DateTime departureTime,
            DateTime arrivalTime
        );

        Task<OperationResult> UpdateFlightsPlanAsync(FlightsPlan flightsPlan);
        Task<OperationResult> DeleteFlightsPlanAsync(FlightsPlan flightsPlan);
    }
}