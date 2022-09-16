using Dapper;
using System.Data;
using webapi_bilheteria_c.Domain.Interface;
using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Infra.Repository
{
    public class CredentialsRepository : ICredentialsRepository
    {
        private readonly IDbConnection _dbConnection;

        public CredentialsRepository(IDbConnection dbConnection){
            _dbConnection = dbConnection;
        }
        
        public async Task<List<Credentials>> GetCredentialsFromDB(){
            var command = $@"
                select
                    id,
                    origin,
                    value1,
                    value2
                from credentials
            ";
            var credentials = await _dbConnection.QueryAsync<Credentials>(command);
            return credentials.ToList();
        }
    }
}