using FLyTicketService.DTO;
using FLyTicketService.Mapper;
using FLyTicketService.Model;
using FLyTicketService.Model.Enums;
using FLyTicketService.Repositories.Interfaces;
using FLyTicketService.Service.Interfaces;
using FLyTicketService.Shared;

namespace FLyTicketService.Service
{
    public class TicketService: ITicketService
    {
        #region Fields

        private readonly int _maxReservationIDays = 30;
        private OperationResult<TicketDTO> _operationResult;

        private readonly IGenericRepository<Ticket> _ticketRepository;
        private readonly IGenericRepository<Tenant> _tenantRepository;
        private readonly IGenericRepository<FlightSeat> _flightSeatRepository;
        private readonly IGenericRepository<FlightSchedule> _flightScheduleRepository;
        private readonly IGroupStrategyFactory _strategyFactory;
        private readonly IFlightPriceService _flightPriceService;
        private readonly ILogger<TicketService> _logger;

        #endregion

        #region Constructors

        public TicketService(
            IGenericRepository<Ticket> ticketRepository,
            ILogger<TicketService> logger,
            IGroupStrategy groupStrategy,
            IGenericRepository<FlightSeat> flightSeatRepository,
            IGenericRepository<Tenant> tenantRepository,
            IFlightPriceService flightPriceService,
            IGroupStrategyFactory groupStrategies,
            IGenericRepository<FlightSchedule> flightScheduleRepository
        )
        {
            _ticketRepository = ticketRepository;
            _logger = logger;
            _flightSeatRepository = flightSeatRepository;
            _tenantRepository = tenantRepository;
            _flightPriceService = flightPriceService;
            _strategyFactory = groupStrategies;
            _flightScheduleRepository = flightScheduleRepository;
        }

        #endregion

        #region Methods

        public async Task<OperationResult<TicketDTO?>> ReserveTicketAsync(string flightId, string seatNo, Guid tenantId)
        {
            Ticket? ticket = await ReserveTicketInternalAsync(flightId, seatNo, tenantId);
            if (ticket == null)
            {
                return _operationResult ?? new OperationResult<TicketDTO?>(OperationStatus.InternalServerError, "Internal Error", null);
            }

            return new OperationResult<TicketDTO?>(OperationStatus.Ok, string.Empty, ticket.ToDTO());

        }

        public async Task<OperationResult<TicketDTO?>> SaleTicketAsync(string flightId, string seatNo, Guid tenantId, IEnumerable<DiscountDTO> discounts)
        {
            Ticket? ticket = await ReserveTicketInternalAsync(flightId, seatNo, tenantId);
            if (ticket == null)
            {
                return _operationResult ?? new OperationResult<TicketDTO?>(OperationStatus.InternalServerError, "Internal Error", null);
            }

            List<DiscountDTO> applicableDiscounts = GetApplicableDiscounts(discounts, ticket);

            await ApplyDiscountAsync(flightId, applicableDiscounts);

            ticket.Status = TicketStatus.Sold;

            bool result = await _ticketRepository.UpdateAsync(ticket);

            return new OperationResult<TicketDTO>(
                result
                    ? OperationStatus.Ok
                    : OperationStatus.InternalServerError,
                result
                    ? "Ticket sold successfully"
                    : "Failed to sale ticket",
                ticket.ToDTO());
        }

        public async Task<OperationResult<TicketDTO?>> SaleReservedTicketAsync(string ticketNumber)
        {
            Ticket? ticket = await GetTicketInternalAsync(ticketNumber);
            if (ticket == null)
            {
                _logger.LogWarning("Ticket not found");
                return new OperationResult<TicketDTO>(OperationStatus.NotFound, "Ticket not found", null);
            }

            ticket.Status = TicketStatus.Sold;

            bool result = await _ticketRepository.UpdateAsync(ticket);

            return new OperationResult<TicketDTO>(
                result
                    ? OperationStatus.Ok
                    : OperationStatus.InternalServerError,
                result
                    ? "Ticket sold successfully"
                    : "Failed to sale ticket",
                ticket.ToDTO());
        }

        public async Task<OperationResult<IEnumerable<DiscountDTO>>> GetAllApplicableDiscountsAsync(string ticketNumber)
        {
            Ticket? ticket = await GetTicketInternalAsync(ticketNumber);
            if (ticket == null)
            {
                _logger.LogWarning("Ticket not found");
                return new OperationResult<IEnumerable<DiscountDTO>>(OperationStatus.NotFound, "Ticket not found", new List<DiscountDTO>());
            }

            List<Discount> discounts = await _flightPriceService.GetAllApplicableDiscountsAsync(ticket);

            return new OperationResult<IEnumerable<DiscountDTO>>(OperationStatus.Ok, "Discounts found", discounts.Select(x => x.ToDTO()));
        }

