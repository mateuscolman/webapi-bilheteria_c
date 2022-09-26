using System.Data;
using Dapper;
using webapi_bilheteria_c.Domain.Interface;
using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Infra.Repository
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IDbConnection _dbConnection;

        public UsersRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Users> AuthUser(string email, string password)
        {
            var command = $@"
                select 
	                uid,
                    email,
                    password,
                    privileges,
                    created,
                    last_acess as LastAcess,
                    birthday,
                    name
                from user u
                where u.email = @email and u.password = @password";

            return await _dbConnection.QueryFirstOrDefaultAsync<Users>(command, new { email, password });
        }

        public async Task<string> GetUserByEmail(string email)
        {
            var command = $@"
                select 
	                uid
                from user u
                where u.email = @email";

            return await _dbConnection.QueryFirstOrDefaultAsync<string>(command, new { email });
        }

        public async Task InsertUser(Users user)
        {
            var command = $@"
                insert into user
                values(uuid(), @email, @password, 0, sysdate(), sysdate(), @birthday, @name)
            ";

            await _dbConnection.ExecuteAsync(command, new
            {
                email = user.Email,
                password = user.Password,
                birthday = user.Birthday,
                name = user.Name
            });
        }

        public async Task SetPrivileges(int privileges, string email)
        {
            var command = $@"
                update user
                set privileges = @privileges
                where email = @email
            ";

            await _dbConnection.ExecuteAsync(command, new { privileges, email });
        }
    }
}