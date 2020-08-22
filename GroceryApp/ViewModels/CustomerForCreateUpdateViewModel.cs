using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryApp.ViewModels
{
    public class CustomerForCreateUpdateViewModel
    {
        public int? CustomerId { get; set; }

        [Required, MaxLength(32)]
        [Display(Name = "First Name")]        
        public string FirstName { get; set; }
        
        [Required, MaxLength(32)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        
        public string FullName { get; set; }

        [Required, MaxLength(128)]
        [EmailAddress]
        [Remote(action: "IsEmailInUse", controller: "Customers", AdditionalFields = "CustomerId")]
        public string Email { get; set; }

        [Required, MaxLength(11)]        
        [RegularExpression(@"^(\d{11})$", ErrorMessage = "Mobile number not valid.")]
        [Remote(action: "IsMobileInUse", controller: "Customers", AdditionalFields = "CustomerId")]
        public string Mobile { get; set; }
        
        [MaxLength(512)]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Status")]
        public bool IsActive { get; set; } = true;        
        public SelectList IsActiveSelectList { get; set; }
    }
}
