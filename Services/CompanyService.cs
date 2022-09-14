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

        public List<Company> GetCompanyByOwner(string? ownerUid){
            return _companyRepository.GetCompanyByOwner(ownerUid).Result;
        }
        
        public Company GetCompanyByUid(string? uid){
            return _companyRepository.GetCompanyByUid(uid).Result;
        }

        public bool CreateCompany(Company company){
            return _companyRepository.CreateCompany(company).Result;    
        }

        public void EditCompany(Company company){
            _companyRepository.EditCompany(company);
        }
    }
}