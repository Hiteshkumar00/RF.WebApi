using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RF.WebApi.Api.Domain.Common;

namespace RF.WebApi.Api.Application.DTOs.SellingBill
{
    public class CreateSellingBillDto
    {
        [StringLength(50)]
        public string? BillNo { get; set; }

        [Required(ErrorMessage = SellingBillMessages.CustomerNameRequired)]
        [StringLength(250)]
        public string CustomerName { get; set; } = string.Empty;

        [StringLength(250)]
        public string? Email { get; set; }

        [StringLength(20)]
        public string? PhoneNo { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }

        [Required(ErrorMessage = SellingBillMessages.DateRequired)]
        public DateOnly Date { get; set; }

        // Nested Collections
        public List<CreateSellingBillItemDto> Items { get; set; } = new List<CreateSellingBillItemDto>();
        public List<CreateSellingBillPaymentDto> Payments { get; set; } = new List<CreateSellingBillPaymentDto>();
    }
}
