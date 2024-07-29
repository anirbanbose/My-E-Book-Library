using MyEbookLibrary.Common.DTO;
using MyEbookLibrary.Data.EF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Data.Repository.Contracts
{
    public interface IBookRepository : IGenericRepository<Book>
    {
        Task<IReadOnlyList<Book>> GetByCriteriaAsync(Expression<Func<Book, bool>> criteria);
        new Task<Book?> GetAsync(int id, Expression<Func<Book, bool>>? criteria);
    }
}
