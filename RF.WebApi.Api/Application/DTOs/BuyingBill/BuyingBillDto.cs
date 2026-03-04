using System;
using System.Collections.Generic;

namespace RF.WebApi.Api.Application.DTOs.BuyingBill
{
    public class BuyingBillDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int AgencyId { get; set; }
        public string BillNo { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public decimal Discount { get; set; }

        public List<BuyingBillItemDto> Items { get; set; } = new List<BuyingBillItemDto>();
        public List<BuyingBillPaymentDto> Payments { get; set; } = new List<BuyingBillPaymentDto>();
        public List<BuyingBillExpenceDto> Expences { get; set; } = new List<BuyingBillExpenceDto>();
    }
}
