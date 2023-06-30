using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Data;

public class ConfigDbContext : ConfigurationDbContext
{
    public ConfigDbContext(DbContextOptions<ConfigurationDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}