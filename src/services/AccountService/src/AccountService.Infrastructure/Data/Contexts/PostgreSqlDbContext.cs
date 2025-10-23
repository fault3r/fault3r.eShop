
using System;
using AccountService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Infrastructure.Data.Contexts
{
    public class PostgreSqlDbContext : DbContext
    {
        public PostgreSqlDbContext(DbContextOptions<PostgreSqlDbContext> dbContext)
            : base(dbContext) { }

        public DbSet<Account> Accounts => Set<Account>();

        public DbSet<Role> Roles => Set<Role>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Role>().HasKey(x => x.Id);
            builder.Entity<Role>().HasData(
                new Role { Name = nameof(Account) });
            builder.Entity<Role>()
                .HasMany(x => x.Accounts)
                .WithOne(x => x.Role)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
            

        }
        
    }
}
