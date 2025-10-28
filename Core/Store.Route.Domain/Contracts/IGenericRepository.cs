using Store.Route.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Domain.Contracts
{
    public interface IGenericRepository<TKey,TEntity> where TEntity:BaseEntity<TKey>
    {

        Task<IEnumerable<TEntity>> GetAllAsync(bool changeTracker=false);

        Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TKey, TEntity> spec, bool changeTracker = false);

        Task<TEntity?> GetAsync(TKey id);
        Task<TEntity?> GetAsync(ISpecifications<TKey,TEntity> spec,TKey id);
        Task<int> CountAsync(ISpecifications<TKey,TEntity> spec);

        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
