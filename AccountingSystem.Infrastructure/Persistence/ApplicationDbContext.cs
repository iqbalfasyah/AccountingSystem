using Microsoft.EntityFrameworkCore;
using AccountingSystem.Domain.Entities;

namespace AccountingSystem.Infrastructure.Persistence { 
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
       
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Tenant
            modelBuilder.Entity<Tenant>().HasData(new Tenant
            {
                Id = 1,
                Name = "Default Tenant"
            });

            // Use a pre-generated hash value
            var testUser = new User
            {
                Id = 1,
                FullName = "Admin User",
                Email = "balfasyah@gmail.com",
                TenantId = 1,
                // Pre-generated hash for "p@ssword123!"
                PasswordHash = "$2a$11$K3g6XoqyX4vqaWxRxKMhX.WnxJ3wZKfXjuZB4E4Hz0GKXm0Td72fa"
            };

            modelBuilder.Entity<User>().HasData(testUser);
        }
    }
}
