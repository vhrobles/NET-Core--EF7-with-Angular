using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpressionEvaluator;
using GuidantFinancial.Entities;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Logging;

namespace GuidantFinancial.Services
{
    //TODO: Implement Generic Repo, Unit Test
    /// <summary>
    /// Portfolio Repo for DB operations
    /// </summary>
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AccountRepository> _logger;

        public PortfolioRepository(
            ApplicationDbContext context,
            ILogger<AccountRepository> logger
            )
        {
            _context = context;
            _logger = logger;
        }
        public async Task<CustomerPortfolio> GetCustomerPortfolioAsync(int customerId)
        {
            
                try
                {
                    IList<CustomerSecurities> securities = await (from c in _context.Customers
                        join p in _context.Portfolios on c.Portfolio.Id equals p.Id
                        join s in _context.Securities on p.Id equals s.Portfolio.Id
                        join st in _context.SecurityTypes on s.Type.Id equals st.Id
                        where c.Id == customerId
                        select new CustomerSecurities()
                        {
                            Symbol = s.Symbol,
                            Price = s.Price,
                            Type = st.Type
                        }).ToListAsync();
                    securities = await CalculateMarketValueAsync(securities);
                    var portfolio = new CustomerPortfolio()
                    {
                        CustomerId = customerId,
                        PortfolioId =
                            _context.Customers.Where(x => x.Id == customerId)
                                .Select(x => x.Portfolio.Id)
                                .FirstOrDefault(),
                        CustomerName = _context.Customers.FirstOrDefault(c => c.Id == customerId)?.Name,
                        CustomerSecurities = securities,
                        PortfolioValue = securities.Select(x => x.MarketValue).Sum(),
                        TotalPaidPrice = securities.Select(x => x.Price).Sum()
                    };
                    
                    return portfolio;
                }                
                catch (NotSupportedException ex)
                {
                    _logger.LogError("Validation error occurred", ex);
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogError("Error while processing entities in database", ex);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Generic exception", ex);
                }
                finally
                {

                    _context.Dispose();
                }
            return null;
        }

        public async Task<bool> AddCustomerSecurityAsync(NewCustomerSecurity customerSecurity)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var portfolio = await
                        _context.Portfolios.Where(x => x.Id == customerSecurity.PortfolioId).FirstOrDefaultAsync();

                    var security = new Security()
                    {
                        Type = _context.SecurityTypes.SingleOrDefault(x => x.Type == customerSecurity.Type),
                        Price = customerSecurity.Price,
                        Symbol = customerSecurity.Symbol
                    };

                    var customer =
                        await _context.Customers.Where(x => x.Id == customerSecurity.CustomerId).SingleOrDefaultAsync();

                    ICollection<Security> securities =
                        await
                            _context.Securities.Where(x => x.Portfolio.Id == customerSecurity.PortfolioId).ToListAsync();

                    if (!securities.Any())
                    {
                        portfolio = new Portfolio()
                        {
                            Securities = new List<Security>()
                            {
                                security
                            }
                        };
                    }
                    else
                    {
                        portfolio.Securities.Add(security);
                    }

