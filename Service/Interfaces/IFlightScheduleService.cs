using FLyTicketService.DTO;
using FLyTicketService.Infrastructure;
using FLyTicketService.Model;
using FLyTicketService.Model.Enums;

namespace FLyTicketService.Services.Interfaces
{
    public interface IFlightScheduleService
    {
        Task<FlightSchedule?> GetFlightAsync(string flightId);
        Task<IEnumerable<FlightSchedule>> GetAllFlightsAsync();
        Task<OperationResult> ScheduleFlightAsync(FlightScheduleDTO flightSchedule);
        Task<OperationResult> ScheduleRecurringFlightAsync(FlightScheduleDTO flightSchedule, DaysOfWeekMask dayOfWeek, int numberOfOccuring);
        Task<OperationResult> UpdateFlightAsync(FlightScheduleDTO flightSchedule);
        Task<OperationResult> DeleteFlightAsync(FlightScheduleDTO flightSchedule, bool allRecurrences);
    }
}