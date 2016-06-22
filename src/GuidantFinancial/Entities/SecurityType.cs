using System.ComponentModel.DataAnnotations;

namespace GuidantFinancial.Entities
{
    public enum SecurityTypes
    {
        Funds = 1,
        Stocks = 2,
        Bonds = 3
    }
    public class SecurityType
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public SecurityTypes Type { get; set; }   
        [Required]
        public string Calculation { get; set; }                     

    }    
}
