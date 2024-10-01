using CrudRedisActiveMQ.Interfaces;
using CrudRedisActiveMQ.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace CrudRedisActiveMQ.Caching
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _redisDb;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _redisDb = redis.GetDatabase();
        }

        public async Task<Product> GetProductFromCacheAsync(int id)
        {
            var cachedProduct = await _redisDb.StringGetAsync(id.ToString());
            if (!cachedProduct.IsNullOrEmpty)
            {
                return JsonSerializer.Deserialize<Product>(cachedProduct);
            }
            return null;
        }

        public async Task SetProductToCacheAsync(Product product)
        {
            var serializedProduct = JsonSerializer.Serialize(product);
            await _redisDb.StringSetAsync(product.Id.ToString(), serializedProduct, TimeSpan.FromMinutes(10));
        }
    }
}
