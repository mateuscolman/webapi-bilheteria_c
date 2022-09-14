using webapi_bilheteria_c.Domain.Interface;
using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyService(ICompanyRepository companyRepository){
            _companyRepository = companyRepository;
        }

        public Company GetCompanyByOwner(string? uid){
            return _companyRepository.GetCompanyByOwner(uid).Result;
        }
        
        public void CreateCompany(Company company){
            _companyRepository.CreateCompany(company);
        }
    }
}