        public async Task<OperationResult<IEnumerable<DiscountDTO>>> GetAllDiscountsAsync()
        {

            List<Discount> discounts = await _flightPriceService.GetAllDiscountsAsync();

            return new OperationResult<IEnumerable<DiscountDTO>>(OperationStatus.Ok, "Discounts found", discounts.Select(x => x.ToDTO()));
        }

        public async Task<OperationResult<bool>> ApplyDiscountAsync(string ticketNumber, IEnumerable<DiscountDTO> discounts)
        {
            Ticket? ticket = await GetTicketInternalAsync(ticketNumber);
            if (ticket == null)
            {
                _logger.LogWarning("Ticket not found");
                return new OperationResult<bool>(OperationStatus.NotFound, "Ticket not found", false);
            }

            List<DiscountDTO> applicableDiscounts = GetApplicableDiscounts(discounts, ticket);

            IGroupStrategy strategy = _strategyFactory.GetStrategy(ticket.Tenant.Group);
            (decimal Price, decimal Discount) discount = strategy.ApplyDiscountBasedOnTenantGroup(applicableDiscounts.Select(x => x.ToDomain()), ticket);

            ticket.Price = discount.Price;
            ticket.Discounts = strategy.GetListBasedOnTenantGroup(applicableDiscounts.Select(x => x.ToDomain()));

            bool result = await _ticketRepository.UpdateAsync(ticket);

            return new OperationResult<bool>(
                result
                    ? OperationStatus.Ok
                    : OperationStatus.InternalServerError,
                result
                    ? "Discount applied successfully"
                    : "Failed to apply discount",
                result);
        }

        public async Task<OperationResult<bool>> CanDiscountAppliedAsync(string ticketNumber, DiscountDTO discounts)
        {
            Ticket? ticket = await GetTicketInternalAsync(ticketNumber);
            if (ticket == null)
            {
                _logger.LogWarning("Ticket not found");
                return new OperationResult<bool>(OperationStatus.NotFound, "Ticket not found", false);
            }

            bool result = _flightPriceService.IsDiscountApplicable(discounts.ToDomain(), ticket);

            return new OperationResult<bool>(
                result
                    ? OperationStatus.Ok
                    : OperationStatus.InternalServerError,
                result
                    ? "Discount can be applied"
                    : "Discount can not be applied",
                result);
        }

        public async Task<OperationResult<bool>> CancelTicketAsync(string ticketNumber)
        {
            Ticket? ticket = await GetTicketInternalAsync(ticketNumber);
            if (ticket == null)
            {
                _logger.LogWarning("Ticket not found");
                return new OperationResult<bool>(OperationStatus.NotFound, "Ticket not found", false);
            }

            bool result = await _ticketRepository.RemoveAsync(ticket.TicketId);

            return new OperationResult<bool>(
                result
                    ? OperationStatus.Ok
                    : OperationStatus.InternalServerError,
                result
                    ? "Ticket canceled successfully"
                    : "Failed to canceled ticket",
                result);
        }

        public async Task<OperationResult<TicketDTO?>> GetTicketAsync(string ticketNumber)
        {
            Ticket? ticket = await GetTicketInternalAsync(ticketNumber);
            if (ticket == null)
            {
                _logger.LogWarning("Ticket not found");
                return new OperationResult<TicketDTO?>(OperationStatus.NotFound, "Ticket not found", null);
            }

            TicketDTO results = ticket.ToDTO();

            return new OperationResult<TicketDTO?>(OperationStatus.Ok, "Ticket found", results);
        }

        public async Task<OperationResult<IEnumerable<TicketDTO>>> GetTicketListByAsync(
            string? flyNumber,
            Guid? tenantId,
            DateTime? Departure,
            string? OriginIATA,
            string? DestinationIATA
        )
        {
            IEnumerable<Ticket> tickets = await _ticketRepository.FilterByAsync(
                x =>
                    (flyNumber == null || x.FlightSeat.FlightSchedule.FlightId == flyNumber) &&
                    (tenantId == null || x.Tenant.TenantId == tenantId) &&
                    (Departure == null || x.FlightSeat.FlightSchedule.Departure == Departure) &&
                    (OriginIATA == null || x.FlightSeat.FlightSchedule.Origin.IATA == OriginIATA) &&
                    (DestinationIATA == null || x.FlightSeat.FlightSchedule.Destination.IATA == DestinationIATA));

            List<TicketDTO> ticketDTOs = tickets.Select(t => t.ToDTO()).ToList();

            return new OperationResult<IEnumerable<TicketDTO>>(OperationStatus.Ok, string.Empty, ticketDTOs);
        }

        private async Task<Ticket?> GetTicketInternalAsync(string ticketNumber)
        {
            return await _ticketRepository.GetByAsync(x => x.TicketNumber == ticketNumber);
        }

