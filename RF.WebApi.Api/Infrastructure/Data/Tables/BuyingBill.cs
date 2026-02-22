
namespace RF.WebApi.Api.Infrastructure.Data.Tables
{
    public class BuyingBill
    {
        public int? Id { get; set; }
        public int? AccountId { get; set; }
        public int? AgencyId { get; set; }
        public string? BillNo { get; set; }
        public DateOnly? Date { get; set; } 
        public decimal? Discount { get; set; }
    }
}