using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Domain.Interface
{
    public interface ICompanyService
    {
        Company GetCompanyByUid(string? uid);
        List<Company> GetCompanyByOwner(string? uid);
        bool CreateCompany(Company company);
        void EditCompany(Company company);
        bool InsertPaymentMethod(CompanyPaymentMethod companyPaymentMethod);
        List<CompanyPaymentMethod> GetPaymentMethod(string companyUid);
    }
}