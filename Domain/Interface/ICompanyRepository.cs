using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Domain.Interface
{
    public interface ICompanyRepository
    {
        Task<List<Company>> GetCompanyByOwner(string? ownerUid);
        Task<Company> GetCompanyByUid(string? uid);
        Task<bool> CreateCompany(Company company);
        Task EditCompany(Company company);
    }
}