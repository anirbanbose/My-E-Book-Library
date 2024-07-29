using MyEbookLibrary.Common.DTO.Account;
using MyEbookLibrary.Common.DTO.AuthorType;
using MyEbookLibrary.Common.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Service.Contracts
{
    public interface IAuthorTypeService
    {
        Task<DropdownResult<AuthorTypeDropdownListDTO>> GetAuthorTypeDropdownList(UserDTO user);
    }
}
