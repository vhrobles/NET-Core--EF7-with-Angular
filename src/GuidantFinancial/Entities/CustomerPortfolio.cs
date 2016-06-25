using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GuidantFinancial.Entities
{
    public class CustomerPortfolio
    {
        public int CustomerId { get; set; }
        public int? PortfolioId { get; set; }
        public string CustomerName { get; set; }
        public IList<CustomerSecurities> CustomerSecurities { get; set; }
        public decimal PortfolioValue { get; set; }
        public decimal TotalPaidPrice { get; set; }
    }
}
