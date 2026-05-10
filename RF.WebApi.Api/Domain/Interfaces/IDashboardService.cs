using RF.WebApi.Api.Application.DTOs.Dashboard;
using RF.WebApi.Api.Domain.Exceptions;

namespace RF.WebApi.Api.Domain.Interfaces
{
    public interface IDashboardService
    {
        Task<ServiceResponse<DashboardDto>> GetDashboardMetricsAsync();
        Task<ServiceResponse<PaymentAccountDashboardDto>> GetPaymentAccountDashboardMetricsAsync();
        Task<ServiceResponse<List<AllTimeDashboardItemDto>>> GetAllTimeDashboardMetricsAsync();
        Task<ServiceResponse<ProductDashboardDto>> GetProductProfitMetricsAsync();
    }
}
