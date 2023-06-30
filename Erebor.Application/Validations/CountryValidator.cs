using Erebor.Application.Validations.Interfaces;
using Erebor.Domain.Models;
using FluentValidation;

namespace Erebor.Application.Validations;
internal class CountryValidator : AbstractValidator<Country>, ICountryValidator
{
    public CountryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(20);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(100);

        RuleFor(x => x.Dangerousness)
            .NotNull();
    }
}
