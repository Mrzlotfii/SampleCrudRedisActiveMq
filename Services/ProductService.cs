using CrudRedisActiveMQ.Interfaces;
using CrudRedisActiveMQ.Models;

namespace CrudRedisActiveMQ.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICacheService _cacheService;

        public ProductService(IProductRepository productRepository, ICacheService cacheService)
        {
            _productRepository = productRepository;
            _cacheService = cacheService;
        }

        public async Task<Product> GetProductAsync(int id)
        {
            // ابتدا از کش ردیس تلاش می‌کنیم داده را واکشی کنیم
            var cachedProduct = await _cacheService.GetProductFromCacheAsync(id);
            if (cachedProduct != null)
            {
                return cachedProduct;
            }

            // اگر دیتایی در کش نبود، از دیتابیس می‌خوانیم
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product != null)
            {
                await _cacheService.SetProductToCacheAsync(product);
            }
            return product;
        }

        public async Task AddProductAsync(Product product)
        {
            await _productRepository.AddProductAsync(product);
            await _cacheService.SetProductToCacheAsync(product);
        }
    }
}
