using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RF.WebApi.Api.Domain.Common;

namespace RF.WebApi.Api.Application.DTOs.BuyingBill
{
    public class CreateBuyingBillDto
    {
        [Required(ErrorMessage = BuyingBillMessages.AgencyIdRequired)]
        public int AgencyId { get; set; }

        [Required(ErrorMessage = BuyingBillMessages.BillNoRequired)]
        [StringLength(100)]
        public string BillNo { get; set; } = string.Empty;

        [Required(ErrorMessage = BuyingBillMessages.DateRequired)]
        public DateOnly Date { get; set; }

        public decimal Discount { get; set; }

        public List<CreateBuyingBillItemDto> Items { get; set; } = new List<CreateBuyingBillItemDto>();
        public List<CreateBuyingBillPaymentDto> Payments { get; set; } = new List<CreateBuyingBillPaymentDto>();
        public List<CreateBuyingBillExpenceDto> Expences { get; set; } = new List<CreateBuyingBillExpenceDto>();
    }
}
