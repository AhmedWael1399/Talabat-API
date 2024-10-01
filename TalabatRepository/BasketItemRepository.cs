using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TalabatCore.Entities;
using TalabatCore.Repositories;


namespace TalabatRepository
{
    public class BasketItemRepository : IBasketIItemRepository
    {
        private readonly IDatabase _database;

        public BasketItemRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _database.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
            var basket = await _database.StringGetAsync(basketId);
            
            //if (basket.IsNull) return null;
            //else return JsonSerializer.Deserialize<CustomerBasket>(basket);

            return basket.IsNull? null : JsonSerializer.Deserialize<CustomerBasket>(basket);
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
        {
            var jsonBasket = JsonSerializer.Serialize(basket);
            var createdOrUpdated = await _database.StringSetAsync(basket.Id, jsonBasket, TimeSpan.FromDays(1));
            if (!createdOrUpdated) return null;

            return await GetBasketAsync(basket.Id);
        }
    }
}
