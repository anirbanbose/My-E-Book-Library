using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Data.EF.Entities
{
    [Table("BookAuthor", Schema = "EbookLibrary")]
    public class BookAuthor
    {
        public int BookId { get; set; }
        public int AuthorId { get; set; }
        public int AuthorTypeId { get; set; }

        public Book Book { get; set; }
        public Author Author { get; set; }
        public AuthorType AuthorType { get; set; }
    }
}
