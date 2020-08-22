using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GroceryApp.ViewModels
{
    public class UserForRegisterViewModel
    {
        [Required, StringLength(64, MinimumLength = 3)]
        [Display(Name = "Username")]
        [Remote(action: "IsUsernameInUse", controller: "Account")]
        public string UserName { get; set; }

        [Required, MaxLength(128)]
        [EmailAddress]
        [Remote(action: "IsEmailInUse", controller: "Account")]
        public string Email { get; set; }        
        
        [Required, StringLength(10, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]        
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
