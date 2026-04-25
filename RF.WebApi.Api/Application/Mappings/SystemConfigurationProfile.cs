using AutoMapper;
using RF.WebApi.Api.Application.DTOs.SystemConfiguration;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Application.Mappings
{
    public class SystemConfigurationProfile : Profile
    {
        public SystemConfigurationProfile()
        {
            CreateMap<SystemConfiguration, SystemConfigurationDto>();
            CreateMap<UpdateSystemConfigurationDto, SystemConfiguration>();
        }
    }
}
