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
    public interface IAuthorRepository : IGenericRepository<Author>
    {
        Task<IReadOnlyList<Author>> GetByCriteriaAsync(Expression<Func<Author, bool>> criteria);
    }
}
