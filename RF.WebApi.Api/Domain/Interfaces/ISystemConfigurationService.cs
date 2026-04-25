using RF.WebApi.Api.Application.DTOs.SystemConfiguration;
using RF.WebApi.Api.Domain.Exceptions;

namespace RF.WebApi.Api.Domain.Interfaces
{
    public interface ISystemConfigurationService
    {
        Task<ServiceResponse<List<SystemConfigurationDto>>> GetAllConfigurations();
        Task<ServiceResponse<bool>> UpdateConfiguration(UpdateSystemConfigurationDto dto);
        Task<bool> GetConfigurationValueAsBool(string propertyName);
        Task<string> GetConfigurationValueAsString(string propertyName);
    }
}
