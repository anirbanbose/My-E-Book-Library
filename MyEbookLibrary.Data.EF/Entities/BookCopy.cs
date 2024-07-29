using MyEbookLibrary.Data.EF.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Data.EF.Entities
{
    [Table("BookCopy", Schema = "EbookLibrary")]
    public class BookCopy : BaseEntity
    {
        public BookCopy(){}

        public BookCopy(int addedBy, DateTime addedDate, DateTime updatedDate)
        {
            AddedBy = addedBy;
            AddedDate = addedDate;
            LastUpdatedDate = updatedDate;
        }

        public int BookId { get; set; }

        [Required]
        public string FileName { get; set; }
        [Required]
        public string FileType { get; set; }
        public int FileSize { get; set; }
        [Required]
        public byte[] BinaryFile { get; set; }
        public Book? Book { get; set; } = null;

    }
}
