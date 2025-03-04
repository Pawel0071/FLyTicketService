using FLyTicketService.DTO;
using FLyTicketService.Infrastructure;
using FLyTicketService.Service.Interfaces;
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
            OperationResult<bool> result = await _ticketService.ReserveTicketAsync(flightId, seatNo, tenantId);
            return StatusCode(result.Status.ToInt(), result.Message);
        }

        [HttpPost("sell")]
        public async Task<IActionResult> SellTicket([FromQuery] string flightId, [FromQuery] string seatNo, [FromQuery] Guid tenantId, [FromQuery] decimal discount)
        {
            OperationResult<bool> result = await _ticketService.SoldTicketAsync(flightId, seatNo, tenantId, discount);
            return result.GetResult();
        }

        [HttpPost("sell-reserved")]
        public async Task<IActionResult> SellReservedTicket([FromQuery] string ticketNumber, [FromQuery] decimal discount)
        {
            OperationResult<bool> result = await _ticketService.SoldReservedTicketAsync(ticketNumber, discount);
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
        public async Task<IActionResult> GetTickets([FromQuery] string? flyNumber, [FromQuery] Guid? tenantId)
        {
            OperationResult<IEnumerable<TicketDTO>> result = await _ticketService.GetTicketsAsync(flyNumber, tenantId);
            return result.GetResult();
        }

        #endregion
    }
}