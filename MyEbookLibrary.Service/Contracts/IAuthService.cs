using MyEbookLibrary.Common.DTO.Account;
using MyEbookLibrary.Common.DTO.AutheticationDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Service.Contracts
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(string email, string password, bool rememberMe, string? deviceId, string userAgent);
        Task<LoginResponse> RefreshToken(TokenDTO model);
        Task<UserDTO> GetUserFromAccessToken(string accessToken);
    }
}
