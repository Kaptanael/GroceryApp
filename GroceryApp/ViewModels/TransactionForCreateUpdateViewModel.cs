using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroceryApp.ViewModels
{
    public class TransactionForCreateUpdateViewModel
    {
        public int CustomerTransactionId { get; set; }

        [Display(Name = "Amount")]
        [Required]
        public decimal Amount { get; set; }

        [DataType(DataType.Html)]
        [MaxLength(2048)]
        public string Description { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime TransactionDate => DateTime.Now;

        [Display(Name = "Customer")]
        [Required]
        public int CustomerId { get; set; }

        public byte TransactionType { get; set; }

        public SelectList CustomerSelectList { get; set; }
    }
}
