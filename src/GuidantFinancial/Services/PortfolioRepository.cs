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
    public class PortfolioRepository : IPortfolioRepository, IDisposable
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
                    CustomerName = _context.Customers.FirstOrDefault(c => c.Id == customerId)?.Name,
                    CustomerSecurities = securities,
                    PortfolioValue = securities.Select(x => x.MarketValue).Sum(),
                    TotalPaidPrice = securities.Select(x => x.Price).Sum()
                };                
                return portfolio;
            }
            catch (Exception ex)
            {
                _logger.LogError("Database operation error", ex);
                return null;
            }
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

        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
