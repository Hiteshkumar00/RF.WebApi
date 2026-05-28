using RF.WebApi.Api.Application.DTOs.Agency;
using RF.WebApi.Api.Domain.Exceptions;

namespace RF.WebApi.Api.Domain.Interfaces
{
    public interface IAgencyService
    {
        Task<ServiceResponse<int>> CreateAgency(CreateAgencyDto dto);
        Task<ServiceResponse<AgencyDto>> GetAgencyById(int id);
        Task<ServiceResponse<bool>> UpdateAgency(UpdateAgencyDto dto);
        Task<ServiceResponse<bool>> DeleteAgency(int id);
        Task<ServiceResponse<List<AgencyDto>>> GetAllAgencies();
        Task<ServiceResponse<List<AgencyAdvancedListDto>>> GetAllAgencysAdvancedAsync();
        Task<ServiceResponse<ViewAgencyAllDetailDto>> GetAgencyAllDetailAsync(int agencyId);
        Task<ServiceResponse<bool>> PayOldestBillsAsync(PayAgencyOldestBillsDto dto);
    }
}
