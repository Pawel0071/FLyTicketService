using FLyTicketService.DTO;
using FLyTicketService.Extension;
using FLyTicketService.Infrastructure;
using FLyTicketService.Model;
using FLyTicketService.Model.Enums;
using FLyTicketService.Repositories.Interfaces;
using FLyTicketService.Services.Interfaces;

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
        private readonly IGenericRepository<FlightType> _flightTypeRepository;
        private readonly ILogger<FlightScheduleService> _logger;

        #endregion

        #region Constructors

        public FlightScheduleService(
            IGenericRepository<FlightSchedule> flightScheduleRepository,
            IGenericRepository<Aircraft> aircraftRepository,
            IGenericRepository<Airline> airlineRepository,
            IGenericRepository<Airport> airportRepository,
            IGenericRepository<FlightType> flightTypeRepository,
            ILogger<FlightScheduleService> logger
        )
        {
            _flightScheduleRepository = flightScheduleRepository;
            _aircraftRepository = aircraftRepository;
            _airlineRepository = airlineRepository;
            _airportRepository = airportRepository;
            _flightTypeRepository = flightTypeRepository;
            _logger = logger;
        }

        #endregion

        #region Methods

        public async Task<OperationResult<FlightScheduleDTO?>> GetFlightAsync(string flightId)
        {
            _logger.LogInformation($"Getting flight with ID {flightId}");

            FlightSchedule? flightSchedule = await _flightScheduleRepository.GetByAsync(fs => fs.FlightId == flightId);

            if (flightSchedule == null)
            {
                _logger.LogWarning("Flight not found");
                return new OperationResult<FlightScheduleDTO?>(OperationStatus.NotFound, "Flight not found", null);
            }

            FlightScheduleDTO flightScheduleDTO = MapToFlightScheduleDTO(flightSchedule);

            _logger.LogInformation("Flight retrieved successfully");

            return new OperationResult<FlightScheduleDTO?>(OperationStatus.Ok, "Flight retrieved successfully", flightScheduleDTO);
        }

        public async Task<OperationResult<IEnumerable<FlightScheduleDTO>>> GetAllFlightsAsync()
        {
            _logger.LogInformation("Getting all flights");

            IEnumerable<FlightSchedule> flightSchedules = await _flightScheduleRepository.GetAllAsync();

            if (!flightSchedules.Any())
            {
                _logger.LogWarning("No flights found");
                return new OperationResult<IEnumerable<FlightScheduleDTO>>(OperationStatus.NotFound, "No flights found", Enumerable.Empty<FlightScheduleDTO>());
            }

            IEnumerable<FlightScheduleDTO> flightScheduleDTOs = flightSchedules.Select(MapToFlightScheduleDTO);

            _logger.LogInformation("Flights retrieved successfully");

            return new OperationResult<IEnumerable<FlightScheduleDTO>>(OperationStatus.Ok, "Flights retrieved successfully", flightScheduleDTOs);
        }

        public async Task<OperationResult<bool>> ScheduleFlightAsync(FlightScheduleDTO flightSchedule)
        {
            if (ValidateFlightScheduleDTO(flightSchedule))
            {
                return new OperationResult<bool>(OperationStatus.BadRequest, "Invalid flight schedule details", false);
            }

            (Airline? Airline, Aircraft? Aircraft, Airport? Origin, Airport? Destination, FlightType? FlightType) flightDetails =
                await GetFlightDetailsAsync(flightSchedule);

            if (flightDetails.Airline == null ||
                flightDetails.Aircraft == null ||
                flightDetails.Origin == null ||
                flightDetails.Destination == null ||
                flightDetails.FlightType == null)
            {
                return new OperationResult<bool>(OperationStatus.BadRequest, "Invalid flight details", false);
            }

            FlightSchedule flight = MapFromFlightScheduleDTO(flightSchedule, flightDetails);

            bool result = await _flightScheduleRepository.AddAsync(flight);
            return new OperationResult<bool>(OperationStatus.Created, string.Empty, result);
        }

        public async Task<OperationResult<bool>> ScheduleRecurringFlightAsync(FlightScheduleDTO flightSchedule, DaysOfWeekMask dayOfWeek, int numberOfOccuring)
        {
            if (!ValidateFlightScheduleDTO(flightSchedule))
            {
                return new OperationResult<bool>(OperationStatus.BadRequest, "Invalid flight schedule details", false);
            }

            (Airline? Airline, Aircraft? Aircraft, Airport? Origin, Airport? Destination, FlightType? FlightType) flightDetails =
                await GetFlightDetailsAsync(flightSchedule);

            if (flightDetails.Airline == null ||
                flightDetails.Aircraft == null ||
                flightDetails.Origin == null ||
                flightDetails.Destination == null ||
                flightDetails.FlightType == null)
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

            (Airline? Airline, Aircraft? Aircraft, Airport? Origin, Airport? Destination, FlightType? FlightType) flightDetails =
                await GetFlightDetailsAsync(flightSchedule);

            if (flightDetails.Airline == null ||
                flightDetails.Aircraft == null ||
                flightDetails.Origin == null ||
                flightDetails.Destination == null ||
                flightDetails.FlightType == null)
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

        private FlightScheduleDTO MapToFlightScheduleDTO(FlightSchedule flightSchedule)
        {
            return new FlightScheduleDTO
            {
                FlightScheduleId = flightSchedule.FlightScheduleId,
                AirlineIATA = flightSchedule.Airline.IATA,
                AircraftRegistrationNumber = flightSchedule.Aircraft.RegistrationNumber,
                Number = flightSchedule.FlightId.Replace(" ", "").Substring(3, 5),
                NumberSuffix = flightSchedule.FlightId.Replace(" ", "").Substring(flightSchedule.FlightId.Length - 3),
                FlightType = flightSchedule.Type.Name,
                Departure = flightSchedule.Departure.ConvertToTargetTimeZone(SimplyTimeZoneExtension.GetSystemTimeZone(), flightSchedule.Origin.Timezone),
                Arrival = flightSchedule.Arrival.ConvertToTargetTimeZone(SimplyTimeZoneExtension.GetSystemTimeZone(), flightSchedule.Destination.Timezone),
                OriginIATA = flightSchedule.Origin.IATA,
                DestinationIATA = flightSchedule.Destination.IATA,
                Price = flightSchedule.Price
            };
        }

        private FlightSchedule MapFromFlightScheduleDTO(
            FlightScheduleDTO flightSchedule,
            (Airline? Airline, Aircraft? Aircraft, Airport? Origin, Airport? Destination, FlightType? FlightType) flightDetails
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
                Type = flightDetails.FlightType,
                Departure = flightSchedule.Departure,
                Arrival = flightSchedule.Arrival,
                Origin = flightDetails.Origin,
                Destination = flightDetails.Destination,
                Price = flightSchedule.Price
            };
        }

        private async Task<(Airline? Airline, Aircraft? Aircraft, Airport? Origin, Airport? Destination, FlightType? FlightType)> GetFlightDetailsAsync(
            FlightScheduleDTO flightSchedule
        )
        {
            Task<Airline?> airlineTask = _airlineRepository.GetByAsync(x => x.IATA == flightSchedule.AirlineIATA);
            Task<Aircraft> aircraftTask = _aircraftRepository.GetByAsync(x => x.RegistrationNumber == flightSchedule.AircraftRegistrationNumber);
            Task<Airport> originTask = _airportRepository.GetByAsync(x => x.IATA == flightSchedule.OriginIATA);
            Task<Airport> destinationTask = _airportRepository.GetByAsync(x => x.IATA == flightSchedule.DestinationIATA);
            Task<FlightType> flightTypeTask = _flightTypeRepository.GetByAsync(x => x.Name == flightSchedule.FlightType);

            await Task.WhenAll(
                airlineTask,
                aircraftTask,
                originTask,
                destinationTask,
                flightTypeTask);

            return (
                await airlineTask,
                await aircraftTask,
                await originTask,
                await destinationTask,
                await flightTypeTask
            );
        }

        private bool ValidateFlightScheduleDTO(FlightScheduleDTO flightSchedule)
        {
            if (!string.IsNullOrWhiteSpace(flightSchedule.AirlineIATA) &&
                !string.IsNullOrWhiteSpace(flightSchedule.AircraftRegistrationNumber) &&
                !string.IsNullOrWhiteSpace(flightSchedule.OriginIATA) &&
                !string.IsNullOrWhiteSpace(flightSchedule.DestinationIATA) &&
                !string.IsNullOrWhiteSpace(flightSchedule.FlightType) &&
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