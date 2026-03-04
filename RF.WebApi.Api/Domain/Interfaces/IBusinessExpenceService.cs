using RF.WebApi.Api.Application.DTOs.BusinessExpence;
using RF.WebApi.Api.Domain.Exceptions;

namespace RF.WebApi.Api.Domain.Interfaces
{
    public interface IBusinessExpenceService
    {
        Task<ServiceResponse<int>> CreateBusinessExpence(CreateBusinessExpenceDto dto);
        Task<ServiceResponse<BusinessExpenceDto>> GetBusinessExpenceById(int id);
        Task<ServiceResponse<bool>> UpdateBusinessExpence(UpdateBusinessExpenceDto dto);
        Task<ServiceResponse<bool>> DeleteBusinessExpence(int id);
        Task<ServiceResponse<List<BusinessExpenceDto>>> GetAllBusinessExpences();
    }
}
