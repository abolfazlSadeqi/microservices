using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class TranscationBaseConfiguration : IEntityTypeConfiguration<TranscationBase>
{
    public void Configure(EntityTypeBuilder<TranscationBase> builder)
    {
        builder.ToTable("TranscationBase");

        builder.Ignore(e => e.DomainEvents);

        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        //builder.Property(p => p.RowVersion)
        //   .IsConcurrencyToken()
        //   .ValueGeneratedOnAddOrUpdate();

        builder.Property(e => e.CustomerId)
            .HasMaxLength(100).HasColumnType("int");

        builder.Property(e => e.Amount)
            .HasMaxLength(100).HasColumnType("bigint");

        builder.Property(e => e.Date)
          .HasMaxLength(100).HasColumnType("date");


        builder.Property(e => e.DeviceType)
         .HasMaxLength(20).HasColumnType("VARCHAR(100)");


    }
}
