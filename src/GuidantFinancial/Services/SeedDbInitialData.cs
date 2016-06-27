using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using GuidantFinancial.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Logging;

namespace GuidantFinancial.Services
{
    public class SeedDbInitialData
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AccountRepository> _logger;

        public SeedDbInitialData(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
             ILogger<AccountRepository> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task EnsureSeedData()
        {


            if (_context.Customers.Any()) return;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //Seed Database, add admin and test user by default
                    var securityTypes = new List<SecurityType>()
                    {
                        new SecurityType()
                        {
                            Type = SecurityTypes.Stocks,
                            Calculation = "{0} / {1}" //Price / total shares
                        },
                        new SecurityType()
                        {
                            Type = SecurityTypes.Bonds,
                            Calculation = "{0} * {1}" //Price * total shares
                        },
                        new SecurityType()
                        {
                            Type = SecurityTypes.Funds,
                            Calculation = "{0} * {1} / 2" //Price * total shares / 2
                        }
                    };

                    _context.SecurityTypes.AddRange(securityTypes);

                    var securities = new List<Security>()
                    {
                        new Security()
                        {
                            Symbol = "NDQ",
                            Price = 100.00m,
                            Type = securityTypes.FirstOrDefault(x => x.Type == SecurityTypes.Stocks)
                        },
                        new Security()
                        {
                            Symbol = "SHLL",
                            Price = 500.00m,
                            Type = securityTypes.FirstOrDefault(x => x.Type == SecurityTypes.Stocks)
                        },
                        new Security()
                        {
                            Symbol = "FBK",
                            Price = 250.00m,
                            Type = securityTypes.FirstOrDefault(x => x.Type == SecurityTypes.Stocks)
                        },
                        new Security()
                        {
                            Symbol = "GHB",
                            Price = 150.50m,
                            Type = securityTypes.FirstOrDefault(x => x.Type == SecurityTypes.Stocks)
                        }
                    };

                    _context.Securities.AddRange(securities);

                    var portfolio = new Portfolio()
                    {
                        Name = "test",
                        Securities = securities
                    };

                    _context.Portfolios.Add(portfolio);

                    var customers = new List<Customer>()
                    {
                        new Customer()
                        {
                            Name = "admin@domain.com",
                            Email = "admin@domain.com"
                        },
                        new Customer()
                        {
                            Name = "test@domain.com",
                            Email = "test@domain.com",
                            Portfolio = portfolio
                        }
                    };

                    _context.Customers.AddRange(customers);


                    await
                        _userManager.CreateAsync(
                            new ApplicationUser() {UserName = "admin@domain.com", Email = "admin@domain.com"},
                            "Password1");
                    await
                        _userManager.CreateAsync(
                            new ApplicationUser() {UserName = "test@domain.com", Email = "test@domain.com"}, "Password1");
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogError("Error while seeding the database", ex);
                }
                finally
                {
                    _context.Dispose();
                }
            }
            
        }



    }

}
