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

        public TokenResponse GetUserByEmail(string email, string password){
            return _tokenService.GenerateToken(_usersRepository.GetUserByEmail(email, password).Result);
        }
    }
}