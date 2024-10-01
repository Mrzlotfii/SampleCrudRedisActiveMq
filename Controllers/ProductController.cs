using CrudRedisActiveMQ.Models;
using Microsoft.AspNetCore.Mvc;
using CrudRedisActiveMQ.Messaging;
using CrudRedisActiveMQ.Services;
using System.Text.Json;

namespace CrudRedisActiveMQ.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;
        private readonly ActiveMqService _activeMqService;

        public ProductsController(ProductService productService, ActiveMqService activeMqService)
        {
            _productService = productService;
            _activeMqService = activeMqService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productService.GetProductAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            await _productService.AddProductAsync(product);
            _activeMqService.SendMessage("ProductQueue", JsonSerializer.Serialize(product));
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }
    }
}
