using System.Threading.Tasks;
using GuidantFinancial.Entities;
using GuidantFinancial.Services;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;


namespace GuidantFinancial.Controllers.Api
{
    [Authorize]
    [Route("api/[controller]")]    
    public class PortfolioServiceController : Controller
    {        
        private readonly IPortfolioRepository _portfolio;
        private readonly IAccountRepository _account;
        
        public PortfolioServiceController(            
            IPortfolioRepository portfolio,
            IAccountRepository account
            )
        {            
            _portfolio = portfolio;
            _account = account;
        }
                
        [HttpGet]        
        [Route("GetAll")]
        public async Task<JsonResult> GetAll()
        {
            var portfolios = await _portfolio.GetAllCustomerPortfoliosAsync();
            return Json(portfolios);
        }

        
        [HttpGet]
        [Route("GetAllSecurityTypes")]
        public async Task<JsonResult> GetAllSecurityTypes()
        {
            var securityTypes = await _portfolio.GetAllSecurityTypesAsync();
            return Json(securityTypes);
        }

        [HttpPut]
        [Route("UpdateSecurityType")]        
        public async Task<JsonResult> UpdateSecurityType(int id, string calculation)
        {
            var result = await _portfolio.UpdateSecurityType(id, calculation);
            return Json(result);
        }
        // GET: api/values
        [HttpGet]
        public async Task<JsonResult> Get()
        {
            var userName = User.Identity.Name;
            var customerData = await _account.GetCustomerByEmailAsync(userName);
            var customerPortfolio = await _portfolio.GetCustomerPortfolioAsync(customerData.Id);
            return Json(customerPortfolio);
        }                

        // GET api/values/5
        [HttpGet("{id}")]        
        public async Task<JsonResult> Get(int id)
        {

            var customerPortfolio = await _portfolio.GetCustomerPortfolioAsync(id);
            return Json(customerPortfolio);
        }

        // POST api/values
        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> Post([FromBody]NewCustomerSecurity customerSecurity)
        {                 
            var result = await _portfolio.AddCustomerSecurityAsync(customerSecurity);
            return Json(result);
        }
                

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
