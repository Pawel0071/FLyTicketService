using FLyTicketService.DTO;
using FLyTicketService.Service.Interfaces;
using FLyTicketService.Shared;
using Microsoft.AspNetCore.Mvc;

namespace FLyTicketService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController: ControllerBase
    {
        #region Fields

        private readonly ITicketService _ticketService;

        #endregion

        #region Constructors

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        #endregion

        #region Methods

        [HttpPost("reserve")]
        public async Task<IActionResult> ReserveTicket([FromQuery] string flightId, [FromQuery] string seatNo, [FromQuery] Guid tenantId)
        {
            OperationResult<TicketDTO?> result = await _ticketService.ReserveTicketAsync(flightId, seatNo, tenantId);
            return result.GetResult();
        }

        [HttpPost("sell")]
        public async Task<IActionResult> SellTicket(
            [FromQuery] string flightId,
            [FromQuery] string seatNo,
            [FromQuery] Guid tenantId,
            [FromBody] IEnumerable<DiscountDTO> discounts
        )
        {
            OperationResult<TicketDTO?> result = await _ticketService.SaleTicketAsync(flightId, seatNo, tenantId, discounts);
            return result.GetResult();
        }

        [HttpPost("sell-reserved")]
        public async Task<IActionResult> SellReservedTicket([FromQuery] string ticketNumber)
        {
            OperationResult<TicketDTO?> result = await _ticketService.SaleReservedTicketAsync(ticketNumber);
            return result.GetResult();
        }

        [HttpDelete("{ticketNumber}")]
        public async Task<IActionResult> CancelTicket(string ticketNumber)
        {
            OperationResult<bool> result = await _ticketService.CancelTicketAsync(ticketNumber);
            return result.GetResult();
        }

        [HttpGet("{ticketNumber}")]
        public async Task<IActionResult> GetTicket(string ticketNumber)
        {
            OperationResult<TicketDTO?> result = await _ticketService.GetTicketAsync(ticketNumber);
            return result.GetResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetTickets(
            [FromQuery] string? flyNumber,
            [FromQuery] Guid? tenantId,
            [FromQuery] DateTime? departure,
            [FromQuery] string? originIATA,
            [FromQuery] string? destinationIATA
        )
        {
            var result = await _ticketService.GetTicketListByAsync(
                flyNumber,
                tenantId,
                departure,
                originIATA,
                destinationIATA);

            return result.GetResult();
        }

        [HttpGet("discounts/{ticketNumber}")]
        public async Task<IActionResult> GetAllApplicableDiscounts(string ticketNumber)
        {
            OperationResult<IEnumerable<DiscountDTO>> result = await _ticketService.GetAllApplicableDiscountsAsync(ticketNumber);
            return result.GetResult();
        }

        [HttpPost("apply-discount")]
        public async Task<IActionResult> ApplyDiscount([FromQuery] string ticketNumber, [FromBody] IEnumerable<DiscountDTO> discounts)
        {
            OperationResult<bool> result = await _ticketService.ApplyDiscountAsync(ticketNumber, discounts);
            return result.GetResult();
        }

        [HttpGet("can-apply-discount/{ticketNumber}")]
        public async Task<IActionResult> CanDiscountBeApplied(string ticketNumber, [FromQuery] DiscountDTO discount)
        {
            OperationResult<bool> result = await _ticketService.CanDiscountAppliedAsync(ticketNumber, discount);
            return result.GetResult();

            #endregion
        }
    }
}