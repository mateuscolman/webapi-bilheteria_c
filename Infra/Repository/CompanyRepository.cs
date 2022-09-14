using System.Data;
using Dapper;
using webapi_bilheteria_c.Domain.Interface;
using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Infra.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly IDbConnection _dbConnection;

        public CompanyRepository(IDbConnection dbConnection){
            _dbConnection = dbConnection;
        }

        public async Task<Company> GetCompanyByOwner(string? uid){
            var command = $@"
                select 
                    uid,
                    description,
                    created,
                    name,
                    active,
                    owner_uid as OwnerUid
                from company
                where owner_uid = @uid
            ";

            return await _dbConnection.QueryFirstOrDefaultAsync<Company>(command, new {uid});
        } 

        public async Task CreateCompany(Company company){
            var command = $@"
                insert into company
                values(uuid(), @description, sysdate(), @name, 1, @ownerUid)
            ";

            await _dbConnection.ExecuteAsync(command, new {
                description = company.Description,
                name = company.Name,
                ownerUid = company.OwnerUid
            });
        }
    }
}