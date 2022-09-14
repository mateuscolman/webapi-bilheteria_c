using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Domain.Interface
{
    public interface ICompanyRepository
    {
        Task<Company> GetCompanyByOwner(string? uid);
        Task CreateCompany(Company company);
    }
}