using MyEbookLibrary.Common.DTO.Account;
using MyEbookLibrary.Common.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Service.Contracts
{
    public interface IUserService
    {
        Task<DetailResult<UserDTO>> GetProfile(int userId);
        Task<SaveResult<UserDTO>> SaveProfile(SaveProfileDTO user, int userId);
        Task<SaveResult> ChangePassword(ChangePasswordDTO passwordDto, int userId);
        Task<bool> IsEmailAvailable(string email);
        Task<SaveResult> Register(RegistrationDTO registration);
    }
}
