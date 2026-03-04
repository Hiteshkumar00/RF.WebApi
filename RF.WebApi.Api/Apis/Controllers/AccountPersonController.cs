using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RF.WebApi.Api.Application.DTOs.AccountPerson;
using RF.WebApi.Api.Domain.Interfaces;

namespace RF.WebApi.Api.Apis.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountPersonController : BaseController
    {
        private readonly IAccountPersonService _accountPersonService;

        public AccountPersonController(IAccountPersonService accountPersonService)
        {
            _accountPersonService = accountPersonService;
        }

        [HttpPost()]
        public async Task<IActionResult> Create(CreateAccountPersonDto dto)
        {
            var result = await _accountPersonService.CreateAccountPerson(dto);
            return HandleResponse(result);
        }

        [HttpGet()]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _accountPersonService.GetAccountPersonById(id);
            return HandleResponse(result);
        }

        [HttpPut()]
        public async Task<IActionResult> Update(UpdateAccountPersonDto dto)
        {
            var result = await _accountPersonService.UpdateAccountPerson(dto);
            return HandleResponse(result);
        }

        [HttpDelete()]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _accountPersonService.DeleteAccountPerson(id);
            return HandleResponse(result);
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var result = await _accountPersonService.GetAllAccountPersons();
            return HandleResponse(result);
        }
    }
}
