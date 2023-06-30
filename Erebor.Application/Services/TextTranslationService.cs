using System.Text;
using Erebor.Application.Services.Interfaces;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Options;
using Erebor.Application.Models;
using Microsoft.Extensions.Logging;
using Erebor.Application.Providers;
using Erebor.Application.Models.HttpRequestModels;

namespace Erebor.Application.Services;

internal class TextTranslationService : ITextTranslationService
{
    private readonly TranslationAzureCredentials _config;
    private readonly IEreborApiClient<List<TranslationModel>, object[]> _client;
    private readonly ILogger<TextTranslationService> _logger;

    public TextTranslationService(
        IEreborApiClient<List<TranslationModel>, object[]> client,
        IOptions<TranslationAzureCredentials> config,
        ILogger<TextTranslationService> logger)
    {
        _config = config.Value;
        _client = client;
        _logger = logger;
    }

    public async Task<string> Translate(string text, string sourceLanguage, string toLanguage)
    {
        object[] body = { new { Text = text } };
        var headers = new Dictionary<string, string>
        {
            {"Ocp-Apim-Subscription-Key", _config.Credential},
            {"Ocp-Apim-Subscription-Region", _config.Region}
        };
        var uri = _config.Endpoint + "/translate?api-version=3.0&from=" + sourceLanguage + "&to=" + toLanguage;
        var response = await _client.PerformPostRequest(body, uri, headers);

        return response.Match(
            result => result.First().Translations.First().Text,
            error => error
        );
    }

}