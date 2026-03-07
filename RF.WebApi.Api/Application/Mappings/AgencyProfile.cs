using AutoMapper;
using RF.WebApi.Api.Application.DTOs.Agency;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Application.Mappings
{
    public class AgencyProfile : Profile
    {
        public AgencyProfile()
        {
            CreateMap<Agency, AgencyDto>().ReverseMap();
            CreateMap<Agency, AgencyAdvancedListDto>().ReverseMap();
            CreateMap<Agency, ViewAgencyAllDetailDto>().ReverseMap();
            CreateMap<CreateAgencyDto, Agency>();
            CreateMap<UpdateAgencyDto, Agency>();
        }
    }
}
