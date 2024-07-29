using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Common.DTO.Author
{
    public class AuthorDTO : BaseDTO
    {
        public AuthorDTO(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
        [MaxLength(100, ErrorMessage = "First Name mustn't be more than 100 characters long.")]
        [Required(ErrorMessage = "First Name is required")]
        [NotNull()]
        public string FirstName { get; set; }
        [MaxLength(100, ErrorMessage = "Middle Name mustn't be more than 100 characters long.")]
        public string? MiddleName { get; set; } = null;
        [Required(ErrorMessage = "Last Name is required")]
        [MaxLength(100, ErrorMessage = "Last Name mustn't be more than 100 characters long.")]
        [NotNull()]
        public string LastName { get; set; }
    }
}
