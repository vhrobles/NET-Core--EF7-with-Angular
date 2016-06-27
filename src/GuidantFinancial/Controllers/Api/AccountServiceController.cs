using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using GuidantFinancial.Entities;
using GuidantFinancial.Services;
using GuidantFinancial.ViewModels.Account;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;

namespace GuidantFinancial.Controllers.Api
{    
    [Authorize]
    [Route("api/[controller]")]
    public class AccountServiceController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountServiceController(
            IAccountRepository accountRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            _accountRepository = accountRepository;
            _userManager = userManager;
        }
        // GET: api/values
        [HttpGet]
        public JsonResult Get()
        {
            return Json(new { name = "Jason"});
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return id.ToString();
        }

        // POST api/values
        [HttpPost]
        public async Task<JsonResult> Post([FromBody]NewCustomer customer)
        {            
            var existingCustomer = await _accountRepository.GetCustomerByEmailAsync(customer.Email);            
            if (existingCustomer != null)
            {
                return Json(false);
            }
            var newCustomer = new Customer()
            {
                Name = customer.Email,
                Email = customer.Email                
            };
            var result = await _accountRepository.AddCustomerAsync(newCustomer, customer.Password);
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
