using StackExchange.Redis;
using Store.Route.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Store.Route.Persistance.Repositories
{
    public class CacheRepository(IConnectionMultiplexer connection) : ICacheRepository
    {

        private readonly IDatabase _Database = connection.GetDatabase();
        public async Task<string?> GetAsync(string key)
        {
            var Result= await _Database.StringGetAsync(key);
            return !Result.IsNullOrEmpty ? Result : default;
        }

        public async Task SetAsync(string key, object value, TimeSpan duration)
        {
            await _Database.StringSetAsync(key, JsonSerializer.Serialize(value), duration);
       
            
        }
    }
}
