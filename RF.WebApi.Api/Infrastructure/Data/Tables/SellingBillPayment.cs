namespace RF.WebApi.Api.Infrastructure.Data.Tables
{
    public class SellingBillPayment
    {
        public int? Id { get; set; }
        public decimal? Amount { get; set; }
        public int? BillId { get; set; }
        public int? PaymentAccountId { get; set; }
        public DateOnly? Date { get; set; }

        public SellingBill? Bill { get; set; }
    }
}