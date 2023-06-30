using System.Reactive;

namespace Erebor.Infrastructure.Commands;

public interface ICommand {}

public interface ICommand<in TIn, TOut> : ICommand
{
    Task<CommandResult<TOut>> ExecuteAsync(TIn model);
}

public interface ICommand<in TIn> : ICommand<TIn, Unit>
{

}