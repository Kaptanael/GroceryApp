using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryApp.ViewModels
{
    public class CustomerTransactionForListViewModel
    {
        public int TransactionId { get; set; }

        [Display(Name = "Sell Amount")]
        public decimal SoldAmount { get; set; }

        [Display(Name = "Receive Amount")]
        public decimal ReceivedAmount { get; set; }

        [Display(Name = "Transaction Date")]
        public DateTime TransactionDate { get; set; }
        public int CustomerId { get; set; }        
        
        [Display(Name = "First Name")]
        public string FirstName { get; set; }      
        
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }        

        public string Mobile { get; set; }

        public bool Status { get; set; }
    }
}
