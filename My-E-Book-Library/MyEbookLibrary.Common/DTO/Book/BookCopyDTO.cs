using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyEbookLibrary.Common.DTO.Author;
using MyEbookLibrary.Common.DTO.Genre;
using MyEbookLibrary.Common.DTO.Language;
using MyEbookLibrary.Common.DTO.Publisher;

namespace MyEbookLibrary.Common.DTO.Book
{
    public class BookCopyDTO : BaseDTO
    {
        public BookCopyDTO(){}

        public BookCopyDTO(string title)
        {
            Title = title;
        }
        public string Title { get; set; }
        
        public IList<FileDataDto> Copies { get; set; } = new List<FileDataDto>();
    }
}
