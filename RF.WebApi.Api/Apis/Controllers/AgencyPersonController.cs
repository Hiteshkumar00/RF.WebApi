using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RF.WebApi.Api.Application.DTOs.AgencyPerson;
using RF.WebApi.Api.Domain.Interfaces;

namespace RF.WebApi.Api.Apis.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AgencyPersonController : BaseController
    {
        private readonly IAgencyPersonService _agencyPersonService;

        public AgencyPersonController(IAgencyPersonService agencyPersonService)
        {
            _agencyPersonService = agencyPersonService;
        }

        [HttpPost()]
        public async Task<IActionResult> Create(CreateAgencyPersonDto dto)
        {
            var result = await _agencyPersonService.CreateAgencyPerson(dto);
            return HandleResponse(result);
        }

        [HttpGet()]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _agencyPersonService.GetAgencyPersonById(id);
            return HandleResponse(result);
        }

        [HttpPut()]
        public async Task<IActionResult> Update(UpdateAgencyPersonDto dto)
        {
            var result = await _agencyPersonService.UpdateAgencyPerson(dto);
            return HandleResponse(result);
        }

        [HttpDelete()]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _agencyPersonService.DeleteAgencyPerson(id);
            return HandleResponse(result);
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var result = await _agencyPersonService.GetAllAgencyPersons();
            return HandleResponse(result);
        }

        [HttpGet()]
        public async Task<IActionResult> Export()
        {
            var result = await _agencyPersonService.ExportAgencyPersons();
            if (result.Success && result.Data != null)
            {
                return File(result.Data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AgencyPersons.xlsx");
            }
            return HandleResponse(result);
        }

        [HttpPost()]
        public async Task<IActionResult> Import(List<ImportAgencyPersonDto> dtos)
        {
            var result = await _agencyPersonService.ImportAgencyPersons(dtos);
            return HandleResponse(result);
        }
    }
}
