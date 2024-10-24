using StackExchange.Redis;
using Store.DEMO.Core.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.DEMO.Service.Services.Caches
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _database;
        public CacheService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task<string> GetCacheKeyAsync(string key)
        {
            var cacheResponse = await _database.StringGetAsync(key);
            if (cacheResponse.IsNullOrEmpty) return null;
            return cacheResponse.ToString();
        }

        public async Task SetCacheKeyAsnc(string key, object value, TimeSpan expireTime)
        {
            if (value is null) return;
            var option = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            await _database.StringSetAsync(key, JsonSerializer.Serialize(value, option), expireTime);
        }
    }
}
