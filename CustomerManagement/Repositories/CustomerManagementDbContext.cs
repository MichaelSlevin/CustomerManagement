using CustomerManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerManagement.Repositories
{
    public class CustomerManagementDbContext : DbContext
    {
        private readonly IConfiguration _config;
        public CustomerManagementDbContext(IConfiguration config)
        {
            _config = config;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .EnableSensitiveDataLogging()
                .UseSqlServer(_config.GetConnectionString("SQL"));
            
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }

    }
}