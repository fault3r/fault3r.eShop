using System;
using AccountService.Domain.Aggregates.Account;
using AccountService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountService.Infrastructure.Persistence.Configurations;

public sealed class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> account)
    {
        account.ToTable("Accounts");

        account.HasKey(p => p.Id);
        account.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => new Identity(value))
            .ValueGeneratedNever();

        account.Property(p => p.FullName)
            .IsRequired()
            .HasMaxLength(128);

        account.Property(p => p.Email)
            .HasConversion(
                email => email.Address,
                value => new Email(value))
            .IsRequired()
            .HasMaxLength(256);
        account.HasIndex(p => p.Email)
            .IsUnique();

        account.Property(p => p.PasswordHash)
            .IsRequired()
            .HasMaxLength(1024);

        account.Property(p => p.Role)
            .HasConversion(
                role => role.Name,
                name => new Role(name))
            .IsRequired()
            .HasMaxLength(16);

        account.Property(a => a.Status)
            .HasConversion(
                status => status.Value,
                value => new Status(value))
            .IsRequired()
            .HasMaxLength(32);

        account.Ignore(p => p.DomainEvents);
    }
}
