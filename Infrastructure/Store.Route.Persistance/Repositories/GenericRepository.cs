using Microsoft.EntityFrameworkCore;
using Store.Route.Domain.Contracts;
using Store.Route.Domain.Entities;
using Store.Route.Domain.Entities.Products;
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

            if (typeof(TEntity) == typeof(Product))
            {
                return changeTracker?(IEnumerable<TEntity>) await _context.Products.Include(p=>p.Brand).Include(p=>p.Type).ToListAsync()
                                     :(IEnumerable<TEntity>)await _context.Products.Include(p=>p.Brand).Include(p=>p.Type).AsNoTracking().ToListAsync();
            }
            return changeTracker ? await _context.Set<TEntity>().ToListAsync()
                                 : await _context.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async Task<TEntity?> GetAsync(TKey id)
        {
            if (typeof(TEntity) == typeof(Product))
            {
                return  await _context.Products.Include(p=>p.Brand).Include(p=>p.Type).FirstOrDefaultAsync(p=>p.Id==id as int?) as TEntity;
            }
            return await _context.Set<TEntity>().FindAsync(id);
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

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TKey, TEntity> spec, bool changeTracker = false)
        {
            return  await ApplySpecifications(spec).ToListAsync();
        }

        public async Task<TEntity?> GetAsync(ISpecifications<TKey, TEntity> spec, TKey id)
        {
          return  await ApplySpecifications(spec).FirstOrDefaultAsync();
        }
        private IQueryable<TEntity> ApplySpecifications(ISpecifications<TKey,TEntity> spec)
        {
            return SpecificationEvaluator.GetQuery(_context.Set<TEntity>(), spec);
        }
    }
    
}
