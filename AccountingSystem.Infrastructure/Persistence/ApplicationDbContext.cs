using Microsoft.EntityFrameworkCore;
using AccountingSystem.Domain.Entities;

namespace AccountingSystem.Infrastructure.Persistence { 
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
       
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}