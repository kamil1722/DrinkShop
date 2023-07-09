using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public DbSet<VVM.Models.Drinks> Drinks { get; set; } = default!;
    }
}
