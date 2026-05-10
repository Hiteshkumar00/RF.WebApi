namespace RF.WebApi.Api.Infrastructure.Data.Tables
{
    public class PaymentTransfer
    {
        public int? Id { get; set; }
        public int? FromPaymentAccountId { get; set; }
        public int? ToPaymentAccountId { get; set; }
        public decimal? Amount { get; set; }
        public string? Description { get; set; }
        public DateOnly? Date { get; set; }
        public int? AccountId { get; set; }

        public virtual PaymentAccount? FromPaymentAccount { get; set; }
        public virtual PaymentAccount? ToPaymentAccount { get; set; }
    }
}
