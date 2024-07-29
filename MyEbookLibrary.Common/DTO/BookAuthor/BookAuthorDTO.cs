using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Common.DTO.BookAuthor
{
    public class BookAuthorDTO
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public int AuthorTypeId { get; set; }
    }
}
