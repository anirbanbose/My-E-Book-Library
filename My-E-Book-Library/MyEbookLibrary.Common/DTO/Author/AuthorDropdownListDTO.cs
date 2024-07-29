using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Common.DTO.Author
{
    public class AuthorDropdownListDTO
    {
        public AuthorDropdownListDTO(int id, string firstName, string? middleName, string lastName)
        {
            Id = id;
            FullName = $"{firstName}{(!string.IsNullOrEmpty(middleName) ? " " + middleName : "")} {lastName}";
        }
        public int Id { get; }
        public string FullName { get; }
    }
}
