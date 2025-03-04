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
            Airline airline = await _airlineRepository.GetByAsync(x => x.IATA == flightSchedule.AirlineIATA);
            Aircraft aircraft = await _aircraftRepository.GetByAsync(x => x.RegistrationNumber == flightSchedule.AircraftRegistrationNumber);
            Airport origin = await _airportRepository.GetByAsync(x => x.IATA == flightSchedule.OriginIATA);
            Airport destination = await _airportRepository.GetByAsync(x => x.IATA == flightSchedule.DestinationIATA);
            FlightType flightType = await _flightTypeRepository.GetByAsync(x => x.Name == flightSchedule.FlightType);

            FlightSchedule flight = MapFromFlightScheduleDTO(flightSchedule, airline, aircraft, origin, destination, flightType);

            bool result = await _flightScheduleRepository.AddAsync(flight);
            return new OperationResult<bool>(OperationStatus.Created, string.Empty, result);
        }

        public async Task<OperationResult<bool>> ScheduleRecurringFlightAsync(FlightScheduleDTO flightSchedule, DaysOfWeekMask dayOfWeek, int numberOfOccuring)
        {
            Airline airline = await _airlineRepository.GetByAsync(x => x.IATA == flightSchedule.AirlineIATA);
            Aircraft aircraft = await _aircraftRepository.GetByAsync(x => x.RegistrationNumber == flightSchedule.AircraftRegistrationNumber);
            Airport origin = await _airportRepository.GetByAsync(x => x.IATA == flightSchedule.OriginIATA);
            Airport destination = await _airportRepository.GetByAsync(x => x.IATA == flightSchedule.DestinationIATA);
            FlightType flightType = await _flightTypeRepository.GetByAsync(x => x.Name == flightSchedule.FlightType);
            
            List<Task<bool>> tasks = new List<Task<bool>>();
            for (int i = 0; i < numberOfOccuring; i++)
            {
                FlightSchedule flight = MapFromFlightScheduleDTO(flightSchedule, airline, aircraft, origin, destination, flightType);
                tasks.Add(_flightScheduleRepository.AddAsync(flight));
            }

            bool[] results = await Task.WhenAll(tasks);
            return new OperationResult<bool>(OperationStatus.Created, string.Empty, results.All(x => x));
        }

        public async Task<OperationResult<bool>> UpdateFlightAsync(Guid flightScheduleId, FlightScheduleDTO flightSchedule, bool allRecurrences)
        {
            FlightSchedule? flight = await _flightScheduleRepository.GetByIdAsync(flightScheduleId);
            if (flight == null)
            {
                return new OperationResult<bool>(false, false);
            }

            flight.Airline = new Airline { IATA = flightSchedule.AirlineIATA };
            flight.Aircraft = new Aircraft { RegistrationNumber = flightSchedule.AircraftRegistrationNumber };
            flight.FlightId = flightSchedule.Number + flightSchedule.NumberSuffix;
            flight.Type = new FlightType { Name = flightSchedule.FlightType };
            flight.Departure = flightSchedule.Departure;
            flight.Arrival = flightSchedule.Arrival;
            flight.Origin = new Airport { IATA = flightSchedule.OriginIATA };
            flight.Destination = new Airport { IATA = flightSchedule.DestinationIATA };
            flight.Price = flightSchedule.Price;

            bool result = await _flightScheduleRepository.UpdateAsync(flight);
            return new OperationResult<bool>(result, result);
        }

        public async Task<OperationResult<bool>> DeleteFlightAsync(Guid flightScheduleId, FlightScheduleDTO flightSchedule, bool allRecurrences)
        {
            bool result = await _flightScheduleRepository.RemoveAsync(flightScheduleId);
            return new OperationResult<bool>(result, result);
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

        private FlightSchedule MapFromFlightScheduleDTO(FlightScheduleDTO flightSchedule, Airline airline, Aircraft aircraft, Airport origin, Airport destination, FlightType flightType)
        {
            List<FlightSeat> flightSeats = new List<FlightSeat>();

            foreach (AircraftSeat aircraftSeat in aircraft.Seats)
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
                Airline = airline,
                Aircraft = aircraft,
                FlightId = flightSchedule.Number + flightSchedule.NumberSuffix,
                Seats = flightSeats,
                Type = flightType,
                Departure = flightSchedule.Departure,
                Arrival = flightSchedule.Arrival,
                Origin = origin,
                Destination = destination,
                Price = flightSchedule.Price
            };
        }

        #endregion
    }
}