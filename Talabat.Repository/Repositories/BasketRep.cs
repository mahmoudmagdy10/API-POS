using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.IRepository;

namespace Talabat.Repository.Repositories
{
    public class BasketRep : IBasketRepo
    {
        private readonly IDatabase _db;

        public BasketRep(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }
        public async Task<bool> DeleteBasketsAsync(string BasketId)
        {
            return await _db.KeyDeleteAsync(BasketId);
        }

        public async Task<CustomerBasket?> GetBasketsAsync(string BasketId)
        {
            var basket = await _db.StringGetAsync(BasketId);
            return basket.IsNull ? null : JsonSerializer.Deserialize<CustomerBasket>(basket);
        }

        public async Task<CustomerBasket?> UpdateBasketsAsync(CustomerBasket Basket)
        {

            var UpdateOrCreateBasket = await _db.StringSetAsync(Basket.Id, JsonSerializer.Serialize<CustomerBasket>(Basket), TimeSpan.FromDays(1));

            if (UpdateOrCreateBasket is false) return null;

            return await GetBasketsAsync(Basket.Id);
        }
    }
}
