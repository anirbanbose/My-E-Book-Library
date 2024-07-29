using MyEbookLibrary.Common.DTO;
using MyEbookLibrary.Data.EF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Data.Repository.Contracts
{
    public interface ITokenRepository
    {
        Task<UserRefreshToken?> GetUserRefreshTokenAsync(int userId, string deviceId);

        Task<UserRefreshToken> AddUserRefreshTokenAsync(UserRefreshToken model);
        Task UpdateUserRefreshTokenAsync(UserRefreshToken model);
    }
}
