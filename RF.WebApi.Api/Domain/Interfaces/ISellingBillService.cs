using RF.WebApi.Api.Application.DTOs.SellingBill;
using RF.WebApi.Api.Application.DTOs.Common;
using RF.WebApi.Api.Domain.Exceptions;

namespace RF.WebApi.Api.Domain.Interfaces
{
    public interface ISellingBillService
    {
        Task<ServiceResponse<int>> CreateSellingBill(CreateSellingBillDto dto);
        Task<ServiceResponse<SellingBillDto>> GetSellingBillById(int id);
        Task<ServiceResponse<bool>> UpdateSellingBill(UpdateSellingBillDto dto);
        Task<ServiceResponse<bool>> DeleteSellingBill(int id);
        Task<ServiceResponse<PagedResult<SellingBillListDto>>> GetAllSellingBills(TableLazyLoadEventDto request, int? customerId = null);
        Task<ServiceResponse<SellingBillStatisticsDto>> GetSellingBillStatistics(int? customerId = null);

        Task<ServiceResponse<byte[]>> GenerateInvoicePdf(int id);
        Task<ServiceResponse<bool>> SendWhatsAppMessage(int id);
        Task<ServiceResponse<bool>> SendEmailMessage(int id);
        Task<ServiceResponse<bool>> BulkSendWhatsAppMessages(List<int> billIds);
        Task<ServiceResponse<bool>> BulkSendEmailMessages(List<int> billIds);
        Task<ServiceResponse<bool>> UpdatePayments(int billId, List<SellingBillPaymentDto> payments);
    }
}