        private async Task<Ticket?> ReserveTicketInternalAsync(string flightId, string seatNo, Guid tenantId)
        {
            FlightSchedule? flightSchedule = await _flightScheduleRepository.GetByAsync(x => x.FlightId == flightId);
            if (flightSchedule == null)
            {
                _logger.LogWarning("Flight not found");
                _operationResult = new OperationResult<TicketDTO>(OperationStatus.NotFound, "Flight not found", null); 
                return null;
            }

            _logger.LogInformation($"Flight found: {flightSchedule.FlightId}, Seats count: {flightSchedule.Seats?.Count ?? 0}");

            // Initialize seats if they don't exist
            if (flightSchedule.Seats == null || !flightSchedule.Seats.Any())
            {
                _logger.LogWarning($"Flight {flightSchedule.FlightId} has no seats initialized. Initializing seats from aircraft.");
                
                if (flightSchedule.Aircraft?.Seats != null && flightSchedule.Aircraft.Seats.Any())
                {
                    List<FlightSeat> flightSeats = new List<FlightSeat>();
                    foreach (AircraftSeat aircraftSeat in flightSchedule.Aircraft.Seats)
                    {
                        FlightSeat newSeat = new FlightSeat
                        {
                            FlightSchedule = flightSchedule,
                            Class = aircraftSeat.Class,
                            SeatNumber = aircraftSeat.SeatNumber,
                            IsAvailable = !aircraftSeat.OutOfService
                        };
                        flightSeats.Add(newSeat);
                        await _flightSeatRepository.AddAsync(newSeat);
                    }
                    flightSchedule.Seats = flightSeats;
                    _logger.LogInformation($"Initialized {flightSeats.Count} seats for flight {flightSchedule.FlightId}");
                }
                else
                {
                    _logger.LogError($"Cannot initialize seats - Aircraft or Aircraft.Seats is null for flight {flightSchedule.FlightId}");
                    _operationResult = new OperationResult<TicketDTO>(OperationStatus.InternalServerError, "Flight seats not available", null);
                    return null;
                }
            }

            Tenant? tenant = await _tenantRepository.GetByIdAsync(tenantId);
            if (tenant == null)
            {
                _logger.LogWarning("Tenant not found");
                _operationResult = new OperationResult<TicketDTO>(OperationStatus.NotFound, "Tenant not found", null);
                return null;
            }

            FlightSeat? seat = flightSchedule.Seats?.FirstOrDefault(x => x.IsAvailable && x.SeatNumber == seatNo);
            if (seat == null)
            {
                _logger.LogWarning($"Seat {seatNo} not available for flight {flightSchedule.FlightId}");
                _operationResult = new OperationResult<TicketDTO>(OperationStatus.NotFound, "Seat not available", null);
                return null;
            }

            Ticket? existingTicket = await _ticketRepository.GetByAsync(x => x.FlightSeat.FlightSeatId == seat.FlightSeatId);
            if (existingTicket != null)
            {
                seat = flightSchedule.Seats?.FirstOrDefault(x => x.SeatNumber == seatNo);
                if (seat == null)
                {
                    _logger.LogWarning("Seat not available");
                    _operationResult = new OperationResult<TicketDTO>(OperationStatus.NotFound, "Seat not available", null);
                    return null;
                }

                if (existingTicket.Status == TicketStatus.Reserved && existingTicket.ReleaseDate > DateTime.Now)
                {
                    _logger.LogWarning("Seat already reserved");
                    _operationResult = new OperationResult<TicketDTO>(OperationStatus.Conflict, "Seat already reserved", null);
                    return null;
                }
            }

            Ticket ticket = new Ticket
            {
                TicketNumber = Guid.NewGuid().ToString().Replace("-", ""),
                FlightSeat = seat,
                Tenant = tenant,
                Status = TicketStatus.Reserved,
                Price = flightSchedule.Price,
                ReleaseDate = DateTime.Now.AddDays(_maxReservationIDays)
            };

            bool result = await _ticketRepository.AddAsync(ticket);

            _operationResult = new OperationResult<TicketDTO>(
                result
                    ? OperationStatus.Created
                    : OperationStatus.InternalServerError,
                result
                    ? "Ticket reserved successfully"
                    : "Failed to reserve ticket",
                ticket.ToDTO());

            return ticket;
        }
        private List<DiscountDTO> GetApplicableDiscounts(IEnumerable<DiscountDTO> discounts, Ticket ticket)
        {
            List<DiscountDTO> applicableDiscounts = new List<DiscountDTO>();

            foreach (var discount in discounts)
            {
                if (_flightPriceService.IsDiscountApplicable(discount.ToDomain(), ticket))
                {
                    applicableDiscounts.Add(discount);
                }
            }

            return applicableDiscounts;
        }

        #endregion
    }
}