using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Erebor.Infrastructure.Data.ValueGenerators;
public class DateTimeOffsetValueGenerator : ValueGenerator<DateTimeOffset>
{
    public override DateTimeOffset Next(EntityEntry entry)
    {
        if (entry is null)
        {
            throw new ArgumentNullException(nameof(entry));
        }

        return DateTimeOffset.UtcNow;
    }

    public override bool GeneratesTemporaryValues => false;
}
