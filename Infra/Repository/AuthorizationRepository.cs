using System.Data;
using Dapper;
using webapi_bilheteria_c.Domain.Interface;

namespace webapi_bilheteria_c.Infra.Repository
{
    public class AuthorizationRepository : IAuthorizationRepository
    {
        private readonly IDbConnection _dbConnection;
        
        public AuthorizationRepository(IDbConnection dbConnection){
            _dbConnection = dbConnection;
        }

        public async Task UpdateJWT(string? jwt, DateTime expiresIn, string? origin){
            var command = $@"
                update authorization_keys
                set bearer = @jwt,
                    expires_in = @expiresIn
                where origin = @origin
            ";

            await _dbConnection.ExecuteAsync(command, new {jwt, expiresIn, origin});
        }

        public async Task<string> SearchJWT(string? origin){
            var command = $@"
                select 
                    bearer
                from authorization_keys
                where 
                    origin = @origin
                    and expires_in > sysdate()
            ";
            return await _dbConnection.QueryFirstOrDefaultAsync<string>(command, new {origin});
        }
    }
}