using Microsoft.AspNetCore.Mvc;
using webapi_bilheteria_c.Domain.Interface;

namespace webapi_bilheteria_c.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        public UsersController(IUsersService usersService){
            _usersService = usersService;
        }

        [HttpPost("sign-in")]
        public ActionResult Login(string email, string password){
            try
            {
                return Ok(_usersService.GetUserByEmail(email, password));
            }
            catch (Exception ex)
            {
                
                return BadRequest(new {error = ex.Message});
            }
        }
    }
}