using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyApp1.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "username and email is required")]
        [MaxLength(20, ErrorMessage = "Max 20 Characters allowed")]
        [DisplayName("Username or email")]
        public string UserNameOrEmail { get; set; }

        [Required(ErrorMessage = "PassWord is required")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Max 20 or minimum 5 Characters allowed")]
        [DataType(DataType.Password)]
        public string Password { get; set; } 
    }
}
