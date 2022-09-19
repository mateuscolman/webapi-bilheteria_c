
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
            IMessageProducer messageProducer, ITokenService tokenService, ILogger<ILoggerService> logger): base(tokenService)
        {            
            _logger = logger;
            _eventsService = eventsService;
            _messageProducer = messageProducer;
            _pixClient = pixClient;
        }

        [AllowAnonymous]
        [HttpPost("test")]
        public ActionResult PostTest(){
            try
            {       
                _logger.LogInformation("Teste");         
                return Accepted (_pixClient.GenerateCharge(new Ticket{
                    EventName = "teste",
                    Value = "0.10",
                    PayerDocument = "41668971879",
                    PayerName = "Mateus Colman"                    
                }, "71cdf9ba-c695-4e3c-b010-abb521a3f1be") .Result);                                            
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        [HttpGet("get-event-by-uid")]
        public ActionResult GetEventByUid(string? uid){
            try
            {
                return Ok(_eventsService.GetEventsByUid(uid));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);                
            }
        }

        [HttpGet("get-events-on-display")]
        public ActionResult GetEventsOnDisplay(){
            try
            {
                return Ok(_eventsService.GetEventsOnDisplay());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);   
            }
        }

        [HttpGet("get-events-on-display-by-company")]
        public ActionResult GetEventsOnDisplayByCompany(string? companyUid){
            try
            {
                return Ok(_eventsService.GetEventsOnDisplayByCompany(companyUid));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);                
            }
        }

        [HttpPost("insert-event")]
        public ActionResult InsertEvent(string? name, DateTime startsIn, DateTime endsIn, 
            string? description, string? companyUid){
            try
            {
                _eventsService.InsertEvent(new Events{
                   Name = name,
                   StartsIn = startsIn,
                   EndsIn = endsIn,
                   Description = description,
                   CompanyUid = companyUid,
                   Reason = string.Empty, 
                   PublishedBy = TokenId 
                });
                return Created("", new {Created = true});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }        
        }
    }
}