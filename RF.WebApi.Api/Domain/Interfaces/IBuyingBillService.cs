using RF.WebApi.Api.Application.DTOs.BuyingBill;
using RF.WebApi.Api.Domain.Exceptions;

namespace RF.WebApi.Api.Domain.Interfaces
{
    public interface IBuyingBillService
    {
        Task<ServiceResponse<int>> CreateBuyingBill(CreateBuyingBillDto dto);
        Task<ServiceResponse<BuyingBillDto>> GetBuyingBillById(int id);
        Task<ServiceResponse<bool>> UpdateBuyingBill(UpdateBuyingBillDto dto);
        Task<ServiceResponse<bool>> DeleteBuyingBill(int id);
        Task<ServiceResponse<List<BuyingBillListDto>>> GetAllBuyingBills();
        Task<ServiceResponse<List<BuyingBillListDto>>> GetAllBuyingBillsByAgencyId(int agencyId);
    }
}
