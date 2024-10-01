using CrudRedisActiveMQ.Models;
using Microsoft.EntityFrameworkCore;

namespace CrudRedisActiveMQ.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
    }
}
