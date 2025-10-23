
using System;
using AccountService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Infrastructure.Data.Contexts
{
    public class PostgresDbContext : DbContext
    {
        public PostgresDbContext(DbContextOptions<PostgresDbContext> dbContext)
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

            builder.Entity<Account>().HasKey(x => x.Id);
            builder.Entity<Account>().HasIndex(x => x.Email)
                .IsUnique();
            builder.Entity<Account>()
                .HasOne(x => x.Role)
                .WithMany(x => x.Accounts)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        }    
    }
}
