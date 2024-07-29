using System.ComponentModel.DataAnnotations;

namespace MyEbookLibrary.Common.DTO.Account
{
    public class SaveProfileDTO : BaseDTO
    {
        [Required(ErrorMessage = "First Name is required")]
        [MaxLength(100, ErrorMessage = "First Name can't be more than 100 characters.")]
        public string FirstName { get; set; }
        [MaxLength(100, ErrorMessage = "Middle Name can't be more than 100 characters.")]
        public string? MiddleName { get; set; } = null;
        [Required(ErrorMessage = "Last Name is required")]
        [MaxLength(100, ErrorMessage = "Last Name can't be more than 100 characters.")]
        public string LastName { get; set; }
        public string? BirthDate { get; set; } = null;
    }
}
