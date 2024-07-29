using MyEbookLibrary.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Common.DTO.Requests
{
    public record BookSearchRequestDTO(int PageNumber, int PageSize, string? SearchText, string SortColumnName, OrderByEnum SortBy, int? AuthorId, int? PublisherId, int? GenreId, int? LanguageId);
    
}
