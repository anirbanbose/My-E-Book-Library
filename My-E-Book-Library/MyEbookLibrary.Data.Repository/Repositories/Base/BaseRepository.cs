using Microsoft.EntityFrameworkCore;
using MyEbookLibrary.Data.EF.Entities.Base;
using MyEbookLibrary.Data.EF;
using MyEbookLibrary.Data.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MyEbookLibrary.Common.DTO;
using MyEbookLibrary.Data.EF.Entities;
using MyEbookLibrary.Data.Repository.Specifications;

namespace MyEbookLibrary.Data.Repository.Repositories.Base
{
    public abstract class BaseRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly DbContext DbContext;

        public BaseRepository(ApplicationDbContext context)
        {
            DbContext = context;
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await DbContext.Set<T>().AddAsync(entity);
            await SaveChangesAsync();
            return entity;
        }

        public virtual async Task AddMultipleAsync(IList<T> entities)
        {
            await DbContext.Set<T>().AddRangeAsync(entities);
            await SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await GetAsync(id);
            if (entity != null)
            {
                entity.Deleted = true;
                await UpdateAsync(entity);
            }
        }

        public virtual async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await DbContext.Set<T>().ToListAsync();
        }

        public virtual async Task SaveChangesAsync()
        {
            await DbContext.SaveChangesAsync();
        }

        //public void Update(T entity)
        //{
        //    DbContext.Set<T>().Attach(entity);
        //    DbContext.Entry(entity).State = EntityState.Modified;
        //}

        public virtual async Task<T> UpdateAsync(T entity)
        {
            DbContext.Set<T>().Attach(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
            await SaveChangesAsync();
            return entity;
        }

        public virtual async Task UpdateMultipleAsync(IList<T> entities)
        {
            DbContext.Set<T>().AttachRange(entities);
            foreach (var entity in entities)
            {
                DbContext.Entry(entity).State = EntityState.Modified;
            }
            await SaveChangesAsync();
        }

        public virtual async Task<T?> GetAsync(int id)
        {
            return await DbContext.Set<T>().FirstOrDefaultAsync(d => d.Id == id);
        }

        public virtual async Task<T?> GetAsync(int id, Expression<Func<T, bool>>? criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException("Null where condition");
            }
            var result = DbContext.Set<T>().Where(criteria).FirstOrDefault(d => d.Id == id);
            return await Task.FromResult(result); 
        }

        public virtual async Task<T?> GetAsync(int id, Specification<T> specification)
        {
            var query = DbContext.Set<T>().AsQueryable();
            var result = SpecificationQueryBuilder.GetQuery(query, specification).FirstOrDefault(d => d.Id == id);
            
            return await Task.FromResult(result);
        }

        public virtual async Task<IReadOnlyList<T>> GetBySpecificationAsync(Specification<T> specification, int? recordsToBeFetched = null)
        {
            var query = DbContext.Set<T>().AsQueryable();
            var result = SpecificationQueryBuilder.GetQuery(query, specification);
            return recordsToBeFetched.HasValue ? await Task.FromResult(result.Take(recordsToBeFetched.Value).ToList()) : await Task.FromResult(result.ToList());            
        }


    }

}
