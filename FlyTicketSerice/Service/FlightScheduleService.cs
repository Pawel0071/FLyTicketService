using FLyTicketService.DTO;
using FLyTicketService.Mapper;
using FLyTicketService.Model;
using FLyTicketService.Model.Enums;
using FLyTicketService.Repositories.Interfaces;
using FLyTicketService.Services.Interfaces;
using FLyTicketService.Shared;

namespace FLyTicketService.Services
{
    public class FlightScheduleService: IFlightScheduleService
    {
        #region Fields

        private readonly decimal _minimalPrice = 20;
        private readonly IGenericRepository<FlightSchedule> _flightScheduleRepository;
        private readonly IGenericRepository<Aircraft> _aircraftRepository;
        private readonly IGenericRepository<Airline> _airlineRepository;
        private readonly IGenericRepository<Airport> _airportRepository;
        private readonly ILogger<FlightScheduleService> _logger;

        #endregion

        #region Constructors

        public FlightScheduleService(
            IGenericRepository<FlightSchedule> flightScheduleRepository,
            IGenericRepository<Aircraft> aircraftRepository,
            IGenericRepository<Airline> airlineRepository,
            IGenericRepository<Airport> airportRepository,
            ILogger<FlightScheduleService> logger
        )
        {
            _flightScheduleRepository = flightScheduleRepository;
            _aircraftRepository = aircraftRepository;
            _airlineRepository = airlineRepository;
            _airportRepository = airportRepository;
            _logger = logger;
        }

        #endregion

        #region Methods

        public async Task<OperationResult<FlightScheduleFullDTO?>> GetFlightAsync(string flightId)
        {
            _logger.LogInformation($"Getting flight with ID {flightId}");

            FlightSchedule? flightSchedule = await _flightScheduleRepository.GetByAsync(fs => fs.FlightId == flightId);

            if (flightSchedule == null)
            {
                _logger.LogWarning("Flight not found");
                return new OperationResult<FlightScheduleFullDTO?>(OperationStatus.NotFound, "Flight not found", null);
            }

            FlightScheduleFullDTO flightScheduleDTO = flightSchedule.ToDTO();

            _logger.LogInformation("Flight retrieved successfully");

            return new OperationResult<FlightScheduleFullDTO?>(OperationStatus.Ok, "Flight retrieved successfully", flightScheduleDTO);
        }

        public async Task<OperationResult<IEnumerable<FlightScheduleFullDTO>>> GetAllFlightsAsync()
        {
            _logger.LogInformation("Getting all flights");

            IEnumerable<FlightSchedule> flightSchedules = await _flightScheduleRepository.GetAllAsync();

            if (!flightSchedules.Any())
            {
                _logger.LogWarning("No flights found");
                return new OperationResult<IEnumerable<FlightScheduleFullDTO>>(OperationStatus.NotFound, "No flights found", Enumerable.Empty<FlightScheduleFullDTO>());
            }

            IEnumerable<FlightScheduleFullDTO> flightScheduleDTOs = flightSchedules.Select(x => x.ToDTO());

            _logger.LogInformation("Flights retrieved successfully");

            return new OperationResult<IEnumerable<FlightScheduleFullDTO>>(OperationStatus.Ok, "Flights retrieved successfully", flightScheduleDTOs);
        }

        public async Task<OperationResult<bool>> ScheduleFlightAsync(FlightScheduleDTO flightSchedule)
        {
            if (!ValidateFlightScheduleDTO(flightSchedule))
            {
                return new OperationResult<bool>(OperationStatus.BadRequest, "Invalid flight schedule details", false);
            }
                
            FlightDetails flightDetails = await GetFlightDetailsAsync(flightSchedule);

            if (flightDetails.Airline == null ||
                flightDetails.Aircraft == null ||
                flightDetails.Origin == null ||
                flightDetails.Destination == null)
            {
                return new OperationResult<bool>(OperationStatus.BadRequest, "Invalid flight details", false);
            }

            FlightSchedule flight = MapFromFlightScheduleDTO(flightSchedule, flightDetails);

            bool result;
            try
            {
                result = await _flightScheduleRepository.AddAsync(flight);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding flight schedule");
                return new OperationResult<bool>(OperationStatus.BadRequest, "Invalid flight details", false);
            }

            return new OperationResult<bool>(OperationStatus.Created, string.Empty, result);
        }

        public async Task<OperationResult<bool>> ScheduleRecurringFlightAsync(FlightScheduleDTO flightSchedule, DaysOfWeekMask dayOfWeek, int numberOfOccuring)
        {
            if (!ValidateFlightScheduleDTO(flightSchedule))
            {
                return new OperationResult<bool>(OperationStatus.BadRequest, "Invalid flight schedule details", false);
            }

            FlightDetails flightDetails = await GetFlightDetailsAsync(flightSchedule);

            if (flightDetails.Airline == null ||
                flightDetails.Aircraft == null ||
                flightDetails.Origin == null ||
                flightDetails.Destination == null)
            {
                return new OperationResult<bool>(OperationStatus.BadRequest, "Invalid flight details", false);
            }

            List<Task<bool>> tasks = new List<Task<bool>>();
            DateTimeOffset departureDate = flightSchedule.Departure;
            DateTimeOffset arrivalDate = flightSchedule.Arrival;
            int occurrences = 0;

            while (occurrences < numberOfOccuring)
            {
                if ((dayOfWeek & (DaysOfWeekMask)(1 << (int)departureDate.DayOfWeek)) != 0)
                {
                    FlightSchedule flight = MapFromFlightScheduleDTO(flightSchedule, flightDetails);
                    flight.Departure = departureDate;
                    flight.Arrival = arrivalDate;

                    tasks.Add(_flightScheduleRepository.AddAsync(flight));
                    occurrences++;
                }

                departureDate = departureDate.AddDays(1);
                arrivalDate = arrivalDate.AddDays(1);
            }

            bool[] results = await Task.WhenAll(tasks);
            return new OperationResult<bool>(OperationStatus.Created, string.Empty, results.All(x => x));
        }

