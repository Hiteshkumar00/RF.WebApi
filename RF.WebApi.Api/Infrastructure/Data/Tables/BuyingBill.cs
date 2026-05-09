using System;
using System.Collections.Generic;

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

        public Agency? Agency { get; set; }

        public ICollection<BuyingBillItem> Items { get; set; } = new List<BuyingBillItem>();
        public ICollection<BuyingBillPayment> Payments { get; set; } = new List<BuyingBillPayment>();
        public ICollection<BusinessExpence> Expences { get; set; } = new List<BusinessExpence>();
    }
}