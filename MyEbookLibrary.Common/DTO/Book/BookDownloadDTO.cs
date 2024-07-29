using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Common.DTO.Book
{
    public class BookDownloadDTO
    {
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
    }
}
