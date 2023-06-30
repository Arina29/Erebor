using Erebor.Application.Models.Requests;
using Erebor.Infrastructure.Commands;

namespace Erebor.Application.Commands.Interfaces;

public interface IUpdateCountryCommand: ICommand<UpdateCountryRequestModel>
{
}