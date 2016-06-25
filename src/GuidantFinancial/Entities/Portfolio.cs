using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GuidantFinancial.Entities
{
    public class Portfolio
    {        
        public int? Id { get; set; }  
        public string Name { get; set; }           
        public ICollection<Security> Securities { get; set; }
        
    }
}
