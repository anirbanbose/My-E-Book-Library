using MyEbookLibrary.Data.EF.Entities.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Data.EF.Entities
{
    [Table("Book", Schema = "EbookLibrary")]
    public partial class Book : BaseEntity
    {
        public Book(){}

        [SetsRequiredMembers]
        public Book(string title)
        {
            Title = title;
        }
        [MaxLength(200)]
        [NotNull()]
        public required string Title { get; set; }
        public string? BookImage { get; set; } = null;

        [MaxLength(150)]
        public string? Subject { get; set; } = null;
        [MaxLength(10)]
        public string? ISBN10 { get; set; } = null;
        [MaxLength(13)]
        public string? ISBN13 { get; set; } = null;
        [MaxLength(500)]
        public string? Description { get; set; } = null;
        [MaxLength(150)]
        public string? EditionName { get; set; } = null;
        public DateTime? PublishedDate { get; set; } = null;
        public int? NoOfPages { get; set; } = null;
        public int? PublisherId { get; set; } = null;
        public Publisher? Publisher { get; set; } = null;

        public ICollection<Genre> Genres { get; set; } = new HashSet<Genre>();
        public ICollection<BookAuthor> BookAuthors { get; set; } = new HashSet<BookAuthor>();
        public ICollection<Language> Languages { get; set; } = new HashSet<Language>();
        public ICollection<BookCopy> Copies { get; set; } = new HashSet<BookCopy>();
    }
}
