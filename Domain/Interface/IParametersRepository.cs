using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Domain.Interface
{
    public interface IParametersRepository
    {
        Task<List<Parameters>> GetParametersFromDB();
    }
}