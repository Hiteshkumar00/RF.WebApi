using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RF.WebApi.Api.Application.DTOs.Entity;
using RF.WebApi.Api.Domain.Interfaces;

namespace RF.WebApi.Api.Apis.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EntityController : BaseController
    {
        private readonly IEntityService _entityService;

        public EntityController(IEntityService entityService)
        {
            _entityService = entityService;
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost()]
        public async Task<IActionResult> Create(CreateEntityDto dto)
        {
            var result = await _entityService.CreateEntity(dto);
            return HandleResponse(result);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPut()]
        public async Task<IActionResult> Update(UpdateEntityDto dto)
        {
            var result = await _entityService.UpdateEntity(dto);
            return HandleResponse(result);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete()]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _entityService.DeleteEntity(id);
            return HandleResponse(result);
        }

        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var result = await _entityService.GetAllEntities();
            return HandleResponse(result);
        }

        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _entityService.GetEntityById(id);
            return HandleResponse(result);
        }

        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> GetByEntityName(string entityName)
        {
            var result = await _entityService.GetEntityByName(entityName);
            return HandleResponse(result);
        }
    }
}
