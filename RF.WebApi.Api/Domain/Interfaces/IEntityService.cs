using RF.WebApi.Api.Application.DTOs.Entity;
using RF.WebApi.Api.Domain.Common;
using RF.WebApi.Api.Domain.Exceptions;

namespace RF.WebApi.Api.Domain.Interfaces
{
    public interface IEntityService
    {
        Task<ServiceResponse<int>> CreateEntity(CreateEntityDto dto);
        Task<ServiceResponse<EntityDto>> GetEntityById(int id);
        Task<ServiceResponse<EntityDto>> GetEntityByName(string entityName);
        Task<ServiceResponse<bool>> UpdateEntity(UpdateEntityDto dto);
        Task<ServiceResponse<bool>> DeleteEntity(int id);
        Task<ServiceResponse<List<EntityDto>>> GetAllEntities();
    }
}
