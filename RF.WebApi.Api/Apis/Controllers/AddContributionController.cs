using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RF.WebApi.Api.Application.DTOs.Contribution;
using RF.WebApi.Api.Domain.Interfaces;

namespace RF.WebApi.Api.Apis.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize] // Protected by default
    public class AddContributionController : BaseController
    {
        private readonly IAddContributionService _addContributionService;

        public AddContributionController(IAddContributionService addContributionService)
        {
            _addContributionService = addContributionService;
        }

        [HttpPost()]
        public async Task<IActionResult> Create(CreateAddContributionDto dto)
        {
            var result = await _addContributionService.CreateAddContribution(dto);
            return HandleResponse(result);
        }

        [HttpPut()]
        public async Task<IActionResult> Update(UpdateAddContributionDto dto)
        {
            var result = await _addContributionService.UpdateAddContribution(dto);
            return HandleResponse(result);
        }

        [HttpDelete()]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _addContributionService.DeleteAddContribution(id);
            return HandleResponse(result);
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var result = await _addContributionService.GetAllAddContributions();
            return HandleResponse(result);
        }

        [HttpGet("{accountPersonId}")]
        public async Task<IActionResult> GetAllByAccountPersonId(int accountPersonId)
        {
            var result = await _addContributionService.GetAllAddContributionsByAccountPersonId(accountPersonId);
            return HandleResponse(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _addContributionService.GetAddContributionById(id);
            return HandleResponse(result);
        }
    }
}
