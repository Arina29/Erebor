using AutoMapper;
using Erebor.Application.Commands.Interfaces;
using Erebor.Application.Models.Requests;
using Erebor.Domain.Models;
using Erebor.Infrastructure.Commands;
using Erebor.Infrastructure.Repositories;
using System.Reactive;
using Erebor.Application.Validations.Interfaces;
using Microsoft.Extensions.Logging;

namespace Erebor.Application.Commands;

internal class UpdateCountryCommand : IUpdateCountryCommand
{
    private readonly IMapper _mapper;
    private readonly IRepository<Country> _repository;
    private readonly ICountryValidator _validator;
    private readonly ILogger<UpdateCountryCommand> _logger;

    public UpdateCountryCommand(
        IMapper mapper, 
        IRepository<Country> repository, 
        ICountryValidator validator,
        ILogger<UpdateCountryCommand> logger)
    {
        _mapper = mapper;
        _repository = repository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<CommandResult<Unit>> ExecuteAsync(UpdateCountryRequestModel model)
    {
        var countryToUpdate = await _repository.GetAsync(x => x.Id == model.Id);
        if (countryToUpdate is null)
        {
            _logger.LogError("Cannot update country with id: {countryId} because it was not found", model.Id);
            return CommandResult.NotFound<Unit>();
        }

        _logger.LogInformation("Updating country with id: {countryId}", model.Id);

        _mapper.Map(model, countryToUpdate);

        var validationResult = await _validator.ValidateAsync(countryToUpdate);
        if (!validationResult.IsValid)
        {
            _logger.LogError("A validation error occured while updating the : {country}", countryToUpdate);
            return CommandResult.ValidationError<Unit>(validationResult.Errors);
        }

        await _repository.UpdateAsync(countryToUpdate);
            
        return CommandResult.Success(Unit.Default);
    }
}