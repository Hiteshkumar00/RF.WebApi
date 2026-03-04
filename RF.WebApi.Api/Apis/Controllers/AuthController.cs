using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RF.WebApi.Api.Application.DTOs.User;
using RF.WebApi.Api.Domain.Interfaces;

namespace RF.WebApi.Api.Apis.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost()]
        public async Task<IActionResult> CreteUser(CreateUserDto createUserDto)
        {
            var result = await _userService.CreateUser(createUserDto);
            return HandleResponse(result);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost()]
        public async Task<IActionResult> LoginAsAdmin(int AccountId)
        {
            var result = await _userService.LoginAsAdmin(AccountId);
            return HandleResponse(result);
        }

        [AllowAnonymous]
        [HttpPost()]
        public async Task<IActionResult> Login(LoginUserDto loginDto)
        {
            var result = await _userService.Login(loginDto);
            return HandleResponse(result);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete()]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUser(id);
            return HandleResponse(result);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost()]
        public async Task<IActionResult> ActivateDeactivateUser(int Id)
        {
            var result = await _userService.ActivateDeactivateUser(Id);
            return HandleResponse(result);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost()]
        public async Task<IActionResult> ResetPasswordBySuperAdmin(ResetPasswordBySuperAdminDto dto)
        {
            var result = await _userService.ResetPasswordBySuperAdmin(dto);
            return HandleResponse(result);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet()]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _userService.GetUserById(id);
            return HandleResponse(result);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPut()]
        public async Task<IActionResult> UpdateUser(UserDto dto)
        {
            var result = await _userService.UpdateUser(dto);
            return HandleResponse(result);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _userService.GetAllUsers();
            return HandleResponse(result);
        }
    }
}
