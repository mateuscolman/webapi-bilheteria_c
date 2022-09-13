using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Domain.Interface
{
    public interface IUsersRepository
    {
        Task<Users> GetUserByEmail(string email, string password);
    }
}