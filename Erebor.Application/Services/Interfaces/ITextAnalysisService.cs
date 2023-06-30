using Azure.AI.TextAnalytics;
using Erebor.Application.Models.Requests;
using DetectedLanguage = Erebor.Application.Models.Requests.DetectedLanguage;


namespace Erebor.Application.Services.Interfaces;

internal interface ITextAnalysisService
{
    Task<TextAnalysisModel> BuildTextAnalysis(string text);
}