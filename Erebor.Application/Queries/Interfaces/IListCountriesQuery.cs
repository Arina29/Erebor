using Erebor.Application.Models.Requests;
using Erebor.Application.Models.Responses;
using Erebor.Infrastructure.Data.BaseModels;
using Erebor.Infrastructure.Queries;

namespace Erebor.Application.Queries.Interfaces;

public interface IListCountriesQuery : IQuery<ListCountriesPagedModel, PagedList<CountryResponseModel>>
{
}