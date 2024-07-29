using MyEbookLibrary.Data.EF.Entities;
using MyEbookLibrary.Data.EF;
using MyEbookLibrary.Data.Repository.Contracts;
using MyEbookLibrary.Data.Repository.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyEbookLibrary.Common.DTO;
using static System.Reflection.Metadata.BlobBuilder;
using AutoMapper;
using System.Linq.Expressions;

namespace MyEbookLibrary.Data.Repository.Repositories
{
    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<Author>> GetByCriteriaAsync(Expression<Func<Author, bool>>? criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException("Null criteria");
            }
            var query = DbContext.Set<Author>().AsQueryable();
            query = query.Include(a => a.BookAuthors).ThenInclude(b => b.Book);
            var result = query.Where(criteria).ToList();
            return await Task.FromResult(result);
        }

    }
}
