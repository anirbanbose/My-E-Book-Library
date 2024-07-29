using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Common.DTO.Publisher
{
    public class PublisherDTO : BaseDTO
    {
        public PublisherDTO(int id, string publisherName)
        {
            Id = id;
            PublisherName = publisherName;
        }
        [Required(ErrorMessage = "Publisher Name is required")]
        [MaxLength(150, ErrorMessage = "Publisher Name mustn't be more than 150 characters long.")]
        [NotNull()]
        public string PublisherName { get; set; }
    }
}
