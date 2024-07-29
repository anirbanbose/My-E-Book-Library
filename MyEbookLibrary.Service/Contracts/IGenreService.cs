using MyEbookLibrary.Common.DTO.Account;
using MyEbookLibrary.Common.DTO.Genre;
using MyEbookLibrary.Common.DTO.Requests;
using MyEbookLibrary.Common.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Service.Contracts
{
    public interface IGenreService
    {
        Task<ListResult<GenreListDTO>> GetGenreList(ListRequestDTO listRequest, UserDTO user);
        Task<bool> DeleteGenre(int id, UserDTO user);
        Task<DropdownResult<GenreDropdownListDTO>> GetGenreDropdownList(string? search, UserDTO user);
        Task<DetailResult<GenreDTO>> GetGenre(int id, UserDTO user);
        Task<SaveResult<GenreDTO>> SaveGenre(GenreDTO genre, UserDTO user);
    }
}
