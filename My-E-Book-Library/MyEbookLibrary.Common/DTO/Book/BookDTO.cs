using MyEbookLibrary.Common.DTO.BookAuthor;
using MyEbookLibrary.Common.DTO.Genre;
using MyEbookLibrary.Common.DTO.Language;
using MyEbookLibrary.Common.DTO.Publisher;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Common.DTO.Book
{
    public class BookDTO : BaseDTO
    {
        public string Title { get; set; }
        public string? BookImage { get; set; } = null;
        public string? Subject { get; set; } = null;
        public string? ISBN10 { get; set; } = null;
        public string? ISBN13 { get; set; } = null;
        public string? Description { get; set; } = null;
        public string? EditionName { get; set; } = null;
        public DateTime? PublishedDate { get; set; } = null;
        public int? NoOfPages { get; set; } = null;
        public List<FileDataDto> Files { get; set; } = new List<FileDataDto>();
        public PublisherDTO? Publisher { get; set; } = null;
        public List<GenreDTO> Genres { get; set; } = new List<GenreDTO>();
        public List<LanguageDTO> Languages { get; set; } = new List<LanguageDTO>();
        public List<BookAuthorDTO> Authors { get; set; } = new List<BookAuthorDTO>();
    }
}
