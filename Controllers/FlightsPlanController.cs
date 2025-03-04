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

        private readonly IFlightsPlanService _flightsPlanService;
        private readonly ILogger<FlightsPlanController> _logger;

        #endregion

        #region Constructors

        public FlightsPlanController(IFlightsPlanService flightsPlanService, ILogger<FlightsPlanController> logger)
        {
            _flightsPlanService = flightsPlanService;
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
        public async Task<ActionResult<IEnumerable<FlightsPlan>>> GetAllFlightsPlans()
        {
            _logger.LogInformation("Getting all flights plans");
            IEnumerable<FlightsPlan> flightsPlans = await _flightsPlanService.GetAllFlightsPlansAsync();
            return Ok(flightsPlans);
        }

        /// <summary>
        /// Get a flights plan by ID.
        /// </summary>
        /// <param name="flightsPlanId">The ID of the flights plan.</param>
        /// <returns>The flights plan with the specified ID.</returns>
        [HttpGet("{flightsPlanId}")]
        [OpenApiOperation("GetFlightsPlanById", "Get a flights plan by ID.")]
        public async Task<ActionResult<FlightsPlan>> GetFlightsPlanById(Guid flightsPlanId)
        {
            _logger.LogInformation("Getting flights plan with ID: {FlightsPlanId}", flightsPlanId);
            FlightsPlan? flightsPlan = await _flightsPlanService.GetFlightsPlanAsync(flightsPlanId);

            if (flightsPlan == null)
            {
                return NotFound();
            }

            return Ok(flightsPlan);
        }

        /// <summary>
        /// Add a new flights plan.
        /// </summary>
        /// <param name="flightsPlan">The flights plan details.</param>
        /// <returns>Result of the operation.</returns>
        [HttpPost]
        [OpenApiOperation("AddFlightsPlan", "Add a new flights plan.")]
        public async Task<ActionResult<OperationResult>> AddFlightsPlan([FromBody] FlightsPlan flightsPlan)
        {
            _logger.LogInformation("Adding new flights plan with fly number: {FlyNumber}", flightsPlan.FlyNumber);

            OperationResult result = await _flightsPlanService.AddFlightsPlanAsync(
                flightsPlan.FlyNumber,
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
        public async Task<ActionResult<OperationResult>> UpdateFlightsPlan([FromBody] FlightsPlan flightsPlan)
        {
            OperationResult result = await _flightsPlanService.UpdateFlightsPlanAsync(flightsPlan);
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

            FlightsPlan? flightsPlan = await _flightsPlanService.GetFlightsPlanAsync(flightsPlanId);
            if (flightsPlan == null)
            {
                return NotFound();
            }

            OperationResult result = await _flightsPlanService.DeleteFlightsPlanAsync(flightsPlan);
            return Ok(result);
        }

        #endregion
    }
}