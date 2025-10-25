
using System;
using AccountService.Application.Interfaces.Services;
using AccountService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Infrastructure.Data.Contexts
{
    public class PostgresDbContext : DbContext
    {
        private readonly ILoggerService<PostgresDbContext> _logger;

        public PostgresDbContext(DbContextOptions<PostgresDbContext> dbContext,
            ILoggerService<PostgresDbContext> logger) : base(dbContext)
        {
            _logger = logger;
            _logger.LogInformation("instance created.");
        }

        public DbSet<Role> Roles => Set<Role>();

        public DbSet<Account> Accounts => Set<Account>();

        protected override void OnModelCreating(ModelBuilder builder)
        {            
            builder.Entity<Role>().HasKey(x => x.Id);
            builder.Entity<Role>().HasData(
                new Role(nameof(Account)));
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
