using System.Collections.Generic;
using GuidantFinancial.Controllers.Api;
using GuidantFinancial.Entities;
using GuidantFinancial.Services;
using Moq;
using NUnit.Framework;
using GuidantFinancial.Tests.Extensions;

namespace GuidantFinancial.Tests
{
    [TestFixture]
    class PortfolioServiceControllerTests
    {
        private readonly Mock<IPortfolioRepository> _portfolioRepositoryMock = new Mock<IPortfolioRepository>();
        private readonly Mock<IAccountRepository> _accountRepositoryMock = new Mock<IAccountRepository>();
        private readonly PortfolioServiceController _controller;
        public PortfolioServiceControllerTests()
        {
            _controller = new PortfolioServiceController(
                _portfolioRepositoryMock.Object,
                _accountRepositoryMock.Object);
            _controller.MockCurrentUser("admin@domain.com", "Password1");
        }

        [Test]
        public void Get_Customer_Portfolio()
        {
                                 
            _portfolioRepositoryMock.Setup(x => x.GetCustomerPortfolioAsync(1)).ReturnsAsync(new CustomerPortfolio()
            {
                CustomerName = "test@domain.com"
            });

            var result = _controller.Get(1).Result.Value;
            Assert.IsNotNull(result);
        }

        [Test]
        public void Get_All_Customer_Portfolios()
        {
            _portfolioRepositoryMock.Setup(x => x.GetAllCustomerPortfoliosAsync())
                .ReturnsAsync(new List<CustomerPortfolio>());
            var result = _controller.GetAll().Result.Value;
            Assert.IsNotNull(result);
        }
    }
}
