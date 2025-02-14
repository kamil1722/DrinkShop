using Drinks.AuthModule.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DrinksProject.Data
{
    public class MyIdentityDbContext(DbContextOptions<MyIdentityDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<UserProfile> UserProfiles { get; set; }
    }
}