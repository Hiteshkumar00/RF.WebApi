using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RF.WebApi.Api.Application.DTOs.BusinessYear;
using RF.WebApi.Api.Domain.Interfaces;

namespace RF.WebApi.Api.Apis.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BusinessYearController : BaseController
    {
        private readonly IBusinessYearService _businessYearService;

        public BusinessYearController(IBusinessYearService businessYearService)
        {
            _businessYearService = businessYearService;
        }

        [HttpPost()]
        public async Task<IActionResult> Create(CreateBusinessYearDto dto)
        {
            var result = await _businessYearService.CreateBusinessYear(dto);
            return HandleResponse(result);
        }

        [HttpPut()]
        public async Task<IActionResult> Update(UpdateBusinessYearDto dto)
        {
            var result = await _businessYearService.UpdateBusinessYear(dto);
            return HandleResponse(result);
        }

        [HttpDelete()]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _businessYearService.DeleteBusinessYear(id);
            return HandleResponse(result);
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var result = await _businessYearService.GetAllBusinessYears();
            return HandleResponse(result);
        }

        [HttpPost()]
        public async Task<IActionResult> ChangeSelectedYear(ChangeUserSelectedYearDto dto)
        {
            var result = await _businessYearService.ChangeSelectedYear(dto);
            return HandleResponse(result);
        }
    }
}
