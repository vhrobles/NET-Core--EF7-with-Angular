using System;
using System.Linq.Expressions;
using Castle.Core.Logging;
using Castle.Core.Smtp;
using GuidantFinancial.Controllers.Api;
using GuidantFinancial.Entities;
using GuidantFinancial.Services;
using GuidantFinancial.Tests.Extensions;
using Microsoft.AspNet.Mvc;
using Moq;
using NUnit.Framework;

namespace GuidantFinancial.Tests
{
    [TestFixture]
    public class AccountServiceControllerTests
    {
        private readonly Mock<IAccountRepository> _accountRepositoryMock = new Mock<IAccountRepository>();        
        private readonly Mock<ILoggerFactory> _loggerFactoryMock = new Mock<ILoggerFactory>();
        private readonly AccountServiceController _controller;

        public AccountServiceControllerTests()
        {                                
            _controller = new AccountServiceController(_accountRepositoryMock.Object);
            _controller.MockCurrentUser("admin@domain.com", "Password1");
        }

        [Test]
        public void Add_Existing_Customer()
        {
            var existingCustomer = new NewCustomer()
            {
                Email = "test@domain.com",
                Password = "Password1"
            };
            _accountRepositoryMock.Setup(x => x.GetCustomerByEmailAsync(existingCustomer.Email)).ReturnsAsync(new Customer());
            var result = _controller.Post(existingCustomer).Result.Value;
            const bool expected = false;
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Add_New_Customer()
        {
            var existingCustomer = new NewCustomer()
            {
                Email = "newcust@domain.com",
                Password = "Password1"
            };
            _accountRepositoryMock.Setup(x => x.AddCustomerAsync(It.IsAny<Customer>(), It.IsAny<string>())).ReturnsAsync(true);
            var result = _controller.Post(existingCustomer).Result.Value;
            const bool expected = true;
            Assert.AreEqual(expected, result);
        }
    }
}
