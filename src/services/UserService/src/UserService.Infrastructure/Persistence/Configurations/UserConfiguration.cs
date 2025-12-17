
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.ValueObjects;

namespace UserService.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(p => p.Id);

        builder.HasIndex(p => p.Email)
            .IsUnique();

        builder.Property(p => p.Id)
            .HasColumnName("Id")
            .HasConversion(
                identity => identity.Value,
                value => Identity.From(value)
            )
            .ValueGeneratedNever();

        builder.Property(p => p.Email)
            .HasColumnName("Email")
            .HasConversion(
                email => email.Value,
                value => Email.Parse(value)
            )
            .IsRequired();

        builder.Property(p => p.PasswordHash)
            .HasColumnName("Password")
            .HasConversion(
                passwordHash => passwordHash.Value,
                value => PasswordHash.Parse(value)
            )
            .IsRequired();

        builder.OwnsOne(p => p.FullName, fullName =>
        {
            fullName.Ignore(p => p.Value);
             
            fullName.Property(p => p.FirstName)
                .HasColumnName("FirstName")
                .IsRequired();

            fullName.Property(p => p.LastName)
                .HasColumnName("LastName")
                .IsRequired();
        })
            .Navigation(p => p.FullName)
            .IsRequired();

        builder.Property(p => p.Role)
            .HasColumnName("Role")
            .HasConversion(
                role => role.Value.ToString(),
                value => Role.Parse(value)
            )
            .IsRequired();                 

        builder.Property(p => p.Status)
            .HasColumnName("Status")
            .HasConversion(
                status => status.Value.ToString(),
                value => Status.Parse(value)
            )
            .IsRequired();           

        builder.Ignore(p => p.DomainEvents);
    }
}
