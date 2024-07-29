using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Common.DTO.Language
{
    public class LanguageDropdownListDTO
    {
        public LanguageDropdownListDTO(int id, string languageName)
        {
            Id = id;
            LanguageName = languageName;
        }
        public int Id { get; }
        public string LanguageName { get; }
    }
}
