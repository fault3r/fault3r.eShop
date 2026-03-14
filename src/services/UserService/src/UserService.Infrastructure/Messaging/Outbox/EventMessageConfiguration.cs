
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Messaging.Outbox;

namespace UserService.Infrastructure.Messaging.Outbox;

public sealed class EventMessageConfiguration : IEntityTypeConfiguration<EventMessage>
{
    public void Configure(EntityTypeBuilder<EventMessage> builder)
    {
        builder.ToTable("Events");

        builder.HasKey(p => p.Id);

        builder.HasIndex(p => p.Timestamp);

        builder.Property(p => p.Id) 
            .HasColumnName("Id")
            .ValueGeneratedNever();

        builder.Property(p => p.Timestamp)
            .HasColumnName("Timestamp")
            .IsRequired();

        builder.Property(p => p.Type)
            .HasColumnName("Type")
            .IsRequired();

        builder.Property(p => p.Payload)
            .HasColumnName("Payload")
            .IsRequired();

        builder.Property(p => p.Processed)
            .HasColumnName("Processed")
            .IsRequired();
            
        builder.Property(p => p.ProcessedAt)
            .HasColumnName("ProcessedAt")
            .IsRequired();

        builder.Property(p => p.CorrelationId)
            .HasColumnName("CorrelationId")
            .IsRequired();
    }
}
