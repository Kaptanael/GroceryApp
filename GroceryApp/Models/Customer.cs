using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GroceryApp.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [Required, MaxLength(32)]
        public string FirstName { get; set; }

        [Required, MaxLength(32)]
        public string LastName { get; set; }
        
        public string FullName { get; set; }           

        [Required, MaxLength(32)]
        public string Mobile { get; set; }

        [MaxLength(128)]
        public string Email { get; set; }

        [MaxLength(512)]
        public string Address { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public ICollection<CustomerTransaction> CustomerTransactions { get; set; }

    }
}
