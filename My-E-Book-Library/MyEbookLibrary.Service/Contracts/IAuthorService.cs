using MyEbookLibrary.Common.DTO.Account;
using MyEbookLibrary.Common.DTO.Author;
using MyEbookLibrary.Common.DTO.Requests;
using MyEbookLibrary.Common.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Service.Contracts
{
    public interface IAuthorService
    {
        Task<ListResult<AuthorListDTO>> GetAuthorList(ListRequestDTO listRequest, UserDTO user);
        Task<bool> DeleteAuthor(int id, UserDTO user);
        Task<SaveResult<AuthorDTO>> SaveAuthor(AuthorDTO author, UserDTO user);
        Task<DetailResult<AuthorDetailDTO>> GetAuthor(int id, UserDTO user);
        Task<DropdownResult<AuthorDropdownListDTO>> GetAuthorDropdownList(string? search, UserDTO user);
    }
}
