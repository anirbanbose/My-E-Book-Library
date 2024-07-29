using MyEbookLibrary.Data.Repository.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Data.Repository.Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetAsync(int id);
        Task<T?> GetAsync(int id, Expression<Func<T, bool>>? criteria);
        Task<T?> GetAsync(int id, Specification<T> specification);        
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task UpdateMultipleAsync(IList<T> entities);
        Task AddMultipleAsync(IList<T> entities);
        //void Update(T entity);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
        Task<IReadOnlyList<T>> GetBySpecificationAsync(Specification<T> specification, int? recordsToBeFetched = null);
    }
}
