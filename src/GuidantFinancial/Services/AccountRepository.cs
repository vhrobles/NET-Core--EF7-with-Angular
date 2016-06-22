using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuidantFinancial.Entities;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Logging;

namespace GuidantFinancial.Services
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AccountRepository> _logger;

        public AccountRepository(
            ApplicationDbContext context, 
            ILogger<AccountRepository> logger)
        {
            _context = context;
            _logger = logger;
        }        

        public async Task AddCustomerAsync(Customer customer)
        {
            try
            {
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Database operation error", ex);
            }
        }

        public async Task<Customer> GetCustomerByName(string name)
        {
            try
            {
                var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Name == name);
                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError("Database operation error", ex);
                return null;
            }
        }

        public async Task<ICollection<Customer>> GetAllCustomers()
        {
            try
            {
                var customers = await _context.Customers.Where(x => x.Name != "admin").OrderBy(x => x.Name).ToListAsync();
                return customers;
            }
            catch (Exception ex)
            {
                _logger.LogError("Database operation error", ex);
                return null;
            }
        }
        public async Task<Customer> GetCustomer(int id)
        {
            try
            {
                var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);
                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError("Database operation error ", ex);
                return null;
            }
        }

        public async Task<Customer> GetCustomerByEmailAsync(string email)
        {
            try
            {
                var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Email == email);
                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError("Database operation error ", ex);
                return null;
            }
        }
    }
}
