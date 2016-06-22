using GuidantFinancial.Services;
using Microsoft.AspNet.Mvc;

namespace GuidantFinancial.Controllers.Web
{
    public class AppController : Controller
    {
        

        public AppController()
        {
            
        }

        public IActionResult Index()
        {            
            return View();
        }
    }
}
