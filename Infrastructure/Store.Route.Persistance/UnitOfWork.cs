using Store.Route.Domain.Contracts;
using Store.Route.Domain.Entities;
using Store.Route.Persistance.Data.Contexts;
using Store.Route.Persistance.Repositories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Persistance
{
    public class UnitOfWork(StoreDbContext _context) : IUnitOfWork
    {


        //public IGenericRepository<TKey, TEntity> GetRepository<TKey, TEntity>() where TEntity : BaseEntity<TKey>
        //{
        //    Dictionary<string, object> _Repositories = new Dictionary<string, object>();

        //    var type= typeof(TEntity).Name;
        //    if (!_Repositories.ContainsKey(type)) {

        //        _Repositories.Add(type, new GenericRepository<TKey, TEntity>(_context));  
        //    }
        //    return (IGenericRepository<TKey, TEntity>) _Repositories[type];
        //}

        public IGenericRepository<TKey, TEntity> GetRepository<TKey, TEntity>() where TEntity : BaseEntity<TKey>
        {
            ConcurrentDictionary<string, object> _Repository = new ConcurrentDictionary<string, object>(); 
         
                var Repository = _Repository.GetOrAdd(typeof(TEntity).Name,new GenericRepository<TKey,TEntity>(_context));
            return (IGenericRepository<TKey, TEntity>)Repository;


                }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