        public async Task<OperationResult<bool>> UpdateFlightAsync(Guid flightScheduleId, FlightScheduleDTO flightSchedule, bool allRecurrences)
        {
            if (ValidateFlightScheduleDTO(flightSchedule))
            {
                return new OperationResult<bool>(OperationStatus.BadRequest, "Invalid flight schedule details", false);
            }

            FlightSchedule? flight = await _flightScheduleRepository.GetByIdAsync(flightScheduleId);
            if (flight == null)
            {
                _logger.LogWarning("Flight not found");
                return new OperationResult<bool>(OperationStatus.NotFound, "Flight not found", false);
            }

            FlightDetails flightDetails = await GetFlightDetailsAsync(flightSchedule);

            if (flightDetails.Airline == null ||
                flightDetails.Aircraft == null ||
                flightDetails.Origin == null ||
                flightDetails.Destination == null)
            {
                return new OperationResult<bool>(OperationStatus.BadRequest, "Invalid flight details", false);
            }

            bool result = true;
            if (!allRecurrences)
            {
                flight = MapFromFlightScheduleDTO(flightSchedule, flightDetails);
                flight.FlightScheduleId = flightScheduleId;
                result = await _flightScheduleRepository.UpdateAsync(flight);
            }
            else
            {
                IEnumerable<FlightSchedule> flights = await _flightScheduleRepository.FilterByAsync(x => x.FlightId == flight.FlightId);
                foreach (FlightSchedule f in flights)
                {
                    FlightSchedule? updated = MapFromFlightScheduleDTO(flightSchedule, flightDetails);
                    updated.FlightScheduleId = f.FlightScheduleId;
                    bool internalResult = await _flightScheduleRepository.UpdateAsync(updated);
                    result = result && internalResult;
                }
            }

            return new OperationResult<bool>(OperationStatus.Ok, string.Empty, result);
        }

        public async Task<OperationResult<bool>> DeleteFlightAsync(Guid flightScheduleId, FlightScheduleDTO flightSchedule, bool allRecurrences)
        {
            FlightSchedule? flight = await _flightScheduleRepository.GetByIdAsync(flightScheduleId);
            if (flight == null)
            {
                _logger.LogWarning("Flight not found");
                return new OperationResult<bool>(OperationStatus.NotFound, "Flight not found", false);
            }

            bool result = await _flightScheduleRepository.RemoveAsync(flightScheduleId);
            return new OperationResult<bool>(OperationStatus.Ok, string.Empty, result);
        }

        private FlightSchedule MapFromFlightScheduleDTO(
            FlightScheduleDTO flightSchedule,
            FlightDetails flightDetails
        )
        {
            List<FlightSeat> flightSeats = new List<FlightSeat>();

            foreach (AircraftSeat aircraftSeat in flightDetails.Aircraft.Seats)
            {
                flightSeats.Add(
                    new FlightSeat
                    {
                        Class = aircraftSeat.Class,
                        SeatNumber = aircraftSeat.SeatNumber,
                        IsAvailable = !aircraftSeat.OutOfService
                    });
            }

            return new FlightSchedule
            {
                FlightScheduleId = flightSchedule.FlightScheduleId,
                Airline = flightDetails.Airline,
                Aircraft = flightDetails.Aircraft,
                FlightId = $"{flightSchedule.AirlineIATA} {flightSchedule.Number} {flightSchedule.NumberSuffix}",
                Seats = flightSeats,
                Departure = flightSchedule.Departure,
                Arrival = flightSchedule.Arrival,
                Origin = flightDetails.Origin,
                Destination = flightDetails.Destination,
                Price = flightSchedule.Price
            };
        }

        private async Task<FlightDetails> GetFlightDetailsAsync(FlightScheduleDTO flightSchedule)
        {
            Airline? airlineTask = await _airlineRepository.GetByAsync(x => x.IATA == flightSchedule.AirlineIATA);
            Aircraft aircraftTask = await _aircraftRepository.GetByAsync(x => x.RegistrationNumber == flightSchedule.AircraftRegistrationNumber);
            Airport? originTask = await _airportRepository.GetByAsync(x => x.IATA == flightSchedule.OriginIATA);
            Airport? destinationTask = await _airportRepository.GetByAsync(x => x.IATA == flightSchedule.DestinationIATA);
/*
 parallel need configure
            await Task.WhenAll(
                airlineTask,
                aircraftTask,
                originTask,
                destinationTask);
*/
            return new FlightDetails(
                airlineTask,
                aircraftTask,
                originTask,
                destinationTask);
        }

        private bool ValidateFlightScheduleDTO(FlightScheduleDTO flightSchedule)
        {
            if (!string.IsNullOrWhiteSpace(flightSchedule.AirlineIATA) &&
                !string.IsNullOrWhiteSpace(flightSchedule.AircraftRegistrationNumber) &&
                !string.IsNullOrWhiteSpace(flightSchedule.OriginIATA) &&
                !string.IsNullOrWhiteSpace(flightSchedule.DestinationIATA) &&
                flightSchedule.Departure != default &&
                flightSchedule.Arrival != default &&
                flightSchedule.Price > _minimalPrice)
            {
                return true;
            }

            _logger.LogWarning("Invalid flight schedule details");
            return false;
        }

        #endregion
    }
}