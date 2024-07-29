using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Common.DTO.Language
{
    public class LanguageListDTO
    {
        public LanguageListDTO(int id, string languageName, string languageCode, int bookCount, int addedBy)
        {
            Id = id;
            LanguageName = languageName;
            LanguageCode = languageCode;
            BookCount = bookCount;
            AddedBy = addedBy;
        }
        public int Id { get; }
        public string LanguageName { get; }
        public string LanguageCode { get; }
        public int BookCount { get; }
        public int AddedBy { get; }
    }
}
