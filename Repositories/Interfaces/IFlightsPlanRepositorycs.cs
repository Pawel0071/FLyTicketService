using FLyTicketService.Model;

namespace FLyTicketService.Repositories.Interfaces
{
    public interface IFlightsPlanRepository
    {
        Task<FlightsPlan?> GetFlightsPlanAsync(Guid flightsPlanId);
        Task<IEnumerable<FlightsPlan>> GetAllFlightsPlansAsync();
        Task<bool> AddFlightsPlanAsync(FlightsPlan flightsPlan);
        Task<bool> UpdateFlightsPlanAsync(FlightsPlan flightsPlan);
        Task<bool> DeleteFlightsPlanAsync(FlightsPlan flightsPlan);
    }
}