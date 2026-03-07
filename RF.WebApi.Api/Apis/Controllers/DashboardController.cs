using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RF.WebApi.Api.Domain.Interfaces;

namespace RF.WebApi.Api.Apis.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DashboardController : BaseController
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _dashboardService.GetDashboardMetricsAsync();
            return HandleResponse(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetPaymentAccountDashboard()
        {
            var result = await _dashboardService.GetPaymentAccountDashboardMetricsAsync();
            return HandleResponse(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTimeDashboard()
        {
            var result = await _dashboardService.GetAllTimeDashboardMetricsAsync();
            return HandleResponse(result);
        }
    }
}
