using MyEbookLibrary.Common.DTO.Account;
using MyEbookLibrary.Common.DTO.Language;
using MyEbookLibrary.Common.DTO.Requests;
using MyEbookLibrary.Common.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Service.Contracts
{
    public interface ILanguageService
    {
        Task<ListResult<LanguageListDTO>> GetLanguageList(ListRequestDTO listRequest, UserDTO user);
        Task<DropdownResult<LanguageDropdownListDTO>> GetLanguageDropdownList(string? search, UserDTO user);
        Task<DetailResult<LanguageDTO>> GetLanguage(int id, UserDTO user);
    }
}
