using Erebor.Domain.Enums;

namespace Erebor.Application.Models.Requests;

public record AddCountryRequestModel(
    string Name, 
    Dangerousness Dangerousness, 
    string Description);