using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GuidantFinancial.Entities;

namespace GuidantFinancial.Entities
{
    public class Customer
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        public Portfolio Portfolio { get; set; }
    }
}