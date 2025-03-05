using FLyTicketService.DTO;
using FLyTicketService.Model.Enums;
using FLyTicketService.Services.Interfaces;
using FLyTicketService.Shared;
using Microsoft.AspNetCore.Mvc;

namespace FLyTicketService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightScheduleController: ControllerBase
    {
        private readonly IFlightScheduleService _flightScheduleService;

        public FlightScheduleController(IFlightScheduleService flightScheduleService)
        {
            _flightScheduleService = flightScheduleService;
        }

        [HttpGet("{flightId}")]
        public async Task<IActionResult> GetFlight(string flightId)
        {
            OperationResult<FlightScheduleDTO?> result = await _flightScheduleService.GetFlightAsync(flightId);
            return result.GetResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFlights()
        {
            OperationResult<IEnumerable<FlightScheduleDTO>> result = await _flightScheduleService.GetAllFlightsAsync();
            return result.GetResult();
        }

        [HttpPost]
        public async Task<IActionResult> ScheduleFlight([FromBody] FlightScheduleDTO flightSchedule)
        {
            OperationResult<bool> result = await _flightScheduleService.ScheduleFlightAsync(flightSchedule);
            return result.GetResult();
        }

        [HttpPost("recurring")]
        public async Task<IActionResult> ScheduleRecurringFlight(
            [FromBody] FlightScheduleDTO flightSchedule,
            [FromQuery] DaysOfWeekMask dayOfWeek,
            [FromQuery] int numberOfOccurring
        )
        {
            OperationResult<bool> result = await _flightScheduleService.ScheduleRecurringFlightAsync(flightSchedule, dayOfWeek, numberOfOccurring);
            return result.GetResult();
        }

        [HttpPut("{flightScheduleId}")]
        public async Task<IActionResult> UpdateFlight(Guid flightScheduleId, [FromBody] FlightScheduleDTO flightSchedule, [FromQuery] bool allRecurrences)
        {
            OperationResult<bool> result = await _flightScheduleService.UpdateFlightAsync(flightScheduleId, flightSchedule, allRecurrences);
            return result.GetResult();
        }

        [HttpDelete("{flightScheduleId}")]
        public async Task<IActionResult> DeleteFlight(Guid flightScheduleId, [FromBody] FlightScheduleDTO flightSchedule, [FromQuery] bool allRecurrences)
        {
            OperationResult<bool> result = await _flightScheduleService.DeleteFlightAsync(flightScheduleId, flightSchedule, allRecurrences);
            return result.GetResult();
        }
    }
}