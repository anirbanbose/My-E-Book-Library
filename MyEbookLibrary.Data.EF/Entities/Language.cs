using MyEbookLibrary.Data.EF.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Data.EF.Entities
{
    [Table("Language", Schema = "EbookLibrary")]
    public partial class Language : BaseEntity
    {
        public Language()
        {            
        }
        [SetsRequiredMembers]
        public Language(string languageName, string languageCode)
        {
            LanguageName = languageName;
            LanguageCode = languageCode;            
        }
        [MaxLength(100)]
        [NotNull()]
        public required string LanguageName { get; set; }

        [MaxLength(4)]
        [NotNull()]
        public required string LanguageCode { get; set; }
        public ICollection<Book> Books { get; set; } = new HashSet<Book>();
    }
}
