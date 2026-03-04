using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RF.WebApi.Api.Domain.Common;

namespace RF.WebApi.Api.Application.DTOs.SellingBill
{
    public class UpdateSellingBillDto
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = SellingBillMessages.BillNoRequired)]
        [StringLength(100)]
        public string BillNo { get; set; } = string.Empty;

        [Required(ErrorMessage = SellingBillMessages.CustomerNameRequired)]
        [StringLength(250)]
        public string CustomerName { get; set; } = string.Empty;

        [StringLength(250)]
        public string? Email { get; set; }

        [Required(ErrorMessage = SellingBillMessages.PhoneNoRequired)]
        [StringLength(20)]
        public string PhoneNo { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Address { get; set; }

        [Required(ErrorMessage = SellingBillMessages.DateRequired)]
        public DateOnly Date { get; set; }

        public decimal Discount { get; set; }

        // Nested Collections
        public List<UpdateSellingBillItemDto> Items { get; set; } = new List<UpdateSellingBillItemDto>();
        public List<UpdateSellingBillPaymentDto> Payments { get; set; } = new List<UpdateSellingBillPaymentDto>();
    }
}
