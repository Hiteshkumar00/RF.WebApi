namespace RF.WebApi.Api.Infrastructure.Data.Tables
{
    public class BuyingBillExpence
    {
        public int? Id { get; set; }
        public decimal? Amount { get; set; }
        public int? BillId { get; set; } // Referenced as SellingBill FK in image, but logically links to BuyingBill context here
        public int? PaymentAccountId { get; set; }
        public string? ExpenceType { get; set; }
    }
}