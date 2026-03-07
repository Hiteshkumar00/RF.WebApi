using RF.WebApi.Api.Application.DTOs.BusinessYear;
using RF.WebApi.Api.Domain.Exceptions;

namespace RF.WebApi.Api.Domain.Interfaces
{
    public interface IBusinessYearService
    {
        Task<ServiceResponse<int>> CreateBusinessYear(CreateBusinessYearDto dto);
        Task<ServiceResponse<bool>> UpdateBusinessYear(UpdateBusinessYearDto dto);
        Task<ServiceResponse<bool>> DeleteBusinessYear(int id);
        Task<ServiceResponse<List<BusinessYearListDto>>> GetAllBusinessYears();
        Task<ServiceResponse<bool>> ChangeSelectedYear(ChangeUserSelectedYearDto dto);
        Task<ServiceResponse<(DateOnly StartDate, DateOnly EndDate)>> GetSelectedBusinessYearDates();
        Task<ServiceResponse<List<BusinessYearDateRangeDto>>> GetAllBusinessYearDateRanges();
    }
}
