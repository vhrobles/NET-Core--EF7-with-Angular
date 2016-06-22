using System.ComponentModel.DataAnnotations;

namespace GuidantFinancial.Entities
{    
    public class Security
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Symbol { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public SecurityType Type { get; set; }

        public virtual Portfolio Portfolio { get; set; }
    }
}
