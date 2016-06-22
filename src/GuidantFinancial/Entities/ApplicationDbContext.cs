using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;

namespace GuidantFinancial.Entities
{
    public sealed class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<Security> Securities { get; set; }
        public DbSet<SecurityType> SecurityTypes { get; set; }

        public ApplicationDbContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = Startup.Configuration["database:connection"];
            optionsBuilder.UseSqlServer(connectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
