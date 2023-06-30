using System.Reflection;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;

namespace Erebor.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterDependenciesByInterface(this IServiceCollection services, Type interfaceType, params Assembly[] assembly)
    {
        var assemblyTypes = assembly.SelectMany(x => x.GetTypes()).ToList();
        assemblyTypes
            .Where(x => x.IsInterface && interfaceType.IsAssignableFrom(x))
            .ToList()
            .ForEach(i => assemblyTypes
                .Where(x => x.IsClass && i.IsAssignableFrom(x))
                .ToList()
                .ForEach(imp => services.AddScoped(i, imp))
            );
        return services;
    }
}