using Erebor.Application.Queries.Interfaces;
using Erebor.Application.Services.Interfaces;
using Erebor.Infrastructure.Queries;


namespace Erebor.Application.Queries;

internal class TextTranslationQuery: ITextTranslationQuery
{
    private readonly ITextTranslationService _textTranslationService;

    public TextTranslationQuery(ITextTranslationService textTranslationService)
    {
        _textTranslationService = textTranslationService;
    }

    public async Task<QueryResult<string>> ExecuteAsync(string model)
    {
        var result = await _textTranslationService.Translate(model, "ro", "en");
        return result.AsSuccess();
    }
}