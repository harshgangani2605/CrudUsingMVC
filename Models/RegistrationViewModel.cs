using System.ComponentModel.DataAnnotations;

namespace MyApp1.Models
{
    public class RegistrationViewModel
    {
        [Required(ErrorMessage = "First name is required")]
        [MaxLength(10, ErrorMessage = "Max 10 Characters allowed")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(10, ErrorMessage = "Max 10 Characters allowed")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [MaxLength(30, ErrorMessage = "Max 30 Characters allowed")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please Enter Valid Email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "username is required")]
        [MaxLength(20, ErrorMessage = "Max 20 Characters allowed")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "PassWord is required")]
        [StringLength(20,MinimumLength =5, ErrorMessage = "Max 20 or minimum 5 Characters allowed")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm Your Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get;set; }

    }
}
