using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi_bilheteria_c.Domain.Interface;
using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class EventController : MainController
    {
        private readonly IEventsService _eventsService;
        private readonly IMessageProducer _messageProducer;
        private readonly IPixClient _pixClient;
        private readonly ILogger<ILoggerService> _logger;

        public EventController(IEventsService eventsService, IPixClient pixClient,
            IMessageProducer messageProducer, ITokenService tokenService, ILogger<ILoggerService> logger) : base(tokenService)
        {
            _logger = logger;
            _eventsService = eventsService;
            _messageProducer = messageProducer;
            _pixClient = pixClient;
        }

        [HttpGet("get-event-by-uid")]
        public ActionResult GetEventByUid(string? uid)
        {
            try
            {
                return Ok(_eventsService.GetEventsByUid(uid));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("get-events-on-display")]
        public ActionResult GetEventsOnDisplay()
        {
            try
            {
                return Ok(_eventsService.GetEventsOnDisplay());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("get-events-on-display-by-company")]
        public ActionResult GetEventsOnDisplayByCompany(string? companyUid)
        {
            try
            {
                return Ok(_eventsService.GetEventsOnDisplayByCompany(companyUid));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("create-event")]
        public ActionResult CreateEvent(string? name, DateTime startsIn, DateTime endsIn,
            string? description, string? companyUid, int fullValue)
        {
            try
            {
                _eventsService.CreateEvent(new Events
                {
                    Name = name,
                    StartsIn = startsIn,
                    EndsIn = endsIn,
                    Description = description,
                    CompanyUid = companyUid,
                    Reason = string.Empty,
                    PublishedBy = TokenId
                }, fullValue);
                return Created("", new { Created = true });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}