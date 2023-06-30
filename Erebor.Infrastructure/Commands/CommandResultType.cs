namespace Erebor.Infrastructure.Commands;

public enum CommandResultType
{
    Success,
    Created,
    ValidationError,
    NotFound,
    InvalidOperation,
}