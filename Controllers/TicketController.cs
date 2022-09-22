using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi_bilheteria_c.Domain.Interface;
using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class TicketController : MainController
    {
        private readonly ILogger<ILoggerService> _logger;
        private readonly ITicketService _ticketService;
        public TicketController(ILogger<ILoggerService> logger, ITokenService tokenService,
            ITicketService ticketService) : base(tokenService)
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        [HttpPost("ticket-order/{eventUid}")]
        public ActionResult InsertTicketOrder([FromBody] List<TicketRelatedPersons> ticketRelatedPersons,
            [FromRoute] string? eventUid)
        {
            try
            {
                return Created("", _ticketService.InsertTicketOrder(ticketRelatedPersons, eventUid));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}