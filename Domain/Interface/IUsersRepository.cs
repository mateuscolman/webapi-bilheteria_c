using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Domain.Interface
{
    public interface IUsersRepository
    {
        Task<Users> AuthUser(string email, string password);
        Task InsertUser(Users user);
        Task<string> GetUserByEmail(string email);
        Task SetPrivileges(int privileges, string email);
    }
}