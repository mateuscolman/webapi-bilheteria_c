using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Domain.Interface
{
    public interface IUsersService
    {
        TokenResponse GetUserByEmail(string email, string password);
    }
}