using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Work.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Login")]
        [StringLength(100, ErrorMessage = "The {0} must be at more {1} characters long.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and not at more {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
    public class RegisterModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
