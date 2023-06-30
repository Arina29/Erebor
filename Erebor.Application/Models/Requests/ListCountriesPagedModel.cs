
using Erebor.Application.Models.Enums;

namespace Erebor.Application.Models.Requests;

public record ListCountriesPagedModel(int PageSize, int Page, CountryOrderBy OrderBy);
