using Microsoft.EntityFrameworkCore;

namespace DrinksProject.Data
{
    public class MyContext(DbContextOptions<MyContext> options) : DbContext(options)
    {
        public DbSet<Models.Drinks> Drinks { get; set; } = default!;
    }
}
