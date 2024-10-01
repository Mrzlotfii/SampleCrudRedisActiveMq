using CrudRedisActiveMQ.Models;

namespace CrudRedisActiveMQ.Interfaces
{
    public interface ICacheService
    {
        Task<Product> GetProductFromCacheAsync(int id);
        Task SetProductToCacheAsync(Product product);
    }
}
