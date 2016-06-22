using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuidantFinancial.Entities;

namespace GuidantFinancial.Services
{
    public interface IPortfolioRepository
    {
        Task<CustomerPortfolio> GetCustomerPortfolioAsync(int customerId);
    }
}
