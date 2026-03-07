using RF.WebApi.Api.Application.DTOs.Contribution;
using RF.WebApi.Api.Domain.Exceptions;

namespace RF.WebApi.Api.Domain.Interfaces
{
    public interface IAddContributionService
    {
        Task<ServiceResponse<int>> CreateAddContribution(CreateAddContributionDto dto);
        Task<ServiceResponse<AddContributionDto>> GetAddContributionById(int id);
        Task<ServiceResponse<bool>> UpdateAddContribution(UpdateAddContributionDto dto);
        Task<ServiceResponse<bool>> DeleteAddContribution(int id);
        Task<ServiceResponse<List<AddContributionListDto>>> GetAllAddContributions();
        Task<ServiceResponse<List<AddContributionListDto>>> GetAllAddContributionsByAccountPersonId(int accountPersonId);
    }
}
