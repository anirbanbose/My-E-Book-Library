using MyEbookLibrary.Common.DTO.Account;
using MyEbookLibrary.Common.DTO.Author;
using MyEbookLibrary.Common.DTO.Dashboard;
using MyEbookLibrary.Common.DTO.Requests;
using MyEbookLibrary.Common.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Service.Contracts
{
    public interface IDashboardService
    {
        Task<ListResult<AuthorListBookCountDTO>> GetAuthorList(UserDTO user);
        Task<ListResult<GenreListBookCountDTO>> GetGenreList(UserDTO user);
        Task<ListResult<PublisherListBookCountDTO>> GetPublisherList(UserDTO user);
        Task<ListResult<LanguageListBookCountDTO>> GetLanguageList(UserDTO user);
    }
}
