using Erebor.Domain.Models;
using Erebor.Infrastructure.Controllers;
using Microsoft.AspNetCore.Mvc;
using Erebor.Application.Models.Requests;
using Erebor.Application.Queries.Interfaces;

namespace Erebor.WebApi.Controllers;

[Route("TextAnalysis")]
public class TextAnalysisController : EreborController
{
    private readonly IGetTextAnalysisQuery _getTextAnalysisQuery;
    private readonly ITextTranslationQuery _textTranslationQuery;

    public TextAnalysisController(IGetTextAnalysisQuery getTextAnalysisQuery, ITextTranslationQuery textTranslationQuery)
    {
        _getTextAnalysisQuery = getTextAnalysisQuery;
        _textTranslationQuery = textTranslationQuery;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string text)
    {
        var result = await _getTextAnalysisQuery.ExecuteAsync(text);

        return ProcessResult<TextAnalysisModel>(result);
    }

    [HttpGet]
    [Route("translation")]
    public async Task<IActionResult> GetTranslation(string text)
    {
        var result = await _textTranslationQuery.ExecuteAsync(text);

        return ProcessResult<string>(result);
    }


}