using AutoMapper;
using Erebor.Application.Commands.Interfaces;
using Erebor.Application.Models.Requests;
using Erebor.Application.Models.Responses;
using Erebor.Application.Validations.Interfaces;
using Erebor.Domain.Models;
using Erebor.Infrastructure.Commands;
using Erebor.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;

namespace Erebor.Application.Commands;

internal class AddCountryCommand : IAddCountryCommand
{
    private readonly IMapper _mapper;
    private readonly IRepository<Country> _repository;
    private readonly ILogger<AddCountryCommand> _logger;
    private readonly ICountryValidator _validator;

    public AddCountryCommand(
        IMapper mapper, 
        IRepository<Country> repository, 
        ILogger<AddCountryCommand> logger,
        ICountryValidator validator)
    {
        _mapper = mapper;
        _repository = repository;
        _logger = logger;
        _validator = validator;
    }

    public async Task<CommandResult<Guid>> ExecuteAsync(AddCountryRequestModel model)
    {
        var countryToAdd = _mapper.Map<Country>(model);

        var validationResult = await _validator.ValidateAsync(countryToAdd);
        if (!validationResult.IsValid)
        {
            _logger.LogError("A validation error occured while adding the country: {country}", countryToAdd);
            return CommandResult.ValidationError<Guid>(validationResult.Errors);
        }

        _logger.LogInformation("Adding new country: {country}", countryToAdd);

        var result = await _repository.AddAsync(countryToAdd);

        return result.Id == Guid.Empty 
            ? CommandResult.InvalidOperation<Guid>() 
            : CommandResult.Created(result.Id);
    }
}