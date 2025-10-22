using Store.Route.Domain.Contracts;
using Store.Route.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Services.Specifications
{
    public class BaseSpecification<TKey, TEntity> : ISpecifications<TKey, TEntity> where TEntity : BaseEntity<TKey>
    {
        public List<Expression<Func<TEntity, object>>> Includes { get; set; } = new List<Expression<Func<TEntity, object>>>();
        public Expression<Func<TEntity, bool>>? Criteria { get ; set ; }
        public Expression<Func<TEntity, object>>? OrderBy { get ; set; }
        public Expression<Func<TEntity, object>>? OrderByDescending { get; set; }

        public BaseSpecification(Expression<Func<TEntity,bool>> expression)
        {
            Criteria = expression;
        }
        public void AddOrderBy(Expression<Func<TEntity,object>> expression)
        {
            OrderBy = expression;
        }
        public void AddOrderByDescending(Expression<Func<TEntity,object>> expression)
        {
            OrderByDescending = expression;
        }
    }
}
