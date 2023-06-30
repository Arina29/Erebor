
namespace Erebor.Application.Services.Interfaces;

internal interface ITextTranslationService
{
    Task<string> Translate(string text, string sourceLanguage, string toLanguage);
}