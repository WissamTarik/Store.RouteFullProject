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

        Task<TEntity?> GetAsync(TEntity entity);

        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
