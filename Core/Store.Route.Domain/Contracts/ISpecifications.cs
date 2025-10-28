using Store.Route.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Domain.Contracts
{


    public interface ISpecifications<TKey,TEntity> where TEntity : BaseEntity<TKey>
    {
        public List<Expression<Func<TEntity,object>>> Includes { get; set; }

        public Expression<Func<TEntity,bool>>? Criteria { get; set; }

        public Expression<Func<TEntity,object>>? OrderBy { get; set; }

        public Expression<Func<TEntity,object>>? OrderByDescending { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public bool IsPagination { get; set; }
    }
}
