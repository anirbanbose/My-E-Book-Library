using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Common.DTO.Author
{
    public class AuthorListDTO
    {
        public AuthorListDTO(int id, string firstName, string? middleName, string lastName, int bookCount, int addedby)
        {
            Id = id;
            AuthorName = $"{firstName}{(!string.IsNullOrEmpty(middleName) ? " " + middleName : "")} {lastName}";
            BookCount = bookCount;
            AddedBy = addedby;
        }
        public int Id { get; }
        public string AuthorName { get; }
        public int BookCount { get; }
        public int AddedBy { get; }
    }
}
