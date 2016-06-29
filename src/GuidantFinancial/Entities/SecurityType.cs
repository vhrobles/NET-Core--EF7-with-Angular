using System.ComponentModel.DataAnnotations;

namespace GuidantFinancial.Entities
{    
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
