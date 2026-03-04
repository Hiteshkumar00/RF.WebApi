using System;
using System.Collections.Generic;

namespace RF.WebApi.Api.Infrastructure.Data.Tables
{
    public class SellingBill
    {
        public int? Id { get; set; } // Adding standard PK
        public int? AccountId { get; set; }
        public string? BillNo { get; set; }
        public string? CustomerName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNo { get; set; }
        public string? Address { get; set; }
        public DateOnly? Date { get; set; }
        public decimal? Discount { get; set; }

        public ICollection<SellingBillItem> Items { get; set; } = new List<SellingBillItem>();
        public ICollection<SellingBillPayment> Payments { get; set; } = new List<SellingBillPayment>();
    }
}