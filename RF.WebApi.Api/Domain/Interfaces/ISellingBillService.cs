using RF.WebApi.Api.Application.DTOs.SellingBill;
using RF.WebApi.Api.Domain.Exceptions;

namespace RF.WebApi.Api.Domain.Interfaces
{
    public interface ISellingBillService
    {
        Task<ServiceResponse<int>> CreateSellingBill(CreateSellingBillDto dto);
        Task<ServiceResponse<SellingBillDto>> GetSellingBillById(int id);
        Task<ServiceResponse<bool>> UpdateSellingBill(UpdateSellingBillDto dto);
        Task<ServiceResponse<bool>> DeleteSellingBill(int id);
        Task<ServiceResponse<List<SellingBillListDto>>> GetAllSellingBills();
    }
}
