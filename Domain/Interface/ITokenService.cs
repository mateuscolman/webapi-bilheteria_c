using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Domain.Interface
{
    public interface ITokenService
    {
        TokenResponse GenerateToken(Users user);
        string? GetTokenKey(string? token, string key, bool required = true);
    }
}