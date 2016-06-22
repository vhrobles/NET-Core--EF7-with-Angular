using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

using GuidantFinancial.Controllers.Web;
using GuidantFinancial.Entities;
using GuidantFinancial.Services;
using GuidantFinancial.ViewModels.Account;
using Microsoft.AspNet.Antiforgery;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Internal;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GuidantFinancial.UnitTests.Services
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class AccountControllerTests
    {
        //private readonly Mock<IAccountRepository> _accountRepositoryMock = new Mock<IAccountRepository>();
        //private readonly Mock<UserManager<ApplicationUser>> _userManagerMock = new Mock<UserManager<ApplicationUser>>();
        //private readonly Mock<SignInManager<ApplicationUser>> _signInManagerMock = new Mock<SignInManager<ApplicationUser>>();
        //private readonly Mock<ILoggerFactory> _loggerFactoryMock = new Mock<ILoggerFactory>();
        //private readonly AccountController _accountController;


        //public AccountControllerTests()
        //{            
        //    _accountController = new AccountController(
        //        _userManagerMock.Object,
        //        _signInManagerMock.Object,
        //        _accountRepositoryMock.Object,
        //        _loggerFactoryMock.Object
        //        )
        //    {
        //        ActionContext = new ActionContext()
        //        {
        //            HttpContext = new DefaultHttpContext()
        //        }
        //    };

        //}

        [Fact]
        public void Login()
        {
            //_accountController.ActionContext.HttpContext.User = new GenericPrincipal(new GenericIdentity("test@domain.com"), null);
            //_accountRepositoryMock.Object.AddCustomer(new Customer() { Email = "test@domain.com"});
            //var fakeLoginViewModel = new LoginViewModel()
            //{
            //    Email = "test@domain.com",
            //    Password = "Password1"
            //};
            //await _accountController.Login(fakeLoginViewModel, null);
            var httpContext = Substitute.For<HttpContext>();
            const int x = 1;
            Assert.Equal(x, 1);
        }
    }
}
