namespace RF.WebApi.Api.Infrastructure.Data.Tables
{
    public class AddContributionPayment
    {
        public int? Id { get; set; }
        public decimal? Amount { get; set; }
        public int? AddContributionId { get; set; } // Referenced as AddContributione FK in your image
        public int? PaymentAccountId { get; set; }
    }
}