                    //If portfolio exists just update it, if not add a new one to customer with securities in it.
                    if (portfolio.Id <= 0)
                    {
                        _context.Entry(portfolio).State = EntityState.Modified;
                    }
                    else
                    {
                        customer.Portfolio = portfolio;
                        _context.Entry(customer).State = EntityState.Modified;
                    }

                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return true;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    transaction.Rollback();
                    _logger.LogError("Couldn't update record, concurrency violation ocurred", ex);
                }
                catch (DbUpdateException ex)
                {
                    transaction.Rollback();
                    _logger.LogError("Couldn't update record", ex);
                }
                catch (NotSupportedException ex)
                {
                    transaction.Rollback();
                    _logger.LogError("Validation error occurred", ex);
                }
                catch (InvalidOperationException ex)
                {
                    transaction.Rollback();
                    _logger.LogError("Error while processing entities in database", ex);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogError("Generic exception", ex);
                }
                
            }
            return false;
        }

        public async Task<ICollection<SecurityType>> GetAllSecurityTypesAsync()
        {
            try
            {
                var securityTypes = await _context.SecurityTypes.ToListAsync();
                return securityTypes;
            }            
            catch (Exception ex)
            {
                _logger.LogError("Generic exception", ex);
            }
            finally
            {
                _context.Dispose();
            }
            return null;
        }

        public async Task<bool> UpdateSecurityType(int id, string calculation)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var securityType =
                        await _context.SecurityTypes.Where(x => (int) x.Type == id).SingleOrDefaultAsync();
                    securityType.Calculation = calculation;
                    _context.Entry(securityType).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return true;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError("Couldn't update record, concurrency violation ocurred", ex);
                    transaction.Rollback();
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError("Couldn't update record", ex);
                    transaction.Rollback();
                }
                catch (NotSupportedException ex)
                {
                    _logger.LogError("Validation error occurred", ex);
                    transaction.Rollback();
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogError("Error while processing entities in database", ex);
                    transaction.Rollback();
                }
                catch (Exception ex)
                {
                    _logger.LogError("Generic exception", ex);
                    transaction.Rollback();
                }
                
            }
            return false;
        }

        public async Task<ICollection<CustomerPortfolio>> GetAllCustomerPortfoliosAsync()
        {
            try
            {
                //Outer left join all the customers regardless if they have a portfolio or not.
                var emptyPortfolios = await (from c in _context.Customers
                                             where !c.Name.Contains("admin") && c.Portfolio.Id == null
                                             select new CustomerPortfolio()
                                             {
                                                 CustomerId = c.Id,
                                                 CustomerName = c.Name,
                                                 PortfolioId = c.Portfolio.Id
                                             }).ToListAsync();

                var portfolios = await (from cx in _context.Customers
                                        where cx.Portfolio.Id != null
                                        select new CustomerPortfolio()
                                        {
                                            CustomerId = cx.Id,
                                            CustomerName = cx.Name,
                                            PortfolioId = cx.Portfolio.Id,
                                            CustomerSecurities = (from c in _context.Customers
                                                                  join p in _context.Portfolios on c.Portfolio.Id equals p.Id
                                                                  join s in _context.Securities on p.Id equals s.Portfolio.Id
                                                                  join st in _context.SecurityTypes on s.Type.Id equals st.Id
                                                                  where c.Id == cx.Id
                                                                  select new CustomerSecurities()
                                                                  {
                                                                      Symbol = s.Symbol,
                                                                      Price = s.Price,
                                                                      Type = st.Type
                                                                  }
                                                                  ).ToList()                                            
                                        }).ToListAsync();

                foreach (var portfolio in portfolios)
                {
                    portfolio.CustomerSecurities =
                        await CalculateMarketValueAsync(portfolio.CustomerSecurities).ConfigureAwait(false);
                    portfolio.PortfolioValue = portfolio.CustomerSecurities.Select(s => s.MarketValue).Sum();
                    portfolio.TotalPaidPrice = portfolio.CustomerSecurities.Select(s => s.Price).Sum();
                }

                portfolios.AddRange(emptyPortfolios);

                return portfolios;
            }            
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Error while processing entities in database", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError("Generic exception", ex);
            }
            
            return null;
        }

        private async Task<IList<CustomerSecurities>> CalculateMarketValueAsync(IList<CustomerSecurities> securities)
        {
            var nStocks = securities.Count(t => t.Type == SecurityTypes.Stocks);
            var stocksCalc =
                   await _context.SecurityTypes.Where(t => t.Type == SecurityTypes.Stocks)
                        .Select(t => t.Calculation)
                        .FirstOrDefaultAsync();
            var nBonds = securities.Count(t => t.Type == SecurityTypes.Bonds);
            var bondsCalc =
                    await _context.SecurityTypes.Where(t => t.Type == SecurityTypes.Bonds)
                        .Select(t => t.Calculation)
                        .FirstOrDefaultAsync();
            var nFunds = securities.Count(t => t.Type == SecurityTypes.Funds);
            var fundsCalc =
                    await _context.SecurityTypes.Where(t => t.Type == SecurityTypes.Funds)
                        .Select(t => t.Calculation)
                        .FirstOrDefaultAsync();

            foreach (var t in securities)
            {
                decimal marketValue;
                switch (t.Type)
                {
                    case SecurityTypes.Stocks:
                        marketValue = Eval(string.Format(stocksCalc, t.Price, nStocks));
                        t.MarketValue = marketValue;
                        break;
                    case SecurityTypes.Bonds:
                        marketValue = Eval(string.Format(bondsCalc, t.Price, nBonds));
                        t.MarketValue = marketValue;
                        break;
                    case SecurityTypes.Funds:
                        marketValue = Eval(string.Format(fundsCalc, t.Price, nFunds));
                        t.MarketValue = marketValue;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return securities;
        }

        private static decimal Eval(string expression)
        {
            var exp = new CompiledExpression(expression);
            decimal result;
            decimal.TryParse(exp.Eval().ToString(), out result);
            return result;
        }        

    }
}

