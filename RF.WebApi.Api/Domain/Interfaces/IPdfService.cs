using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Domain.Interfaces
{
    public interface IPdfService
    {
        byte[] GenerateSellingBillPdf(SellingBill bill, Account account);
        byte[] GenerateBuyingBillPdf(BuyingBill bill, Account account, IEnumerable<BusinessExpence> expences);
    }
}
