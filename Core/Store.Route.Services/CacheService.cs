using Store.Route.Domain.Contracts;
using Store.Route.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Services
{
    public class CacheService(ICacheRepository _repository) : ICacheService
    {
        public async Task<string?> GetCacheValueAsync(string key)
        {
          var Result=  await _repository.GetAsync(key);
            return string.IsNullOrEmpty(Result) ? null:Result;
        }

        public async Task SetCacheValueAsync(string key, object value, TimeSpan duration)
        {
            await  _repository.SetAsync(key, value, duration);
        }
    }
}
