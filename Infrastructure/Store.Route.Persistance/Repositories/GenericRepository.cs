using Microsoft.EntityFrameworkCore;
using Store.Route.Domain.Contracts;
using Store.Route.Domain.Entities;
using Store.Route.Persistance.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Persistance.Repositories
{
    internal class GenericRepository<TKey, TEntity> (StoreDbContext _context): IGenericRepository<TKey, TEntity> where TEntity : BaseEntity<TKey>
    {

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool changeTracker = false)
        {
            return changeTracker ? (IEnumerable<TEntity>)await _context.Set<TEntity>().ToListAsync()
                                 : (IEnumerable<TEntity>) await _context.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async Task<TEntity?> GetAsync(TEntity entity)
        {
            return await _context.Set<TEntity>().FindAsync(entity);
        }

        public async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }
        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }
        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

       
    }
}
