using System.Collections.Generic;
using System.Threading.Tasks;
using GuidantFinancial.Entities;

namespace GuidantFinancial.Services
{
    public interface IAccountRepository
    {
        Task AddCustomerAsync(Customer customer, string password);
        Task<Customer> GetCustomer(int id);
        Task<Customer> GetCustomerByEmailAsync(string email);
        Task<Customer> GetCustomerByName(string name);
        Task<ICollection<Customer>> GetAllCustomers();
    }
}
