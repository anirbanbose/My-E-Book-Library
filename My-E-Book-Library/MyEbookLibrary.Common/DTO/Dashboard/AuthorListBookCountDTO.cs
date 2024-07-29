using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Common.DTO.Dashboard
{
    public class AuthorListBookCountDTO
    {
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public int BookCount { get; set; }
    }
}
