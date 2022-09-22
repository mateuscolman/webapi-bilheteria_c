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
        public CompanyController(ICompanyService companyService, ITokenService tokenService) : base(tokenService)
        {
            _companyService = companyService;
        }

        [HttpGet("get-company-by-owner")]
        [ProducesResponseType(typeof(List<Company>), StatusCodes.Status200OK)]
        public ActionResult GetCompanyByOwner()
        {
            try
            {
                return Ok(_companyService.GetCompanyByOwner(TokenId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("get-company-by-uid")]
        [ProducesResponseType(typeof(Company), StatusCodes.Status200OK)]
        public ActionResult GetCompanyByUid(string? uid)
        {
            try
            {
                return Ok(_companyService.GetCompanyByUid(uid));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("create-company")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult CreateCompany(string name, string description)
        {
            try
            {
                _companyService.CreateCompany(new Company
                {
                    OwnerUid = TokenId,
                    Name = name,
                    Description = description
                });
                return Created("", new { Created = true });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("edit-company/{uid}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public ActionResult EditCompany(string? description, string? name, int active,
            int exclusionLogic, [FromRoute] string? uid)
        {
            try
            {
                _companyService.EditCompany(new Company
                {
                    Description = description,
                    Name = name,
                    Active = active,
                    Uid = uid,
                    ExclusionLogic = exclusionLogic
                });
                return Accepted("", new { Created = true });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("insert-payment-method")]
        public ActionResult InsertPaymentMethod(string? companyUid, string? name, string? paymentKey)
        {
            try
            {
                return Created("",
                _companyService.InsertPaymentMethod(new CompanyPaymentMethod
                {
                    CompanyUid = companyUid,
                    Name = name,
                    PaymentKey = paymentKey
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("get-payment-method")]
        public ActionResult GetPaymentMethod(string companyUid)
        {
            try
            {
                return Ok(_companyService.GetPaymentMethod(companyUid));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}