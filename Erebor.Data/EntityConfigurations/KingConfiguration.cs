using Erebor.Domain.Enums;
using Erebor.Domain.Models;
using Erebor.Infrastructure.Data.ValueGenerators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Erebor.Data.EntityConfigurations;
public class KingConfiguration : IEntityTypeConfiguration<King>
{
    public void Configure(EntityTypeBuilder<King> builder)
    {
        builder.Property(x => x.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(250);

        builder.Property(x => x.FullTitle)
            .HasMaxLength(100);

        builder.Property(x => x.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasValueGenerator(typeof(DateTimeOffsetValueGenerator))
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

        builder.Property(x => x.UpdatedAt)
            .ValueGeneratedOnUpdate()
            .HasValueGenerator(typeof(DateTimeOffsetValueGenerator));
    }
}
