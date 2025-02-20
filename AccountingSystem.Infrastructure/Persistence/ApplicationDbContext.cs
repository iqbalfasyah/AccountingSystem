using Microsoft.EntityFrameworkCore;
using AccountingSystem.Domain.Entities;
using System;
using AccountingSystem.Domain.Enum;

namespace AccountingSystem.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<ChartOfAccount> ChartOfAccount { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<AuditLog> AuditLog { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Default Tenant
            modelBuilder.Entity<Tenant>().HasData(new Tenant
            {
                Id = 1,
                Name = "Default Tenant",
                Domain = "ifuix.com",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            });

            // Seed Admin User
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                FullName = "Admin User",
                Email = "balfasyah@gmail.com",
                PasswordHash = "$2a$12$.dB4omAYKxYu3EMD6Vu8du.VturPvESheHhT8lrm6XKPp65C4lYtW",
                TenantId = 1,
                Role = UserRoleEnum.Admin
            });

            modelBuilder.Entity<AuditLog>()
                .Property(e => e.OldData)
                .HasColumnType("jsonb");

            modelBuilder.Entity<AuditLog>()
                .Property(e => e.NewData)
                .HasColumnType("jsonb");
        }
    }
}
