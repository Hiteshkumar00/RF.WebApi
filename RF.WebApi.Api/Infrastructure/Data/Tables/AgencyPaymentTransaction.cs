using System;

namespace RF.WebApi.Api.Infrastructure.Data.Tables
{
    public class AgencyPaymentTransaction
    {
        public int? Id { get; set; }
        public int? AgencyPaymentId { get; set; }
        public DateOnly? Date { get; set; }
        public decimal? Amount { get; set; }
        public int? PaymentAccountId { get; set; }

        public AgencyPayment? AgencyPayment { get; set; }
        public PaymentAccount? PaymentAccount { get; set; }
    }
}
