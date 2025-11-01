
using System;
using AccountService.Application.Interfaces.Services;
using AccountService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Infrastructure.Data.Contexts
{
    public class PostgresDbContext : DbContext
    {
        private readonly ILoggerService<PostgresDbContext> _logger;

        public PostgresDbContext(DbContextOptions<PostgresDbContext> options,
            ILoggerService<PostgresDbContext> logger) : base(options)
        {
            _logger = logger;
            _logger.LogInformation("instance created.");
        }

        public DbSet<Account> Accounts => Set<Account>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Account>(builder =>
            {
                builder.HasKey(p => p.Id);
            })
            builder.Entity<Account>()
                .HasIndex(p => p.Email)
                .IsUnique();
            builder.Entity<Account>().OwnsOne(p => p.Role, builder =>
            {
                builder.Property(p => p.)
            });
        }
    }
}