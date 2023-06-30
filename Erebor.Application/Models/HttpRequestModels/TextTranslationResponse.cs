using System.Text.Json.Serialization;

namespace Erebor.Application.Models.HttpRequestModels;

internal class TranslationModel
{
    [JsonPropertyName("translations")]
    public List<Translation> Translations { get; set; } = new List<Translation>();
}

internal class Translation
{
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;

    [JsonPropertyName("to")]
    public string To { get; set; } = string.Empty;

}