using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GuidantFinancial.Entities
{
    public class CustomerSecurities
    {        
        public string Symbol { get; set; }
        public decimal Price { get; set; }
        public decimal MarketValue { get; set; }
        public SecurityTypes Type { get; set; }
    }
}
