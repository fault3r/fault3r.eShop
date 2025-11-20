using System;
using AccountService.Domain.Aggregates.Account;
using AccountService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountService.Infrastructure.Persistence.Configurations;

public sealed class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => new Identity(value))
            .ValueGeneratedNever();

        builder.Property(p => p.FullName)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(p => p.Email)
            .HasConversion(
                email => email.Address,
                value => new Email(value))
            .IsRequired()
            .HasMaxLength(256);
        builder.HasIndex(p => p.Email)
            .IsUnique();

        builder.Property(p => p.PasswordHash)
            .IsRequired()
            .HasMaxLength(1024);

        builder.Property(p => p.Role)
            .HasConversion(
                role => role.ToString(),
                name => new Role(name))
            .IsRequired()
            .HasMaxLength(16);

        builder.Property(a => a.Status)
            .HasConversion(
                status => status.ToString(),
                value => new Status(value))
            .IsRequired()
            .HasMaxLength(32);

        builder.Ignore(p => p.DomainEvents);
    }
}
