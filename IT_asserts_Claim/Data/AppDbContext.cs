using IT_asserts_Claim.Models;
using Microsoft.EntityFrameworkCore;

namespace IT_asserts_Claim.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Accessory> Accessories { get; set; }

    }
}
