using webapi_bilheteria_c.Domain.Interface;
using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Services.Provider
{
    public class CredentialsProvider
    {
        private readonly ICredentialsRepository _credentialsRepository;

        public CredentialsProvider(ICredentialsRepository credentialsRepository){
            _credentialsRepository = credentialsRepository;
        }

        public List<Credentials> GetCredentialsFromDB(){
            return _credentialsRepository.GetCredentialsFromDB().Result;
        }
    }
}