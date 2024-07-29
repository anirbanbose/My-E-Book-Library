using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Common.DTO.Book
{
    public class FileDataDto : BaseDTO
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public int FileSize { get; set; }
        public string? FileData { get; set; } // Base64 string
    }
}
