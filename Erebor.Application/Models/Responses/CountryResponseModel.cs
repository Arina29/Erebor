using Erebor.Domain.Enums;

namespace Erebor.Application.Models.Responses;

public record CountryResponseModel(Guid Id, string Name, Dangerousness Dangerousness, string Description);