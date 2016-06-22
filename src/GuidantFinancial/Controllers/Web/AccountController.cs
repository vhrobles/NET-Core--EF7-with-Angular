using System.Security.Claims;
using System.Threading.Tasks;
using GuidantFinancial.Entities;
using GuidantFinancial.Services;
using GuidantFinancial.ViewModels.Account;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;

namespace GuidantFinancial.Controllers.Web
{
    public class AccountController : Controller
    {
        
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAccountRepository _account;
        private readonly ILogger _logger;
        
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IAccountRepository account,
            ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _account = account;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }        

        public IActionResult Index()
        {
            return View();
        }    
        [Authorize]            
        public async Task<IActionResult> Register()
        {
            var user = await GetCurrentUserAsync();
            if (user == null || !user.Email.Contains("admin"))
            {
                return RedirectToAction(nameof(Index), "App");
            }
            return View();
        }        
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            LoginViewModel model,
            string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = await _account.GetCustomerByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "User doesn't exist.");
                    return View(model);
                }
                var result = await _signInManager.PasswordSignInAsync(
                        model.Email,
                        model.Password,
                        false,
                        lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation(1, "User logged in.");
                    return RedirectToLocal(returnUrl);
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning(2, "User account locked out.");
                    //return View("Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }

            }
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return RedirectToAction(nameof(AppController.Index), "App");
        }

        #region Private methods
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(AppController.Index), "App");
            }
        }

        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            return await _userManager.FindByIdAsync(HttpContext.User.GetUserId());
        }
        #endregion
    }
}
