using System;
using System.Collections.Generic;

namespace RF.WebApi.Api.Infrastructure.Data.Tables
{
    public class AgencyPayment
    {
        public int? Id { get; set; }
        public int? AccountId { get; set; }
        public int? AgencyId { get; set; }
        public DateOnly? Date { get; set; }
        public string? Description { get; set; }

        public Agency? Agency { get; set; }
        public ICollection<AgencyPaymentTransaction> Transactions { get; set; } = new List<AgencyPaymentTransaction>();
    }
}
