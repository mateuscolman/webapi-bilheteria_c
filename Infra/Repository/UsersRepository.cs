using System.Data;
using Dapper;
using webapi_bilheteria_c.Domain.Interface;
using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Infra.Repository
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IDbConnection _dbConnection;

        public UsersRepository(IDbConnection dbConnection){
            _dbConnection = dbConnection;
        }

        public async Task<Users> GetUserByEmail(string email, string password){
            var command = $@"
                select 
	                uid,
                    email,
                    password,
                    created,
                    last_acess as LastAcess,
                    birthday,
                    name
                from users u
                where u.email = @email and u.password = @password";
            
            return await _dbConnection.QueryFirstOrDefaultAsync<Users>(command, new {email, password});
        }
    }
}