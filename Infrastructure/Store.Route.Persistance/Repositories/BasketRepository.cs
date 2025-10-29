using StackExchange.Redis;
using Store.Route.Domain.Contracts;
using Store.Route.Domain.Entities.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Route.Persistance.Repositories
{
    public class BasketRepository(IConnectionMultiplexer connection) : IBasketRepository
    {
        private readonly IDatabase _database = connection.GetDatabase();
        public async Task<CustomerBasket?> GetBasketAsync(string id)
        {
            var RedisValue = await _database.StringGetAsync(id);
            if(RedisValue.IsNullOrEmpty) { return null; }
            var basket = JsonSerializer.Deserialize<CustomerBasket>(RedisValue);
            if(basket == null) { return null; }
            return basket;

        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket, TimeSpan? timeToLive = null)
        {
            var RedisValue=JsonSerializer.Serialize(basket);
            
         var Flag=  await  _database.StringSetAsync(basket.Id, RedisValue, TimeSpan.FromDays(30));

            return Flag ? await GetBasketAsync(basket.Id):null;
        }
        public async Task<bool> DeleteBasketAsync(string id)
        {
           
            var Flag = await _database.KeyDeleteAsync(id);
            return Flag;
        }

      
    }
}
