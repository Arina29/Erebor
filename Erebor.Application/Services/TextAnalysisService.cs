using Azure.AI.TextAnalytics;
using Erebor.Application.Models.Requests;
using Erebor.Application.Services.Interfaces;
using DetectedLanguage = Erebor.Application.Models.Requests.DetectedLanguage;

namespace Erebor.Application.Services;

internal class TextAnalysisService: ITextAnalysisService
{
    private readonly TextAnalyticsClient _client;

    public TextAnalysisService(TextAnalyticsClient client)
    {
        _client = client;
    }

    public async Task<TextAnalysisModel> BuildTextAnalysis(string text) =>
        new(
            await GetKeyPhraseCollection(text),
            await GetLanguage(text),
            await GetSentiment(text),
            await ExtractEntities(text)
        );
    private async Task<TextSentiment> GetSentiment(string text) =>
        (await _client.AnalyzeSentimentAsync(text)).Value.Sentiment;

    private async Task<DetectedLanguage> GetLanguage(string text)
    {
        var detectedLanguage = (await _client.DetectLanguageAsync(text)).Value;
        return new DetectedLanguage(detectedLanguage.Name, detectedLanguage.ConfidenceScore);
    }

    private async Task<IEnumerable<string>> GetKeyPhraseCollection(string text)
    {
        var phrases = await _client.ExtractKeyPhrasesAsync(text);
        return phrases?.Value is null ? Enumerable.Empty<string>() : phrases.Value.Select(x => x);
    }

    private async Task<IEnumerable<DetectedEntity>> ExtractEntities(string text)
    {
        var entities = await _client.RecognizeEntitiesAsync(text);

        return entities?.Value is null
            ? Enumerable.Empty<DetectedEntity>()
            : entities.Value.Select(x =>
                new DetectedEntity(
                    x.Text, x.ConfidenceScore, x.Category, x.SubCategory));
    }
}