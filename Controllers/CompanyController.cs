using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi_bilheteria_c.Domain.Interface;
using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class CompanyController : MainController
    {
        private readonly ICompanyService _companyService;
        public CompanyController(ICompanyService companyService, ITokenService tokenService): base(tokenService)
        {
            _companyService = companyService;
        }

        [HttpGet("get-company-by-owner")]
        [ProducesResponseType(typeof(Company), StatusCodes.Status200OK)]
        public ActionResult GetCompanyByOwner(){
            try
            {
                return Ok(_companyService.GetCompanyByOwner(TokenId));               
            }
            catch (Exception ex)
            {
                return BadRequest(new {error = ex.Message});
            }
        }

        [HttpPost("create-company")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult CreateCompany([FromBody] Company company){
            try
            {
                company.OwnerUid = TokenId;
                _companyService.CreateCompany(company);
                return Created("", new {Created = true});               
            }
            catch (Exception ex)
            {
                return BadRequest(new {error = ex.Message});
            }
        }

    }
}