using Erebor.Domain.Enums;
using Erebor.Domain.Models;
using Erebor.Infrastructure.Data.ValueGenerators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Erebor.Data.EntityConfigurations;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.Property(x => x.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(250);

        builder.Property(x => x.Dangerousness)
            .HasDefaultValue(Dangerousness.Neutral);

        builder.Property(x => x.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasValueGenerator(typeof(DateTimeOffsetValueGenerator))
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
    }
}