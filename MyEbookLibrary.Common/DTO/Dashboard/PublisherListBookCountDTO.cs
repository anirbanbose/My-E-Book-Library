using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Common.DTO.Dashboard
{
    public class PublisherListBookCountDTO
    {
        public int PublisherId { get; set; }
        public string PublisherName { get; set; }
        public int BookCount { get; set; }
    }
}
