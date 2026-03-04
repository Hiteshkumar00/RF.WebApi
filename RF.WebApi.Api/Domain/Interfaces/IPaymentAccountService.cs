using RF.WebApi.Api.Application.DTOs.PaymentAccount;
using RF.WebApi.Api.Domain.Exceptions;

namespace RF.WebApi.Api.Domain.Interfaces
{
    public interface IPaymentAccountService
    {
        Task<ServiceResponse<int>> CreatePaymentAccount(CreatePaymentAccountDto dto);
        Task<ServiceResponse<PaymentAccountDto>> GetPaymentAccountById(int id);
        Task<ServiceResponse<bool>> UpdatePaymentAccount(UpdatePaymentAccountDto dto);
        Task<ServiceResponse<bool>> DeletePaymentAccount(int id);
        Task<ServiceResponse<List<PaymentAccountDto>>> GetAllPaymentAccounts();
    }
}
