using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Domain.Interface
{
    public interface ICompanyService
    {
        Company GetCompanyByOwner(string uid);
        void CreateCompany(Company company);
    }
}