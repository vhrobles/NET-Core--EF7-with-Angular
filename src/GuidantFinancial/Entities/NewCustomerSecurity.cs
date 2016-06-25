using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GuidantFinancial.Entities
{
    public class NewCustomerSecurity
    {
        public int CustomerId { get; set; }
        public int? PortfolioId { get; set; }
        public decimal Price { get; set; }
        public string Symbol { get; set; }
        public SecurityTypes Type { get; set; }
    }
}
