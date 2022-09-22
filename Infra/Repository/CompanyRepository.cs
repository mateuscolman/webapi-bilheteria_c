using System.Data;
using Dapper;
using webapi_bilheteria_c.Domain.Interface;
using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Infra.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly IDbConnection _dbConnection;

        public CompanyRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<List<Company>> GetCompanyByOwner(string? ownerUid)
        {
            var command = $@"
                select 
                    uid,
                    description,
                    created,
                    name,
                    active,
                    owner_uid as OwnerUid
                from company
                where 
                    owner_uid = @ownerUid
                    and exclusion_logic = 0
            ";

            var company = await _dbConnection.QueryAsync<Company>(command, new { ownerUid });
            return company.ToList();
        }

        public async Task<Company> GetCompanyByUid(string? uid)
        {
            var command = $@"
                select 
                    uid,
                    description,
                    created,
                    name,
                    active,
                    owner_uid as OwnerUid
                from company
                where uid = @uid
            ";

            return await _dbConnection.QueryFirstOrDefaultAsync<Company>(command, new { uid });
        }

        public async Task<bool> CreateCompany(Company company)
        {
            if (!ValidInsert(company.Name, company.OwnerUid).Result) throw new Exception("Company already exist");
            var command = $@"
                insert into company
                values(uuid(), @description, sysdate(), @name, 1, @ownerUid, 0)
            ";

            var teste = await _dbConnection.ExecuteAsync(command, new
            {
                description = company.Description,
                name = company.Name,
                ownerUid = company.OwnerUid
            });

            return true;
        }

        public async Task EditCompany(Company company)
        {
            var command = $@"
                update company
                set 
                    description = @description,
                    name = @name,
                    active = @active,
                    exclusion_logic = @exclusionLogic
                where uid = @uid            
            ";

            await _dbConnection.ExecuteAsync(command, new
            {
                description = company.Description,
                name = company.Name,
                active = company.Active,
                uid = company.Uid,
                exclusionLogic = company.ExclusionLogic
            });
        }

        public async Task<bool> InsertPaymentMethod(CompanyPaymentMethod companyPaymentMethod)
        {
            var command = $@"
                insert into company_payment_methods
                values(uuid(), @companyUid, @name, @paymentKey)
            ";
            await _dbConnection.ExecuteAsync(command, new
            {
                companyUid = companyPaymentMethod.CompanyUid,
                name = companyPaymentMethod.Name,
                paymentKey = companyPaymentMethod.PaymentKey
            });

            return true;
        }

        public async Task<List<CompanyPaymentMethod>> GetPaymentMethod(string companyUid)
        {
            var command = $@"
                select 
                    uid,
                    company_uid as companyUid,
                    name,
                    payment_key as paymentKey
                from company_payment_methods
                where company_uid = @companyUid
            ";

            var companyPaymentMethods = await _dbConnection.QueryAsync<CompanyPaymentMethod>(command, new { companyUid });
            return companyPaymentMethods.ToList();
        }

        private async Task<bool> ValidInsert(string? name, string? ownerUid)
        {
            var command = $@"
                select 
                    uid
                from company
                where 
                    name = @name
                    and owner_uid = @ownerUid
                    and exclusion_logic = 0
            ";

            var uid = await _dbConnection.QueryFirstOrDefaultAsync<string>(command, new { name, ownerUid });
            return String.IsNullOrEmpty(uid) ? true : false;
        }
    }
}