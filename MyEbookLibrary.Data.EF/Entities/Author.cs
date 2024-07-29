using MyEbookLibrary.Data.EF.Entities.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Data.EF.Entities
{
    [Table("Author", Schema = "EbookLibrary")]
    public partial class Author : BaseEntity
    {
        public Author()
        {
        }

        [SetsRequiredMembers]
        public Author(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;    
        }
        [MaxLength(100)]
        [NotNull()]
        public required string FirstName { get; set; }
        [MaxLength(100)]
        public string? MiddleName { get; set; } = null;
        [MaxLength(100)]
        [NotNull()]
        public required string LastName { get; set; }
        public ICollection<BookAuthor> BookAuthors { get; set; } = new HashSet<BookAuthor>();
    }
}
