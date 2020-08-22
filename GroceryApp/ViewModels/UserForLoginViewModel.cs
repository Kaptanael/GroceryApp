using System.ComponentModel.DataAnnotations;

namespace GroceryApp.ViewModels
{
    public class UserForLoginViewModel
    {
        [Required, StringLength(64, MinimumLength = 3)]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required, StringLength(10, MinimumLength = 6)]
        [DataType(DataType.Password)]        
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}
