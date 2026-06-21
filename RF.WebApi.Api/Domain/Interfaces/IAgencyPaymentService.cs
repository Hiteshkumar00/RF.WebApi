using RF.WebApi.Api.Application.DTOs.AgencyPayment;
using RF.WebApi.Api.Domain.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RF.WebApi.Api.Domain.Interfaces
{
    public interface IAgencyPaymentService
    {
        Task<ServiceResponse<int>> CreateAgencyPayment(CreateAgencyPaymentDto dto);
        Task<ServiceResponse<AgencyPaymentDto>> GetAgencyPaymentById(int id);
        Task<ServiceResponse<bool>> UpdateAgencyPayment(UpdateAgencyPaymentDto dto);
        Task<ServiceResponse<bool>> DeleteAgencyPayment(int id);
        Task<ServiceResponse<List<AgencyPaymentListDto>>> GetAllAgencyPayments();
    }
}
