using System.Reactive;
using Erebor.Infrastructure.Commands;

namespace Erebor.Application.Commands.Interfaces;

public interface IDeleteCountryCommand: ICommand<Guid>
{
}