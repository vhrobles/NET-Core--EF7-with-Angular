using System.Collections.Generic;
using System.Threading.Tasks;
using GuidantFinancial.Entities;

namespace GuidantFinancial.Services
{
    public interface IPortfolioRepository
    {
        Task<CustomerPortfolio> GetCustomerPortfolioAsync(int customerId);
        Task<ICollection<CustomerPortfolio>> GetAllCustomerPortfoliosAsync();
        Task<bool> AddCustomerSecurityAsync(NewCustomerSecurity customerSecurity);
        Task<ICollection<SecurityType>> GetAllSecurityTypesAsync();
        Task<bool> UpdateSecurityType(int id, string calculation);
    }
}
