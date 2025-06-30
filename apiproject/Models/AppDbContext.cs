using Microsoft.EntityFrameworkCore;

namespace apiproject.Models
{
    public class AppDbContext : DbContext  // Burada DbContext’den kalıtım şart!
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=apiproject;User Id=SA;Password=reallyStrongPwd123;TrustServerCertificate=True;");
        }
    }
}
