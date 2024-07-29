using System.ComponentModel.DataAnnotations;

namespace MyEbookLibrary.Common.DTO.Account
{
    public class RegistrationDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Not a valid Email address.")]
        [MaxLength(250, ErrorMessage = "Email can't be more than 250 characters.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [MaxLength(20, ErrorMessage = "Password can't be more than 20 characters.")]
        [MinLength(8, ErrorMessage = "Password can't be less than 8 characters.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        [MaxLength(100, ErrorMessage = "First Name can't be more than 100 characters.")]
        public string FirstName { get; set; }
        [MaxLength(100, ErrorMessage = "Middle Name can't be more than 100 characters.")]
        public string? MiddleName { get; set; } = null;
        [Required(ErrorMessage = "Last Name is required")]
        [MaxLength(100, ErrorMessage = "Last Name can't be more than 100 characters.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]
        public string ConfirmPassword { get; set; }
    }
}
