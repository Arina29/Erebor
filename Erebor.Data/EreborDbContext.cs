using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Erebor.Data;

public class EreborDbContext: DbContext
{
    public EreborDbContext(DbContextOptions<EreborDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}