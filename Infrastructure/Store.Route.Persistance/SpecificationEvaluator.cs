using Microsoft.EntityFrameworkCore;
using Store.Route.Domain.Contracts;
using Store.Route.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Persistance
{
    public static class SpecificationEvaluator
    {
        // _context.Products.Include(p=>p.Brand).Include(p=>p.Type).FirstOrDefaultAsync(p=>p.Id==id as int?) 
        //generate Dynamic query
        //inputQuery:_context.Products

        //
        public static IQueryable<TEntity> GetQuery<TKey,TEntity>(IQueryable<TEntity> inputQuery,ISpecifications<TKey,TEntity> spec) where TEntity:BaseEntity<TKey>
        {
            var query = inputQuery;
            if(spec.Criteria is not null)
            {
                query=  query.Where(spec.Criteria);
            }

            if(spec.OrderBy is not null)
            {
               query= query.OrderBy(spec.OrderBy);
            }else if(spec.OrderByDescending is not null)
            {
              query=  query.OrderByDescending(spec.OrderByDescending);
            }

            if (spec.IsPagination)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            query = spec.Includes.Aggregate(query, (query, includeExpression) => query.Include(includeExpression));
            return query;
        }
    }
}
