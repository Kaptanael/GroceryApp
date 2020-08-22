using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryApp.ViewModels
{
    public class CustomerForListViewModel
    {
        public int CustomerId { get; set; }
        
        [Display(Name = "First Name")]        
        public string FirstName { get; set; }        
        
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }        

        public string Mobile { get; set; }        
        
        public string Address { get; set; }

        [Display(Name = "Status")]
        public bool IsActive { get; set; }                
    }
}
