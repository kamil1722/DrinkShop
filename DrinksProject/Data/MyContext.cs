using Microsoft.EntityFrameworkCore;
using DrinksProject.Models;

namespace DrinksProject.Data
{
    public class MyContext : DbContext
    {
        public MyContext (DbContextOptions<MyContext> options)
            : base(options)
        {
        }

        public DbSet<Drinks> Drinks { get; set; } = default!;
    }
}
