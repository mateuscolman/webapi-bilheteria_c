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