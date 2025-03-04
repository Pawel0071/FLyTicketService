using FLyTicketService.DTO;
using FLyTicketService.Infrastructure;
using FLyTicketService.Model;
using FLyTicketService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace FLyTicketService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsPlanController: ControllerBase
    {
        #region Fields

        private readonly IFlightScheduleService _flightScheduleService;
        private readonly ILogger<FlightsPlanController> _logger;

        #endregion

        #region Constructors

        public FlightsPlanController(IFlightScheduleService flightScheduleService, ILogger<FlightsPlanController> logger)
        {
            _flightScheduleService = flightScheduleService;
            _logger = logger;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get all flights plans.
        /// </summary>
        /// <returns>List of flights plans.</returns>
        [HttpGet]
        [OpenApiOperation("GetAllFlightsPlans", "Get all flights plans.")]
        public async Task<ActionResult<IEnumerable<FlightSchedule>>> GetAllFlightsPlans()
        {
            _logger.LogInformation("Getting all flights plans");
            IEnumerable<FlightSchedule> flightsPlans = await _flightScheduleService.GetAllFlightsPlansAsync();
            return Ok(flightsPlans);
        }

        /// <summary>
        /// Get a flights plan by ID.
        /// </summary>
        /// <param name="flightsPlanId">The ID of the flights plan.</param>
        /// <returns>The flights plan with the specified ID.</returns>
        [HttpGet("{flightsPlanId}")]
        [OpenApiOperation("GetFlightsPlanById", "Get a flights plan by ID.")]
        public async Task<ActionResult<FlightSchedule>> GetFlightsPlanById(Guid flightsPlanId)
        {
            _logger.LogInformation("Getting flights plan with ID: {FlightsPlanId}", flightsPlanId);
            FlightSchedule? flightsPlan = await _flightScheduleService.GetFlightsPlanAsync(flightsPlanId);

            if (flightsPlan == null)
            {
                return NotFound();
            }

            return Ok(flightsPlan);
        }

        /// <summary>
        /// Add a new flights plan.
        /// </summary>
        /// <param name="flightSchedule">The flights plan details.</param>
        /// <returns>Result of the operation.</returns>
        [HttpPost]
        [OpenApiOperation("AddFlightsPlan", "Add a new flights plan.")]
        public async Task<ActionResult<OperationResult>> AddFlightsPlan([FromBody] FlightSchedule flightSchedule)
        {
            _logger.LogInformation("Adding new flights plan with fly number: {FlyNumber}", flightSchedule.FlightId);

            OperationResult result = await _flightScheduleService.AddFlightsPlanAsync(
                flightSchedule.FlightId,
                flightSchedule.Airline,
                flightSchedule.Aircraft,
                flightSchedule.Origin,
                flightSchedule.Destination,
                flightSchedule.Departure,
                flightSchedule.Arrival);

            return CreatedAtAction(nameof(GetFlightsPlanById), new { flightsPlanId = flightSchedule.FlightScheduleId }, result);
        }

        [HttpPost]
        [OpenApiOperation("AddFlightsPlan", "Add a new flights plan.")]
        public async Task<ActionResult<OperationResult>> AddFlightsScheduler(int recurrence, int occurence, [FromBody] FlightScheduleDTO flightsPlan)
        {
            _logger.LogInformation("Adding new flights plan with fly number: {FlyNumber}", flightsPlan.FlightId);

            OperationResult result = await _flightScheduleService.AddFlightsPlanAsync(
                flightsPlan.FlightId,
                flightsPlan.Airline,
                flightsPlan.Aircraft,
                flightsPlan.Origin,
                flightsPlan.Destination,
                flightsPlan.Departure,
                flightsPlan.Arrival);

            return CreatedAtAction(nameof(GetFlightsPlanById), new { flightsPlanId = flightsPlan.FlightsPlanId }, result);
        }

        /// <summary>
        /// Update an existing flights plan.
        /// </summary>
        /// <param name="flightsPlanDto">The updated flights plan details.</param>
        /// <returns>Result of the operation.</returns>
        [HttpPut]
        [OpenApiOperation("UpdateFlightsPlan", "Update an existing flights plan.")]
        public async Task<ActionResult<OperationResult>> UpdateFlightsPlan([FromBody] FlightSchedule flightSchedule)
        {
            OperationResult result = await _flightScheduleService.UpdateFlightsPlanAsync(flightSchedule);
            return Ok(result);
        }

        /// <summary>
        /// Delete an existing flights plan.
        /// </summary>
        /// <param name="flightsPlanId">The ID of the flights plan to delete.</param>
        /// <returns>Result of the operation.</returns>
        [HttpDelete("{flightsPlanId}")]
        [OpenApiOperation("DeleteFlightsPlan", "Delete an existing flights plan.")]
        public async Task<ActionResult<OperationResult>> DeleteFlightsPlan(Guid flightsPlanId)
        {
            _logger.LogInformation("Deleting flights plan with ID: {FlightsPlanId}", flightsPlanId);

            FlightSchedule? flightsPlan = await _flightScheduleService.GetFlightsPlanAsync(flightsPlanId);
            if (flightsPlan == null)
            {
                return NotFound();
            }

            OperationResult result = await _flightScheduleService.DeleteFlightsPlanAsync(flightsPlan);
            return Ok(result);
        }

        #endregion
    }
}