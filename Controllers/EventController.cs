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
        public EventController(IEventsService eventsService, ITokenService tokenService): base(tokenService)
        {            
            _eventsService = eventsService;
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