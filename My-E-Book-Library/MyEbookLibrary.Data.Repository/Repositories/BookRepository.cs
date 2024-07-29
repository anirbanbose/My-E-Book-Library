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
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        public BookRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<Book>> GetByCriteriaAsync(Expression<Func<Book, bool>> criteria)
        {
            var query = DbContext.Set<Book>().AsQueryable();
            query = query
                .Include(book => book.Publisher)
                .Include(book => book.Genres.Where(g => !g.Deleted))
                .Include(book => book.Languages.Where(l => !l.Deleted))
                .Include(book => book.Copies.Where(c => !c.Deleted))
                .Include(book => book.BookAuthors).ThenInclude(ba => ba.Author)
                .Include(book => book.BookAuthors).ThenInclude(ba => ba.AuthorType);
            var result = query.Where(criteria);
            return await Task.FromResult(result.ToList());
        }

        public new async Task<Book?> GetAsync(int id, Expression<Func<Book, bool>>? criteria)
        {
            var query = DbContext.Set<Book>().AsQueryable();
            query = query
                .Include(book => book.Publisher)
                .Include(book => book.Genres.Where(g => !g.Deleted))
                .Include(book => book.Languages.Where(l => !l.Deleted))
                .Include(book => book.Copies.Where(c => !c.Deleted))
                .Include(book => book.BookAuthors).ThenInclude(ba => ba.Author)
                .Include(book => book.BookAuthors).ThenInclude(ba => ba.AuthorType);
            Book? result = (criteria != null ? query.Where(criteria) : query).FirstOrDefault(d => d.Id == id);
            return await Task.FromResult(result);
        }

    }
}
