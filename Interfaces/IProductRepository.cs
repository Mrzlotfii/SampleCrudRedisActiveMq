using CrudRedisActiveMQ.Models;

namespace CrudRedisActiveMQ.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(int id);
        Task AddProductAsync(Product product);
    }
}
