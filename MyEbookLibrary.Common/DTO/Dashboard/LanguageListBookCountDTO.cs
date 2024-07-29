using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Common.DTO.Dashboard
{
    public class LanguageListBookCountDTO
    {
        public int LanguageId { get; set; }
        public string LanguageName { get; set; }
        public int BookCount { get; set; }
    }
}
