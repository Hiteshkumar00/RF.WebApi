using System;
using System.Collections.Generic;

namespace RF.WebApi.Api.Application.DTOs.SellingBill
{
    public class SellingBillDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string BillNo { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string PhoneNo { get; set; } = string.Empty;
        public string? Address { get; set; }
        public DateOnly Date { get; set; }

        // Nested Collections
        public List<SellingBillItemDto> Items { get; set; } = new List<SellingBillItemDto>();
        public List<SellingBillPaymentDto> Payments { get; set; } = new List<SellingBillPaymentDto>();
    }
}
