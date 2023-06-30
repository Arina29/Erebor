using AutoMapper;
using Erebor.Application.Models.Enums;
using Erebor.Application.Models.Requests;
using Erebor.Application.Models.Responses;
using Erebor.Application.Queries.Interfaces;
using Erebor.Domain.Models;
using Erebor.Infrastructure.Data.BaseModels;
using Erebor.Infrastructure.Queries;
using Erebor.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;

namespace Erebor.Application.Queries;

internal class ListCountriesQuery : IListCountriesQuery
{
    private readonly IRepository<Country> _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<ListCountriesQuery> _logger;

    public ListCountriesQuery(
        IRepository<Country> repository, 
        IMapper mapper, 
        ILogger<ListCountriesQuery> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<QueryResult<PagedList<CountryResponseModel>>> ExecuteAsync(ListCountriesPagedModel model)
    {

        var countries = await _repository.GetPagedAsync(
            model.PageSize, 
            model.Page,
            x => GetOrderExpression(model.OrderBy, x)
        );

        var response = new PagedList<CountryResponseModel>
        {
            Page = model.Page,
            PageSize = model.PageSize,
            Data = _mapper.Map<List<Country>, List<CountryResponseModel>>(countries.Data),
            PageCount = countries.PageCount,
            TotalCount = countries.TotalCount,
        };

        return response.AsSuccess();
    }

    private IOrderedQueryable<Country> GetOrderExpression(CountryOrderBy orderByOption,
        IQueryable<Country> countries) =>
        orderByOption switch
        {
            CountryOrderBy.NameAscending => countries.OrderBy(y => y.Name),
            CountryOrderBy.NameDescending => countries.OrderByDescending(x => x.Name),
            CountryOrderBy.DangerousnessAscending => countries.OrderBy(x => x.Dangerousness),
            CountryOrderBy.DangerousnessDescending => countries.OrderByDescending(x => x.Dangerousness),
            CountryOrderBy.DescriptionAscending => countries.OrderBy(x => x.Description),
            CountryOrderBy.DescriptionDescending => countries.OrderByDescending(x => x.Description),
            _ => countries.OrderByDescending(x => x.Name),
        };


}