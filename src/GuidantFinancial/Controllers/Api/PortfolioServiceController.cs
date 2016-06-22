using System.Collections.Generic;
using System.Threading.Tasks;
using GuidantFinancial.Entities;
using GuidantFinancial.Services;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;


namespace GuidantFinancial.Controllers.Api
{
    [Authorize]
    [Route("api/[controller]")]    
    public class PortfolioServiceController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly IPortfolioRepository _portfolio;
        private readonly IAccountRepository _account;
        
        public PortfolioServiceController(
            UserManager<ApplicationUser> userManager,
            IPortfolioRepository portfolio,
            IAccountRepository account
            )
        {
            _userManager = userManager;
            _portfolio = portfolio;
            _account = account;
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
        public JsonResult Get(int id)
        {
            ;
            return Json(new JsonResult(new {}));
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
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
