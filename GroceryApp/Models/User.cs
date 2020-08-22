using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroceryApp.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required, MaxLength(64)]
        public string UserName { get; set; }

        [Required, MaxLength(128)]
        public string Email { get; set; }        

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

    }
}
