using Microsoft.EntityFrameworkCore;
using VVM.Models;

namespace VVM.Data
{
    public class VVMContext : DbContext
    {
        public VVMContext (DbContextOptions<VVMContext> options)
            : base(options)
        {
        }

        public DbSet<Drinks> Drinks { get; set; } = default!;
    }
}
