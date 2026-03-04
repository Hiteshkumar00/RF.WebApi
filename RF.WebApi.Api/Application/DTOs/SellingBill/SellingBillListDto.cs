using System;

namespace RF.WebApi.Api.Application.DTOs.SellingBill
{
    public class SellingBillListDto
    {
        public int Id { get; set; }
        public string BillNo { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string PhoneNo { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        
        // Calculated Summaries
        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
    }
}
