using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Common.DTO.Publisher
{
    public class PublisherDropdownListDTO
    {
        public int Id { get; }
        public string PublisherName { get; }
        public PublisherDropdownListDTO(int id, string publisherName)
        {
            Id = id;
            PublisherName = publisherName;
        }
    }
}
