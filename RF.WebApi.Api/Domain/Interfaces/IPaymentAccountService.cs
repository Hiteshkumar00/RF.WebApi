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
        Task<ServiceResponse<List<PaymentHistoryDto>>> GetPaymentHistory(PaymentHistoryFilterDto filter);

        // Payment Transfer
        Task<ServiceResponse<int>> CreatePaymentTransfer(CreatePaymentTransferDto dto);
        Task<ServiceResponse<bool>> UpdatePaymentTransfer(UpdatePaymentTransferDto dto);
        Task<ServiceResponse<bool>> DeletePaymentTransfer(int id);
        Task<ServiceResponse<List<PaymentTransferDto>>> GetPaymentTransfers(PaymentTransferFilterDto filter);
        Task<ServiceResponse<PaymentTransferDto>> GetTransferById(int id);
    }
}
