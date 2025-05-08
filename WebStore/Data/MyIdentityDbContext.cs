using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebStore.Data
{
    public class MyIdentityDbContext(DbContextOptions<MyIdentityDbContext> options) : IdentityDbContext(options)
    {
    }
}