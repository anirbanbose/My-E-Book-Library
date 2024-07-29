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
    [Table("Publisher", Schema = "EbookLibrary")]
    public class Publisher : BaseEntity
    {
        public Publisher()
        {            
        }

        [SetsRequiredMembers]
        public Publisher(string publisherName)
        {
            PublisherName = publisherName;
        }

        [MaxLength(150)]
        [NotNull()]
        public required string PublisherName { get; set; }

        public ICollection<Book> Books { get; set; } = new HashSet<Book>();
    }
}
