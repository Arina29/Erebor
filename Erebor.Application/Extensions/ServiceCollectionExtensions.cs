using System.Reflection;
using Azure;
using Erebor.Application.Commands;
using Erebor.Application.Commands.Interfaces;
using Erebor.Application.Models;
using Erebor.Application.Providers;
using Erebor.Application.Queries;
using Erebor.Application.Queries.Interfaces;
using Erebor.Application.Services;
using Erebor.Application.Services.Interfaces;
using Erebor.Infrastructure.Commands;
using Erebor.Infrastructure.Extensions;
using Erebor.Infrastructure.Queries;
using FluentValidation;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Erebor.Application.Extensions;

public static class ServiceCollectionExtensions
{

    public static IServiceCollection AddApplication(this IServiceCollection services, ConfigurationManager config) =>
        services
            .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
            .AddAzureClients(config)
            .AddLogging()
            .AddHttpClient()
            .Configure<TranslationAzureCredentials>(options =>
                config.GetSection("AiDeveloperProMax:TextTranslationClient").Bind(options))
            .RegisterDependenciesByInterface(typeof(IValidator), Assembly.GetExecutingAssembly())
            .RegisterDependenciesByInterface(typeof(IQuery), Assembly.GetExecutingAssembly())
            .RegisterDependenciesByInterface(typeof(ICommand), Assembly.GetExecutingAssembly())
            .AddScoped(typeof(IEreborApiClient<,>), typeof(EreborApiClient<,>))
            .AddScoped<ITextAnalysisService, TextAnalysisService>()
            .AddScoped<ITextTranslationService, TextTranslationService>()
        ;


    internal record AzureCredential(Uri? Endpoint, string Credential);

    private static IServiceCollection AddAzureClients(this IServiceCollection services, ConfigurationManager config)
    {
        services.AddAzureClients(builder =>
        {
            var aiConfigs = config.GetSection("AiDeveloperProMax");

            var languageAnalysisConfig = aiConfigs.GetSection("LanguageClient").Get<AzureCredential>();
            if (languageAnalysisConfig is null)
                throw new Exception("Cannot register azure TextAnalyticsClient");

            builder.AddTextAnalyticsClient(languageAnalysisConfig.Endpoint, new AzureKeyCredential(languageAnalysisConfig.Credential));

        });

        return services;
    }
}