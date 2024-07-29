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
    [Table("Genre", Schema = "EbookLibrary")]
    public partial class Genre : BaseEntity
    {
        public Genre()
        {            
        }
        [SetsRequiredMembers]
        public Genre(string genreName)
        {
            GenreName = genreName;
        }
        [MaxLength(100)]
        [NotNull()]
        public required string GenreName { get; set; }
        public ICollection<Book> Books { get; set; } = new HashSet<Book>();
    }
}
