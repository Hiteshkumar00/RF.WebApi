using AutoMapper;
using RF.WebApi.Api.Application.DTOs.BusinessYear;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Application.Mappings
{
    public class BusinessYearProfile : Profile
    {
        public BusinessYearProfile()
        {
            CreateMap<BusinessYear, BusinessYearDto>().ReverseMap();
            CreateMap<BusinessYear, BusinessYearListDto>().ReverseMap();
            CreateMap<CreateBusinessYearDto, BusinessYear>();
            CreateMap<UpdateBusinessYearDto, BusinessYear>();
        }
    }
}
