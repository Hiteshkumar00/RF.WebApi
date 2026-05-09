namespace RF.WebApi.Api.Infrastructure.Data.Tables
{
    public class BusinessExpencePayment
    {
        public int? Id { get; set; }
        public decimal? Amount { get; set; }
        public int? BusinessExpenceId { get; set; }
        public int? PaymentAccountId { get; set; }
        public DateOnly? Date { get; set; }
    }
}