using RF.WebApi.Api.Application.DTOs.User;
using RF.WebApi.Api.Domain.Exceptions;

namespace RF.WebApi.Api.Domain.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResponse<int>> CreateUser(CreateUserDto createUserDto);

        Task<ServiceResponse<string>> LoginAsAdmin(int AccountId);

        Task<ServiceResponse<string>> Login(LoginUserDto loginDto);

        Task<ServiceResponse<bool>> DeleteUser(int Id);

        Task<ServiceResponse<bool>> ActivateDeactivateUser(int Id);

        Task<ServiceResponse<bool>> ResetPasswordBySuperAdmin(ResetPasswordBySuperAdminDto dto);

        Task<ServiceResponse<UserDto>> GetUserById(int Id);

        Task<ServiceResponse<bool>> UpdateUser(UserDto dto);

        Task<ServiceResponse<List<UserDto>>> GetAllUsers();
    }
}
