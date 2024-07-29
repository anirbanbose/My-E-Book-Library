using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Data.Repository.Specifications
{
    public class Specification<T> where T : class
    {
        public Specification()
        {

        }

        public void AddCriteria(Expression<Func<T, bool>> criteria)
        {
            if (criteria != null)
            {
                Criteria = criteria;
            }
        }
        public void AddIncludes(Expression<Func<T, object>>[] includes)
        {
            if (includes != null && includes.Length > 0)
            {
                Includes?.AddRange(includes);
            }
        }
        public void AddOrderBy(Expression<Func<T, object>>? orderBy)
        {
            OrderBy = orderBy;
        }

        public void AddOrderByDesc(Expression<Func<T, object>>? orderByDesc)
        {
            OrderByDesc = orderByDesc;
        }

        public Expression<Func<T, bool>>? Criteria { get; private set; }
        public List<Expression<Func<T, object>>>? Includes { get; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>>? OrderBy { get; private set; }
        public Expression<Func<T, object>>? OrderByDesc { get; private set; }
    }
}
