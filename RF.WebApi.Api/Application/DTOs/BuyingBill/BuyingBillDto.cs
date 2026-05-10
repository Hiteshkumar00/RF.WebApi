using System;
using System.Collections.Generic;

namespace RF.WebApi.Api.Application.DTOs.BuyingBill
{
    public class BuyingBillDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int AgencyId { get; set; }
        public string AgencyName { get; set; } = string.Empty;
        public string BillNo { get; set; } = string.Empty;
        public DateOnly Date { get; set; }

        public List<StockDto> Stocks { get; set; } = new List<StockDto>();
        public List<BuyingBillPaymentDto> Payments { get; set; } = new List<BuyingBillPaymentDto>();
        public List<BuyingBillExpenceDto> Expences { get; set; } = new List<BuyingBillExpenceDto>();

        // Rollups
        public decimal TotalAmount { get; set; }
        public decimal TotalExpence { get; set; }
        public decimal NetAmount { get; set; }
    }
}
