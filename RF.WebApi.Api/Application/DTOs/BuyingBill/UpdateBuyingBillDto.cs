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

        [StringLength(100)]
        public string BillNo { get; set; } = string.Empty;

        [Required(ErrorMessage = BuyingBillMessages.DateRequired)]
        public DateOnly Date { get; set; }

        public List<UpdateStockDto> Stocks { get; set; } = new List<UpdateStockDto>();
        public List<UpdateBuyingBillPaymentDto> Payments { get; set; } = new List<UpdateBuyingBillPaymentDto>();
        public List<UpdateBuyingBillExpenceDto> Expences { get; set; } = new List<UpdateBuyingBillExpenceDto>();
    }
}
