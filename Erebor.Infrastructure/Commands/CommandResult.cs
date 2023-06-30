
using FluentValidation.Results;

namespace Erebor.Infrastructure.Commands;

public class CommandResult<T>
{
    public T? Payload { get; set; }

    public CommandResultType Result { get; set; }

    public string? Error { get; set; }

    public bool IsSuccess => string.IsNullOrWhiteSpace(Error);

    public CommandResult(T? payload, CommandResultType result, string? error)
    {
        Payload = payload;
        Result = result;
        Error = error;
    }
}

public static class CommandResult
{
    public static CommandResult<T> AsSuccess<T>(this T payload) =>
        ToCommandResult(payload, CommandResultType.Success);

    public static CommandResult<T> AsCreated<T>(this T payload) =>
        ToCommandResult(payload, CommandResultType.Created);

    public static CommandResult<T> ValidationError<T>(string? error = "") =>
        ToCommandResult(default(T), CommandResultType.ValidationError, error);

    public static CommandResult<T> ValidationError<T>(IEnumerable<ValidationFailure> errors) =>
        ToCommandResult(
            default(T), 
            CommandResultType.ValidationError, 
            string.Join("\n", errors.Select(x => $"{x.PropertyName}: {x.ErrorMessage}")));

    public static CommandResult<T> NotFound<T>() =>
        ToCommandResult(default(T), CommandResultType.NotFound, null);

    public static CommandResult<T> InvalidOperation<T>() =>
        ToCommandResult(default(T), CommandResultType.InvalidOperation);

    public static CommandResult<T> Success<T>(T? payload) =>
        ToCommandResult(payload, CommandResultType.Success);

    public static CommandResult<T> Created<T>(T? payload) =>
        ToCommandResult(payload, CommandResultType.Success);


    private static CommandResult<T> ToCommandResult<T>(T? payload, CommandResultType result, string? errors = null)
    {
        return new CommandResult<T>(payload, result, errors);
    }
}