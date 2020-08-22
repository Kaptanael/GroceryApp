using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroceryApp.Models
{
    public class CustomerTransaction
    {
        public int CustomerTransactionId { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal SoldAmount { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal ReceivedAmount { get; set; }

        [Column(TypeName = "nvarchar(2048)")]
        public string Description { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
