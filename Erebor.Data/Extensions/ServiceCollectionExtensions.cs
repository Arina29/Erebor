using System.Reflection;
using Erebor.Data.Repositories;
using Erebor.Infrastructure.Attributes;
using Erebor.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Erebor.Data.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddDatabaseContext(this IServiceCollection services, string connectionString) =>
        services
            .AddDbContextFactory<EreborDbContext>(options => options.UseSqlServer(connectionString))
            .AddEntityRepositories(AppDomain.CurrentDomain.GetAssemblies());

    public static IServiceCollection AddEntityRepositories(this IServiceCollection services, params Assembly[] assemblies)
    {
        assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsDefined(typeof(EntityObjectAttribute)))
            .ToList()
            .ForEach(
                element => services.AddTransient(
                    typeof(IRepository<>).MakeGenericType(element),
                    typeof(Repository<>).MakeGenericType(element)));

        return services;
    }
}