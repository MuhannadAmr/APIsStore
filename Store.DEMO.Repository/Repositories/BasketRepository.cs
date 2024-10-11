using StackExchange.Redis;
using Store.DEMO.Core.Entites;
using Store.DEMO.Core.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.DEMO.Repository.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer radis)
        {
            _database = radis.GetDatabase();
        }
        public async Task<bool> DeleteBasketAsync(string id)
        {
            return await _database.KeyDeleteAsync(id);
        }

        public async Task<CustmerBasket?> GetBasketAsync(string id)
        {
            var basket = await _database.StringGetAsync(id);
            
            return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustmerBasket>(basket);
        }

        public async Task<CustmerBasket?> UpdateBasketAsync(CustmerBasket basket)
        {
            var CreatedOrUpdatedBasket = await _database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));
            if(CreatedOrUpdatedBasket is false) return null;

            return await GetBasketAsync(basket.Id);
        }
    }
}
