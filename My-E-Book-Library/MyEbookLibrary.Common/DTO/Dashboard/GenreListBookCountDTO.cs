using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Common.DTO.Dashboard
{
    public class GenreListBookCountDTO
    {
        public int GenreId { get; set; }
        public string GenreName { get; set; }
        public int BookCount { get; set; }
    }
}
