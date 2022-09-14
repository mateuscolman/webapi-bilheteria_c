using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Domain.Interface
{
    public interface IUsersService
    {
        TokenResponse Login(string email, string password);
        void SignUp(Users user);
        void SetPrivileges(int privileges, string email);
    }
}