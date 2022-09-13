using webapi_bilheteria_c.Domain.Interface;
using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Services.Provider
{
    public class ParametersProvider
    {
        private readonly IParametersRepository _parametersRepository;

        public ParametersProvider(IParametersRepository parametersRepository){
            _parametersRepository = parametersRepository;
        }

        public List<Parameters> GetParametersFromDB(){
            return _parametersRepository.GetParametersFromDB().Result;
        } 
    }
}