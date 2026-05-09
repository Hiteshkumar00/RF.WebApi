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

        [AllowAnonymous]
        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var result = await _configService.GetAllConfigurations();
            return HandleResponse(result);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPut()]
        public async Task<IActionResult> UpdateMultiple(List<UpdateSystemConfigurationDto> dtos)
        {
            var result = await _configService.UpdateMultipleConfigurations(dtos);
            return HandleResponse(result);
        }
    }
}
