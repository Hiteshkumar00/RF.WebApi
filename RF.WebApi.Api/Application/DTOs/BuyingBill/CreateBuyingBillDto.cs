using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RF.WebApi.Api.Domain.Common;

namespace RF.WebApi.Api.Application.DTOs.BuyingBill
{
    public class CreateBuyingBillDto
    {
        [StringLength(50)]
        public string? BillNo { get; set; }

        [Required(ErrorMessage = BuyingBillMessages.AgencyIdRequired)]
        public int AgencyId { get; set; }

        [Required(ErrorMessage = BuyingBillMessages.DateRequired)]
        public DateOnly Date { get; set; }

        public List<CreateStockDto> Stocks { get; set; } = new List<CreateStockDto>();
        public List<CreateBuyingBillPaymentDto> Payments { get; set; } = new List<CreateBuyingBillPaymentDto>();
        public List<CreateBuyingBillExpenceDto> Expences { get; set; } = new List<CreateBuyingBillExpenceDto>();
    }
}
