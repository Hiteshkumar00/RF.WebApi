using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RF.WebApi.Api.Application.DTOs.Account;
using RF.WebApi.Api.Domain.Interfaces;

namespace RF.WebApi.Api.Apis.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost()]
        public async Task<IActionResult> CreateAccount(CreateAccountDto createAccountDto)
        {
            var result = await _accountService.CreateAccount(createAccountDto);
            return HandleResponse(result);
        }

        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> GetAccountById(int id)
        {
            var result = await _accountService.GetAccountById(id);
            return HandleResponse(result);
        }

        [Authorize]
        [HttpPut()]
        public async Task<IActionResult> UpdateAccount(UpdateAccountDto updateAccountDto)
        {
            var result = await _accountService.UpdateAccount(updateAccountDto);
            return HandleResponse(result);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete()]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var result = await _accountService.DeleteAccount(id);
            return HandleResponse(result);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _accountService.GetAllAccounts();
            return HandleResponse(result);
        }
    }
}
