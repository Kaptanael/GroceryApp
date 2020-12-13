using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryApp.ViewModels
{
    public class TransactionForSellViewModel
    {
        public int CustomerTransactionId { get; set; }

        [Display(Name = "Amount")]
        [Required]
        [Range(0.1, Double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public decimal? SoldAmount { get; set; }
        
        [DataType(DataType.Html)]
        [MaxLength(2048)]
        public string Description { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date)]        
        public DateTime TransactionDate => DateTime.Now;

        [Display(Name = "Customer")]
        [Required]
        public int CustomerId { get; set; }

        public SelectList CustomerSelectList { get; set; }
    }
}
