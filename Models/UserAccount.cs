using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MyApp1.Models
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(UserName), IsUnique = true)]
    public class UserAccount
    {
        [Key]
        public int Id {get; set; }

        [Required(ErrorMessage = "First name is required")]
        [MaxLength(10,ErrorMessage ="Max 10 Characters allowed")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(10, ErrorMessage = "Max 10 Characters allowed")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [MaxLength(30, ErrorMessage = "Max 30 Characters allowed")]
        public string Email { get; set; }

        public string UserName { get; set; }

        [Required(ErrorMessage = "PassWord is required")]
        [MaxLength(20, ErrorMessage = "Max 20 Characters allowed")]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
