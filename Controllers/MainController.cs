using Microsoft.AspNetCore.Mvc;
using webapi_bilheteria_c.Domain.Interface;

namespace webapi_bilheteria_c.Controllers
{
    public class MainController : ControllerBase
    {
        public readonly ITokenService _tokenService;

        public MainController(ITokenService tokenService){
            _tokenService = tokenService;
        }

        public string? Token => Request.Headers["Authorization"];

        public string? TokenId => _tokenService.GetTokenKey(Token, "Id") == null ? throw new UnauthorizedAccessException("Sessão de usuário ínvalida")
            : _tokenService.GetTokenKey(Request.Headers["Authorization"], "Id");
    }    
}