using System;
using System.Linq;

namespace RF.WebApi.Api.Application.DTOs.BuyingBill
{
    public class BuyingBillListDto
    {
        public int Id { get; set; }
        public int AgencyId { get; set; }
        public string AgencyName { get; set; } = string.Empty;
        public string BillNo { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        
        // Calculated Summaries
        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal TotalExpence { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
    }
}
