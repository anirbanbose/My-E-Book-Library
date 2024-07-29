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
    [Table("AuthorType", Schema = "EbookLibrary")]
    public class AuthorType : BaseEntity
    {
        public AuthorType(){}
        [SetsRequiredMembers]
        public AuthorType(string typeName)
        {
            TypeName = typeName;
        }

        [MaxLength(100)]
        [NotNull()]
        public required string TypeName { get; set; }

        public ICollection<BookAuthor> BookAuthors { get; set; } = new HashSet<BookAuthor>();
    }
}
