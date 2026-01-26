
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Messaging.Outbox;

namespace UserService.Infrastructure.Messaging.Outbox;

public sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("OuboxMessages");

        builder.HasKey(p => p.Id);

        builder.HasIndex(p => p.EnqueuedOn);

        builder.Property(p => p.Id)
            .HasColumnName("Id")
            .ValueGeneratedNever();

        builder.Property(p => p.EnqueuedOn)
            .HasColumnName("EnqueuedOn")
            .IsRequired();

        builder.Property(p => p.Type)
            .HasColumnName("Type")
            .IsRequired();

        builder.Property(p => p.Payload)
            .HasColumnName("Payload")
            .IsRequired();
        
        builder.Property(p => p.Published)
            .HasColumnName("Published")
            .IsRequired();

        builder.Property(p => p.CorrelationId)
            .HasColumnName("CorrelationId")
            .IsRequired();
    }
}
