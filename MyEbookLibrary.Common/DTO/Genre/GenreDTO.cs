using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyEbookLibrary.Common.DTO.Book;

namespace MyEbookLibrary.Common.DTO.Genre
{
    public class GenreDTO : BaseDTO
    {
        public GenreDTO(int id, string genreName)
        {
            Id = id;
            GenreName = genreName;
        }
        [Required(ErrorMessage = "Genre is required")]
        [MaxLength(100, ErrorMessage = "Genre mustn't be more than 100 characters long.")]
        [NotNull()]
        public string GenreName { get; set; }
    }
}
