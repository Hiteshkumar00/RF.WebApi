using RF.WebApi.Api.Infrastructure.Data.Tables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RF.WebApi.Api.Domain.Interfaces
{
    public interface IPdfService
    {
        Task<byte[]> GenerateSellingBillPdf(SellingBill bill, Account account);
        Task<byte[]> GenerateBuyingBillPdf(BuyingBill bill, Account account, IEnumerable<BusinessExpence> expences);
    }
}
