using RF.WebApi.Api.Application.DTOs.Account;
using RF.WebApi.Api.Domain.Exceptions;

namespace RF.WebApi.Api.Domain.Interfaces
{
    public interface IAccountService
    {
        Task<ServiceResponse<int>> CreateAccount(CreateAccountDto createAccountDto);
        Task<ServiceResponse<AccountDto>> GetAccountById(int id);
        Task<ServiceResponse<bool>> UpdateAccount(UpdateAccountDto updateAccountDto);
        Task<ServiceResponse<bool>> DeleteAccount(int id);
        Task<ServiceResponse<List<AccountDto>>> GetAllAccounts();
    }
}
