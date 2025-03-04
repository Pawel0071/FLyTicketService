using FLyTicketService.DTO;
using FLyTicketService.Infrastructure;
using FLyTicketService.Model.Enums;

namespace FLyTicketService.Services.Interfaces
{
    public interface IFlightScheduleService
    {
        Task<OperationResult<FlightScheduleDTO?>> GetFlightAsync(string flightId);
        Task<OperationResult<IEnumerable<FlightScheduleDTO>>> GetAllFlightsAsync();
        Task<OperationResult<bool>> ScheduleFlightAsync(FlightScheduleDTO flightSchedule);
        Task<OperationResult<bool>> ScheduleRecurringFlightAsync(FlightScheduleDTO flightSchedule, DaysOfWeekMask dayOfWeek, int numberOfOccuring);
        Task<OperationResult<bool>> UpdateFlightAsync(Guid flightScheduleId, FlightScheduleDTO flightSchedule, bool allRecurrences);
        Task<OperationResult<bool>> DeleteFlightAsync(Guid flightScheduleId, FlightScheduleDTO flightSchedule, bool allRecurrences);
    }
}