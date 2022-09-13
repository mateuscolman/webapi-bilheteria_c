using System.Data;
using Dapper;
using webapi_bilheteria_c.Domain.Interface;
using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Infra.Repository
{
    public class ParametersRepository : IParametersRepository
    {
        private readonly IDbConnection _dbConnection;

        public ParametersRepository(IDbConnection dbConnection){
            _dbConnection = dbConnection;
        }

        public async Task<List<Parameters>> GetParametersFromDB(){
            var command = $@"
                select 
                    code,
                    description,
                    value,
                    created,
                    active
                from parameters p
                where p.active = 1";
            
            var parametersList = await _dbConnection.QueryAsync<Parameters>(command);
            return parametersList.ToList();
        }
    }
}