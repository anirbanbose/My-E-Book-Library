using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyEbookLibrary.Common.DTO.Book;

namespace MyEbookLibrary.Common.DTO.Language
{
    public class LanguageDTO : BaseDTO
    {
        public LanguageDTO(int id, string languageName)
        {
            Id = id;
            LanguageName = languageName;
        }
        [Required(ErrorMessage = "Language is required")]
        [MaxLength(100, ErrorMessage = "Language mustn't be more than 100 characters long.")]
        [NotNull()]
        public string LanguageName { get; set; }
    }
}
