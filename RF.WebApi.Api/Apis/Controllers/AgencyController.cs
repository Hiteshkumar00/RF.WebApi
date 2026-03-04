using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RF.WebApi.Api.Application.DTOs.Agency;
using RF.WebApi.Api.Domain.Interfaces;

namespace RF.WebApi.Api.Apis.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AgencyController : BaseController
    {
        private readonly IAgencyService _agencyService;

        public AgencyController(IAgencyService agencyService)
        {
            _agencyService = agencyService;
        }

        [HttpPost()]
        public async Task<IActionResult> Create(CreateAgencyDto dto)
        {
            var result = await _agencyService.CreateAgency(dto);
            return HandleResponse(result);
        }

        [HttpGet()]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _agencyService.GetAgencyById(id);
            return HandleResponse(result);
        }

        [HttpPut()]
        public async Task<IActionResult> Update(UpdateAgencyDto dto)
        {
            var result = await _agencyService.UpdateAgency(dto);
            return HandleResponse(result);
        }

        [HttpDelete()]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _agencyService.DeleteAgency(id);
            return HandleResponse(result);
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var result = await _agencyService.GetAllAgencies();
            return HandleResponse(result);
        }
    }
}
