using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Common.DTO.Publisher
{
    public class PublisherListDTO
    {
        public PublisherListDTO(int id, string publisherName, int bookCount, int addedBy)
        {
            Id = id;
            PublisherName = publisherName;
            BookCount = bookCount;
            AddedBy = addedBy;
        }
        public int Id { get; }
        public string PublisherName { get; }
        public int BookCount { get; }
        public int AddedBy { get; }
    }
}
