
using Erebor.Application.Extensions;

namespace Erebor.Application.Models;

internal record TranslationAzureCredentials(Uri? Endpoint, string Credential, string Region) :
    ServiceCollectionExtensions.AzureCredential(Endpoint, Credential)
{
    public TranslationAzureCredentials() : this(default, string.Empty, string.Empty)
    {
    }
};