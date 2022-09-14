using webapi_bilheteria_c.Domain.Interface;
using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ITokenService _tokenService;

        public UsersService(IUsersRepository usersRepository, ITokenService tokenService){
            _usersRepository = usersRepository;
            _tokenService = tokenService;
        }

        public TokenResponse Login(string email, string password){
            return _tokenService.GenerateToken(_usersRepository.AuthUser(email, password).Result);            
        }

        public void SignUp(Users user){
            var exist = String.IsNullOrEmpty(_usersRepository.GetUserByEmail(user.Email).Result) ? true : false;
            if (!exist) throw new Exception("204");
            _usersRepository.InsertUser(user);
        }

        public void SetPrivileges(int privileges, string email){
            _usersRepository.SetPrivileges(privileges, email);
        }
    }
}