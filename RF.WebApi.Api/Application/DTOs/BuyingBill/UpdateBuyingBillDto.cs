using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RF.WebApi.Api.Domain.Common;

namespace RF.WebApi.Api.Application.DTOs.BuyingBill
{
    public class UpdateBuyingBillDto
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = BuyingBillMessages.AgencyIdRequired)]
        public int AgencyId { get; set; }

        [Required(ErrorMessage = BuyingBillMessages.BillNoRequired)]
        [StringLength(100)]
        public string BillNo { get; set; } = string.Empty;

        [Required(ErrorMessage = BuyingBillMessages.DateRequired)]
        public DateOnly Date { get; set; }

        public decimal Discount { get; set; }

        public List<UpdateBuyingBillItemDto> Items { get; set; } = new List<UpdateBuyingBillItemDto>();
        public List<UpdateBuyingBillPaymentDto> Payments { get; set; } = new List<UpdateBuyingBillPaymentDto>();
        public List<UpdateBuyingBillExpenceDto> Expences { get; set; } = new List<UpdateBuyingBillExpenceDto>();
    }
}
