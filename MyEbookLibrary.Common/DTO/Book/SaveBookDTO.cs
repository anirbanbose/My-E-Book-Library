using MyEbookLibrary.Common.DTO.Author;
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
    public class SaveBookDTO : BaseDTO
    {
        [MaxLength(200, ErrorMessage = "Book title mustn't be more than 200 characters long.")]
        [Required(ErrorMessage = "Book title is required")]
        public string Title { get; set; }
        public string? BookImage { get; set; } = null;
        public bool ImageUploaded { get; set; } = false;
        [MaxLength(150, ErrorMessage = "Subject mustn't be more than 150 characters long.")]
        public string? Subject { get; set; } = null;
        [MaxLength(10, ErrorMessage = "ISBN10 mustn't be more than 10 characters long.")]
        [RegularExpression("([0-9]*)", ErrorMessage = "ISBN10 must be a digit only value.")]
        public string? ISBN10 { get; set; } = null;
        [MaxLength(13, ErrorMessage = "ISBN13 mustn't be more than 13 characters long.")]
        [RegularExpression("([0-9]*)", ErrorMessage = "ISBN13 must be a digit only value.")]
        public string? ISBN13 { get; set; } = null;
        [MaxLength(500, ErrorMessage = "Description mustn't be more than 500 characters long.")]
        public string? Description { get; set; } = null;
        [MaxLength(150, ErrorMessage = "Edition Name mustn't be more than 150 characters long.")]
        public string? EditionName { get; set; } = null;
        public string? PublishedDate { get; set; } = null;
        [RegularExpression("([0-9]*)", ErrorMessage = "Number Of Pages must be a digit only value.")]
        public int? NoOfPages { get; set; } = null;
        public List<FileDataDto> Files { get; set; } = new List<FileDataDto>();
        public PublisherDTO? Publisher { get; set; } = null;
        public List<GenreDTO> Genres { get; set; } = new List<GenreDTO>();
        public List<LanguageDTO> Languages { get; set; } = new List<LanguageDTO>();
        public List<BookAuthorDTO> Authors { get; set; } = new List<BookAuthorDTO>();

    }
}
