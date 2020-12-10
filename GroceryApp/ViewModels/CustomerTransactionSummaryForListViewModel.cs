using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryApp.ViewModels
{
    public class CustomerTransactionSummaryForListViewModel
    {             
        public int CustomerId { get; set; }                
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }        
        public decimal TotalSellAmount { get; set; }
        public decimal TotalReceiveAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalDueAmount { get; set; }
        public string Date { get; set; }
    }
}
