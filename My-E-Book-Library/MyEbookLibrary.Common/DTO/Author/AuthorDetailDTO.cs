using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Common.DTO.Author
{
    public class AuthorDetailDTO : BaseDTO
    {
        public AuthorDetailDTO(string firstName, string lastName, string? middleName)
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
        }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; } = null;
        public string LastName { get; set; }
        public string FullName 
        {
            get
            {
                return $"{FirstName}{(!string.IsNullOrEmpty(MiddleName) ? " " + MiddleName : "")} {LastName}";
            }
        }
    }
}
