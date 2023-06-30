using System.Diagnostics;
using Erebor.Application.Commands.Interfaces;
using Erebor.Application.Models.Requests;
using Erebor.Application.Queries.Interfaces;
using Erebor.Infrastructure.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Erebor.WebApi.Controllers;

[Route("country")]
[Authorize]
[ApiVersion("1.0")]
public class CountryController : EreborController
{
    private readonly IAddCountryCommand _addCountryCommand;
    private readonly IUpdateCountryCommand _updateCountryCommand;
    private readonly IDeleteCountryCommand _deleteCountryCommand;
    private readonly IListCountriesQuery _listCountriesQuery;

    public CountryController(
        IAddCountryCommand addCountryCommand,
        IUpdateCountryCommand updateCountryCommand,
        IDeleteCountryCommand deleteCountryCommand,
        IListCountriesQuery listCountriesQuery)
    {
        _addCountryCommand = addCountryCommand;
        _updateCountryCommand = updateCountryCommand;
        _deleteCountryCommand = deleteCountryCommand;
        _listCountriesQuery = listCountriesQuery;
    }


    [HttpPost("add")]
    public async Task<IActionResult> AddCountry(AddCountryRequestModel model)
    {
        var result = await _addCountryCommand.ExecuteAsync(model);
        return ProcessResult(result);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateCountry(UpdateCountryRequestModel model)
    {
        var result = await _updateCountryCommand.ExecuteAsync(model);
        return ProcessResult(result);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> UpdateCountry(Guid id)
    {
        var result = await _deleteCountryCommand.ExecuteAsync(id);
        return ProcessResult(result);
    }

    [HttpGet("list")]
    public async Task<IActionResult> ListCountries([FromQuery] ListCountriesPagedModel model)
    {
        var result = await _listCountriesQuery.ExecuteAsync(model);
        return ProcessResult(result);
    }
}