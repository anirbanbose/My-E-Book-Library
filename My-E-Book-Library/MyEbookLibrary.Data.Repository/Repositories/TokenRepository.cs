using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyEbookLibrary.Common.DTO;
using MyEbookLibrary.Data.EF;
using MyEbookLibrary.Data.EF.Entities;
using MyEbookLibrary.Data.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Data.Repository.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        protected readonly DbContext DbContext;

        public TokenRepository(ApplicationDbContext context)
        {
            DbContext = context;
        }
        public async Task<UserRefreshToken?> GetUserRefreshTokenAsync(int userId, string deviceId)
        {
            var entity = await DbContext.Set<UserRefreshToken>().Where(d => 
                d.UserId == userId && 
                d.DeviceUniqueId.ToUpper() == deviceId.ToUpper() && 
                d.ExpiresAt > DateTime.UtcNow
            ).OrderByDescending(d => d.CreatedAt).FirstOrDefaultAsync();
            
            return entity;
        }

        public async Task<UserRefreshToken> AddUserRefreshTokenAsync(UserRefreshToken entity)
        {
            await DbContext.Set<UserRefreshToken>().AddAsync(entity);
            await DbContext.SaveChangesAsync();
            return entity;
        }


        public async Task UpdateUserRefreshTokenAsync(UserRefreshToken entity)
        {
            DbContext.Set<UserRefreshToken>().Attach(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
        }
    }
}
