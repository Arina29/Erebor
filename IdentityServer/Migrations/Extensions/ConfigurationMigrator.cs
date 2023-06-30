using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using IdentityServer.Models.DomainModels;

namespace IdentityServer.Migrations.Extensions;

public static class ConfigurationMigrator
{
    public static void SeedConfigData(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

        context.Database.Migrate();

        if (!context.Clients.Any())
        {
            Console.WriteLine("Clients being populated");
            foreach (var client in Config.Clients)
            {
                context.Clients.Add(client.ToEntity());
            }

            context.SaveChanges();
        }
        else
        {
            Console.WriteLine("Clients already populated");
        }

        if (!context.IdentityResources.Any())
        {
            Console.WriteLine("IdentityResources being populated");
            foreach (var resource in Config.IdentityResources)
            {
                context.IdentityResources.Add(resource.ToEntity());
            }

            context.SaveChanges();
        }
        else
        {
            Console.WriteLine("IdentityResources already populated");
        }

        if (!context.ApiScopes.Any())
        {
            Console.WriteLine("ApiResources being populated");
            foreach (var resource in Config.ApiScopes)
            {
                context.ApiScopes.Add(resource.ToEntity());
            }

            context.SaveChanges();
        }
        else
        {
            Console.WriteLine("ApiScopes already populated");
        }
    }
}
