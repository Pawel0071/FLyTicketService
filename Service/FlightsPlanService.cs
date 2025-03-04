using FLyTicketService.Infrastructure;
using FLyTicketService.Model;
using FLyTicketService.Repositories.Interfaces;
using FLyTicketService.Services.Interfaces;

namespace FLyTicketService.Services.Implementations
{
    public class FlightsPlanService: IFlightsPlanService
    {
        #region Fields

        private readonly IFlightsPlanRepository _flightsPlanRepository;
        private readonly ILogger<FlightsPlanService> _logger;

        #endregion

        #region Constructors

        public FlightsPlanService(IFlightsPlanRepository flightsPlanRepository, ILogger<FlightsPlanService> logger)
        {
            _flightsPlanRepository = flightsPlanRepository;
            _logger = logger;
        }

        #endregion

        #region Methods

        public async Task<FlightsPlan?> GetFlightsPlanAsync(Guid flightsPlanId)
        {
            _logger.LogInformation("Getting flights plan with ID: {FlightsPlanId}", flightsPlanId);
            return await _flightsPlanRepository.GetFlightsPlanAsync(flightsPlanId);
        }

        public async Task<IEnumerable<FlightsPlan>> GetAllFlightsPlansAsync()
        {
            _logger.LogInformation("Getting all flights plans");
            return await _flightsPlanRepository.GetAllFlightsPlansAsync();
        }

        public async Task<OperationResult> AddFlightsPlanAsync(
            string flyNumber,
            Airline? airline,
            Aircraft? aircraft,
            Airport? origin,
            Airport? destination,
            DateTime departureTime,
            DateTime arrivalTime
        )
        {
            _logger.LogInformation("Adding new flights plan with fly number: {FlyNumber}", flyNumber);

            OperationResult? validationResult = ValidateFlightsPlan(
                flyNumber,
                departureTime,
                arrivalTime,
                origin,
                destination);

            if (validationResult != null)
            {
                return validationResult;
            }

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
                FlyNumber = flyNumber,
                Airline = airline,
                Aircraft = aircraft,
                Origin = origin,
                Destination = destination,
                Departure = departureTime,
                Arrival = arrivalTime,
                Seats = seats
            };

            await _flightsPlanRepository.AddFlightsPlanAsync(flightsPlan);

            _logger.LogInformation("Flights plan added successfully with fly number: {FlyNumber}", flyNumber);
            return new OperationResult { Status = OperationStatus.Created, Message = "Flights plan added successfully" };
        }

        public async Task<OperationResult> UpdateFlightsPlanAsync(FlightsPlan flightsPlan)
        {
            _logger.LogInformation("Updating flights plan with ID: {FlightsPlanId}", flightsPlan.FlightsPlanId);

            OperationResult? validationResult = ValidateFlightsPlan(
                flightsPlan.FlyNumber,
                flightsPlan.Departure,
                flightsPlan.Arrival,
                flightsPlan.Origin,
                flightsPlan.Destination);

            if (validationResult != null)
            {
                return validationResult;
            }

            await _flightsPlanRepository.UpdateFlightsPlanAsync(flightsPlan);

            _logger.LogInformation("Flights plan updated successfully with ID: {FlightsPlanId}", flightsPlan.FlightsPlanId);
            return new OperationResult { Status = OperationStatus.Ok, Message = "Flights plan updated successfully" };
        }

        public async Task<OperationResult> DeleteFlightsPlanAsync(FlightsPlan flightsPlan)
        {
            _logger.LogInformation("Deleting flights plan with ID: {FlightsPlanId}", flightsPlan.FlightsPlanId);
            await _flightsPlanRepository.DeleteFlightsPlanAsync(flightsPlan);

            _logger.LogInformation("Flights plan deleted successfully with ID: {FlightsPlanId}", flightsPlan.FlightsPlanId);
            return new OperationResult { Status = OperationStatus.Ok, Message = "Flights plan deleted successfully" };
        }

        private OperationResult? ValidateFlightsPlan(
            string flyNumber,
            DateTime departureTime,
            DateTime arrivalTime,
            Airport? origin,
            Airport? destination
        )
        {
            if (string.IsNullOrWhiteSpace(flyNumber) || flyNumber.Length > 10)
            {
                return new OperationResult { Status = OperationStatus.BadRequest, Message = "Invalid fly number" };
            }

            if (departureTime >= arrivalTime)
            {
                return new OperationResult { Status = OperationStatus.BadRequest, Message = "Departure time must be before arrival time" };
            }

            if (origin == null || destination == null)
            {
                return new OperationResult { Status = OperationStatus.BadRequest, Message = "Origin and destination cannot be null" };
            }

            if (origin.AirportId == destination.AirportId)
            {
                return new OperationResult { Status = OperationStatus.BadRequest, Message = "Origin and destination cannot be the same" };
            }

            return null;
        }

        #endregion
    }
}