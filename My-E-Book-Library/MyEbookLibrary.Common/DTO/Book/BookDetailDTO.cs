using MyEbookLibrary.Common.DTO.BookAuthor;
using MyEbookLibrary.Common.DTO.Genre;
using MyEbookLibrary.Common.DTO.Language;
using MyEbookLibrary.Common.DTO.Publisher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Common.DTO.Book
{
    public class BookDetailDTO : BaseDTO
    {
        public string Title { get; set; }
        public string? BookImage { get; set; } = null;
        public string? Subject { get; set; } = null;
        public string? ISBN10 { get; set; } = null;
        public string? ISBN13 { get; set; } = null;
        public string? Description { get; set; } = null;
        public string? EditionName { get; set; } = null;
        public string? PublishedDate { get; set; } = null;
        public int? NoOfPages { get; set; } = null;
        public string? Genres { get; set; } = null;
        public string? Publisher { get; set; } = null;
        public string? Authors { get; set; } = null;
        public string? Languages { get; set; } = null;
        public List<FileDetailDTO> Files { get; set; } = new List<FileDetailDTO>();
    }
}
