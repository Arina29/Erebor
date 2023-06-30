using Azure.AI.TextAnalytics;

namespace Erebor.Application.Models.Requests;

public record TextAnalysisModel(
    IEnumerable<string> DetectedKeyPhrases,
    DetectedLanguage Language,
    TextSentiment Sentiment,
    IEnumerable<DetectedEntity> DetectedEntities);

public record DetectedLanguage(string Language, double Confidence);

public record DetectedEntity(string Text, double Confidence, EntityCategory Category, string SubCategory);