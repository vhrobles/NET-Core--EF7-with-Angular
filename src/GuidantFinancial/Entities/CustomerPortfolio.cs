using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GuidantFinancial.Entities
{
    public class CustomerPortfolio
    {
        public string CustomerName { get; set; }
        public IList<CustomerSecurities> CustomerSecurities { get; set; }
        public decimal PortfolioValue { get; set; }
        public decimal TotalPaidPrice { get; set; }
    }
}
