using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi_bilheteria_c.Domain.Interface;
using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : MainController
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService, ITokenService tokenService) : base(tokenService)
        {
            _usersService = usersService;

        }

        [HttpPost("sign-in")]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        public ActionResult Login([FromBody] Login login)
        {
            try
            {
                return Ok(_usersService.Login(login.email, login.password));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("sign-up")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult SignUp([FromBody] Users user)
        {
            try
            {
                _usersService.SignUp(user);
                return Created("", new { Created = true });
            }
            catch (Exception ex)
            {
                switch (ex.Message)
                {
                    case "204":
                        return NoContent();

                    default:
                        return BadRequest(new { error = ex.Message });
                }

            }
        }

        [HttpPut("set-privileges")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public ActionResult SetPrivileges(int privileges, string email)
        {
            try
            {
                _usersService.SetPrivileges(privileges, email);
                return Accepted("", new { Accepted = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}