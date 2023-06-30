using Erebor.Domain.Models;
using FluentValidation;

namespace Erebor.Application.Validations.Interfaces;
internal interface ICountryValidator : IValidator<Country>
{
}
