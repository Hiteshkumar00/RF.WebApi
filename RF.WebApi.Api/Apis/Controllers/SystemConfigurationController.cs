using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RF.WebApi.Api.Application.DTOs.SystemConfiguration;
using RF.WebApi.Api.Domain.Interfaces;

namespace RF.WebApi.Api.Apis.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SystemConfigurationController : BaseController
    {
        private readonly ISystemConfigurationService _configService;

        public SystemConfigurationController(ISystemConfigurationService configService)
        {
            _configService = configService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var result = await _configService.GetAllConfigurations();
            return HandleResponse(result);
        }

        [HttpPut()]
        public async Task<IActionResult> Update(UpdateSystemConfigurationDto dto)
        {
            var result = await _configService.UpdateConfiguration(dto);
            return HandleResponse(result);
        }
    }
}
