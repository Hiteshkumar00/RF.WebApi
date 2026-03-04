using RF.WebApi.Api.Application.DTOs.AccountPerson;
using RF.WebApi.Api.Domain.Exceptions;

namespace RF.WebApi.Api.Domain.Interfaces
{
    public interface IAccountPersonService
    {
        Task<ServiceResponse<int>> CreateAccountPerson(CreateAccountPersonDto dto);
        Task<ServiceResponse<AccountPersonDto>> GetAccountPersonById(int id);
        Task<ServiceResponse<bool>> UpdateAccountPerson(UpdateAccountPersonDto dto);
        Task<ServiceResponse<bool>> DeleteAccountPerson(int id);
        Task<ServiceResponse<List<AccountPersonDto>>> GetAllAccountPersons();
    }
}
