using System.Reactive;
using Erebor.Application.Commands.Interfaces;
using Erebor.Application.Models.Responses;
using Erebor.Domain.Models;
using Erebor.Infrastructure.Commands;
using Erebor.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;

namespace Erebor.Application.Commands;

internal class DeleteCountryCommand : IDeleteCountryCommand
{
    private readonly IRepository<Country> _repository;
    private readonly ILogger<DeleteCountryCommand> _logger;

    public DeleteCountryCommand(IRepository<Country> repository, ILogger<DeleteCountryCommand> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<CommandResult<Unit>> ExecuteAsync(Guid id)
    {
        var countryToDelete = await _repository.GetAsync(x => x.Id == id);

        if (countryToDelete != null)
        {
            _logger.LogInformation("Deleting country with id : {countryId}", id);
            await _repository.DeleteAsync(countryToDelete);
        }

        return CommandResult.Success(Unit.Default);
    }
}