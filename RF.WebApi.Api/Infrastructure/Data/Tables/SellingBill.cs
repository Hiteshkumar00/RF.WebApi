using System;
using System.Collections.Generic;

namespace RF.WebApi.Api.Infrastructure.Data.Tables
{
    public class SellingBill
    {
        public int? Id { get; set; } // Adding standard PK
        public int? AccountId { get; set; }
        public string? BillNo { get; set; }
        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public DateOnly? Date { get; set; }

        public ICollection<SellingBillItem> Items { get; set; } = new List<SellingBillItem>();
        public ICollection<SellingBillPayment> Payments { get; set; } = new List<SellingBillPayment>();
    }
}