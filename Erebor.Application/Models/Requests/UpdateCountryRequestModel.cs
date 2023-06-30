using Erebor.Domain.Enums;

namespace Erebor.Application.Models.Requests;

public record UpdateCountryRequestModel(Guid Id, string Name, Dangerousness Dangerousness, string Description);
