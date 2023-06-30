using AutoMapper;
using Erebor.Application.Models.Requests;
using Erebor.Application.Models.Responses;
using Erebor.Domain.Models;

namespace Erebor.Application.Mappers;

internal class CountryProfile : Profile
{
    public CountryProfile()
    {
        CreateMap<AddCountryRequestModel, Country>()
            .ForMember(x => x.CreatedAt, opt => opt.MapFrom(src => DateTimeOffset.UtcNow));
            ;
        CreateMap<UpdateCountryRequestModel, Country>()
            .ForMember(x => x.UpdatedAt, opt => opt.MapFrom(src => DateTimeOffset.UtcNow));
        CreateMap<Country, CountryResponseModel>();
    }
}
