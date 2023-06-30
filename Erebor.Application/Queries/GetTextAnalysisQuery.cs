using Erebor.Application.Queries.Interfaces;
using Erebor.Application.Models.Requests;
using Erebor.Application.Services.Interfaces;
using Erebor.Infrastructure.Queries;

namespace Erebor.Application.Queries;

internal class GetTextAnalysisQuery : IGetTextAnalysisQuery
{
    private readonly ITextAnalysisService _textAnalysisService;

    public GetTextAnalysisQuery(ITextAnalysisService textAnalysisService)
    {
        _textAnalysisService = textAnalysisService;
    }

    public async Task<QueryResult<TextAnalysisModel>> ExecuteAsync(string text)
    {
        var result = await _textAnalysisService.BuildTextAnalysis(text);

        return result.AsSuccess();
    }
}