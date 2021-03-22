using System.Linq;
using Core.Entities;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery,
        ISpecification<TEntity> spec)
        {
            var query = inputQuery;
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria); //Product.Where(p => p.ProductTypeId == id)
            }

            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy); //Product.Where(p => p.ProductTypeId == id)
            }

            if (spec.OrderByDesending != null)
            {
                query = query.OrderByDescending(spec.OrderByDesending); //Product.Where(p => p.ProductTypeId == id)
            }

            if (spec.IsPagingEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            query = spec.Includes.Aggregate(query, (current, inculude) => current.Include(inculude));
            return query;
        }
    }
}