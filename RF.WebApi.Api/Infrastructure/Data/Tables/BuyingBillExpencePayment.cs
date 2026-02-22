namespace RF.WebApi.Api.Infrastructure.Data.Tables
{
    public class BuyingBillExpencePayment
    {
        public int? Id { get; set; }
        public decimal? Amount { get; set; }
        public int? BuyingBillExpenceId { get; set; } // Referenced as BusinessExpence in your image
        public int? PaymentAccountId { get; set; }
    }
}
