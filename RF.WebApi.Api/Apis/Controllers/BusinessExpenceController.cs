using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RF.WebApi.Api.Application.DTOs.BusinessExpence;
using RF.WebApi.Api.Domain.Interfaces;

namespace RF.WebApi.Api.Apis.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BusinessExpenceController : BaseController
    {
        private readonly IBusinessExpenceService _businessExpenceService;

        public BusinessExpenceController(IBusinessExpenceService businessExpenceService)
        {
            _businessExpenceService = businessExpenceService;
        }

        [HttpPost()]
        public async Task<IActionResult> Create(CreateBusinessExpenceDto dto)
        {
            var result = await _businessExpenceService.CreateBusinessExpence(dto);
            return HandleResponse(result);
        }

        [HttpGet()]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _businessExpenceService.GetBusinessExpenceById(id);
            return HandleResponse(result);
        }

        [HttpPut()]
        public async Task<IActionResult> Update(UpdateBusinessExpenceDto dto)
        {
            var result = await _businessExpenceService.UpdateBusinessExpence(dto);
            return HandleResponse(result);
        }

        [HttpDelete()]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _businessExpenceService.DeleteBusinessExpence(id);
            return HandleResponse(result);
        }

        /// <summary>
        /// Retrieves Business Expences dynamically filtered by the Currently Selected Business Year.
        /// Start Date = Selected Year's Date
        /// End Date = Next chronological year's date minus 1 day (or Today if no next year exists).
        /// </summary>
        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var result = await _businessExpenceService.GetAllBusinessExpences();
            return HandleResponse(result);
        }
    }
}
