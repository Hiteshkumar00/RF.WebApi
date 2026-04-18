using RF.WebApi.Api.Domain.Exceptions;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Domain.Interfaces
{
    public interface IEmailIntegrationService
    {
        Task<ServiceResponse<bool>> SendBillAsync(SellingBill bill, Account account);
    }
}
