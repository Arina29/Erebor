using Erebor.Infrastructure.Commands;
using Erebor.Infrastructure.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Erebor.Infrastructure.Controllers;

[ApiController]
public abstract class EreborController: ControllerBase
{
    protected internal IActionResult ProcessResult<T>(CommandResult<T> result, string location = "") => (result.Result) switch
    {
        CommandResultType.Created => Created(location, new Result<T?>(result.Payload)),
        CommandResultType.Success => Ok(new Result<T?>(result.Payload)),
        CommandResultType.InvalidOperation or CommandResultType.ValidationError => BadRequest(new Result<T?>(result.Error)),
        CommandResultType.NotFound => NotFound(new Result<T>("Not Found")),
        _ => StatusCode(StatusCodes.Status500InternalServerError)
    };

    protected internal IActionResult ProcessResult<T>(QueryResult<T> result) => (result.Result) switch
    {
        QueryResultType.Success => Ok(new {Result = result.Payload}),
        QueryResultType.InvalidOperation => BadRequest(),
        QueryResultType.NotFound => NotFound(),
        _ => StatusCode(StatusCodes.Status500InternalServerError)
    };

}

public class Result<T>
{
    public T? Data { get; set; }

    public string? Error { get; set; }

    public Result(T? data)
    {
        this.Data = data;
        this.Error = null;
    }

    public Result(string? error)
    {
        this.Data = default;
        this.Error = error;
    }

}