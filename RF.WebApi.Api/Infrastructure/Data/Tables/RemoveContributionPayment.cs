namespace RF.WebApi.Api.Infrastructure.Data.Tables
{
    public class RemoveContributionPayment
    {
        public int? Id { get; set; }
        public decimal? Amount { get; set; }
        public int? RemoveContributionId { get; set; } // Referenced as RemoveContributione FK in image
        public int? PaymentAccountId { get; set; }
        public DateOnly? Date { get; set; }

        public RemoveContribution? RemoveContribution { get; set; }
        public PaymentAccount? PaymentAccount { get; set; }
    }
}