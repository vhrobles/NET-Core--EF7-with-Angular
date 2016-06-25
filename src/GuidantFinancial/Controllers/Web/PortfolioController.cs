using System.Threading.Tasks;
using GuidantFinancial.Services;
using GuidantFinancial.ViewModels.Account;
using GuidantFinancial.ViewModels.Portfolio;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GuidantFinancial.Controllers.Web
{
    [Authorize]
    public class PortfolioController : Controller
    {        
        public IActionResult Index()
        {            
            return View();
        }

        public IActionResult Manage()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Details()
        {
            return View();
        }

        public IActionResult Securities()
        {
            return View();
        }
    }
}
