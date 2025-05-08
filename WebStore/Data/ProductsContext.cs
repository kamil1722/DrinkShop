using Microsoft.EntityFrameworkCore;

namespace WebStore.Data
{
    public class ProductsContext(DbContextOptions<ProductsContext> options) : DbContext(options)
    {
        public DbSet<Models.Drinks> Drinks { get; set; } = default!;
    }
}
