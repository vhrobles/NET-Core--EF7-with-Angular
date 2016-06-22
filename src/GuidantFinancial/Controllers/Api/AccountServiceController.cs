using System.Collections.Generic;
using GuidantFinancial.Entities;
using GuidantFinancial.Services;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;

namespace GuidantFinancial.Controllers.Api
{    
    [Route("api/[controller]")]
    public class AccountServiceController : Controller
    {
        private readonly IAccountRepository _accountRepository;

        public AccountServiceController(
            IAccountRepository accountRepository
            )
        {
            _accountRepository = accountRepository;
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
        public void Post([FromBody]Customer customer)
        {
            //_accountRepository.AddCustomer(customer);
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
