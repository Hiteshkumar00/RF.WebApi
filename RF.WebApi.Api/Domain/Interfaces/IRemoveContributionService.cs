using RF.WebApi.Api.Application.DTOs.Contribution;
using RF.WebApi.Api.Domain.Exceptions;

namespace RF.WebApi.Api.Domain.Interfaces
{
    public interface IRemoveContributionService
    {
        Task<ServiceResponse<int>> CreateRemoveContribution(CreateRemoveContributionDto dto);
        Task<ServiceResponse<RemoveContributionDto>> GetRemoveContributionById(int id);
        Task<ServiceResponse<bool>> UpdateRemoveContribution(UpdateRemoveContributionDto dto);
        Task<ServiceResponse<bool>> DeleteRemoveContribution(int id);
        Task<ServiceResponse<List<RemoveContributionListDto>>> GetAllRemoveContributions();
        Task<ServiceResponse<List<RemoveContributionListDto>>> GetAllRemoveContributionsByAccountPersonId(int accountPersonId);
    }
}
