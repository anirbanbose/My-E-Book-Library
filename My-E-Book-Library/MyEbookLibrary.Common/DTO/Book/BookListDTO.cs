using MyEbookLibrary.Common.DTO.Author;
using MyEbookLibrary.Common.DTO.Genre;
using MyEbookLibrary.Common.DTO.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Common.DTO.Book
{
    public class BookListDTO
    {
        public int Id { get; set; }
        public string? BookImage { get; set; }
        public string Title { get; }
        public string? Genres { get; } = null;
        public string? Publisher { get; } = null;
        public string? Authors { get; } = null;
        public string? Languages { get; } =  null;
        public int NoOfCopies { get; }

        public BookListDTO(int id, string title, string? bookImage, string? publisher, string? genreNames, string? authorNames, string? languages, int noOfCopies)
        {
            Id = id;
            Title = title;
            BookImage = bookImage;
            Genres = genreNames;
            Publisher = publisher;
            Authors = authorNames;
            Languages = languages;
            NoOfCopies = noOfCopies;
        }
    }
}
