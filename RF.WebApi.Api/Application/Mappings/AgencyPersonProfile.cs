using AutoMapper;
using RF.WebApi.Api.Application.DTOs.AgencyPerson;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Application.Mappings
{
    public class AgencyPersonProfile : Profile
    {
        public AgencyPersonProfile()
        {
            CreateMap<AgencyPerson, AgencyPersonDto>().ReverseMap();
            CreateMap<CreateAgencyPersonDto, AgencyPerson>();
            CreateMap<UpdateAgencyPersonDto, AgencyPerson>();
        }
    }
}
