using FLyTicketService.DTO;
using FLyTicketService.Model.Enums;
using FLyTicketService.Shared;

namespace FLyTicketService.Services.Interfaces
{
    public interface IFlightScheduleService
    {
        Task<OperationResult<FlightScheduleFullDTO?>> GetFlightAsync(string flightId);
        Task<OperationResult<IEnumerable<FlightScheduleFullDTO>>> GetAllFlightsAsync();
        Task<OperationResult<bool>> ScheduleFlightAsync(FlightScheduleDTO flightSchedule);
        Task<OperationResult<bool>> ScheduleRecurringFlightAsync(FlightScheduleDTO flightSchedule, DaysOfWeekMask dayOfWeek, int numberOfOccuring);
        Task<OperationResult<bool>> UpdateFlightAsync(Guid flightScheduleId, FlightScheduleDTO flightSchedule, bool allRecurrences);
        Task<OperationResult<bool>> DeleteFlightAsync(Guid flightScheduleId, FlightScheduleDTO flightSchedule, bool allRecurrences);
    }
}