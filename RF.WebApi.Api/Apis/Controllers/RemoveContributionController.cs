using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RF.WebApi.Api.Application.DTOs.Contribution;
using RF.WebApi.Api.Domain.Interfaces;

namespace RF.WebApi.Api.Apis.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize] // Protected by default
    public class RemoveContributionController : BaseController
    {
        private readonly IRemoveContributionService _removeContributionService;

        public RemoveContributionController(IRemoveContributionService removeContributionService)
        {
            _removeContributionService = removeContributionService;
        }

        [HttpPost()]
        public async Task<IActionResult> Create(CreateRemoveContributionDto dto)
        {
            var result = await _removeContributionService.CreateRemoveContribution(dto);
            return HandleResponse(result);
        }

        [HttpPut()]
        public async Task<IActionResult> Update(UpdateRemoveContributionDto dto)
        {
            var result = await _removeContributionService.UpdateRemoveContribution(dto);
            return HandleResponse(result);
        }

        [HttpDelete()]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _removeContributionService.DeleteRemoveContribution(id);
            return HandleResponse(result);
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var result = await _removeContributionService.GetAllRemoveContributions();
            return HandleResponse(result);
        }

        [HttpGet("{accountPersonId}")]
        public async Task<IActionResult> GetAllByAccountPersonId(int accountPersonId)
        {
            var result = await _removeContributionService.GetAllRemoveContributionsByAccountPersonId(accountPersonId);
            return HandleResponse(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _removeContributionService.GetRemoveContributionById(id);
            return HandleResponse(result);
        }
    }
}
