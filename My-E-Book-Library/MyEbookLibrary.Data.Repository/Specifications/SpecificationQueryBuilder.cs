using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Data.Repository.Specifications
{
    public static class SpecificationQueryBuilder
    {
        public static IQueryable<T> GetQuery<T>(IQueryable<T> inputQuery, Specification<T> specification) where T : class
        {
            var query = inputQuery;
            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria).AsQueryable();
            }
            if (specification.Includes != null && specification.Includes.Count > 0)
            {
                query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));
            }
            if(specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }
            if (specification.OrderByDesc != null)
            {
                query = query.OrderByDescending(specification.OrderByDesc);
            }
            return query;
        }
    }
}
