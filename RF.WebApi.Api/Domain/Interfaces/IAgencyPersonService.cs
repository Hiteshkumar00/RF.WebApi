using RF.WebApi.Api.Application.DTOs.AgencyPerson;
using RF.WebApi.Api.Domain.Exceptions;

namespace RF.WebApi.Api.Domain.Interfaces
{
    public interface IAgencyPersonService
    {
        Task<ServiceResponse<int>> CreateAgencyPerson(CreateAgencyPersonDto dto);
        Task<ServiceResponse<AgencyPersonDto>> GetAgencyPersonById(int id);
        Task<ServiceResponse<bool>> UpdateAgencyPerson(UpdateAgencyPersonDto dto);
        Task<ServiceResponse<bool>> DeleteAgencyPerson(int id);
        Task<ServiceResponse<List<AgencyPersonDto>>> GetAllAgencyPersons();
    }
}
