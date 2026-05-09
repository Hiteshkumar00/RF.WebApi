using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RF.WebApi.Api.Domain.Common;

namespace RF.WebApi.Api.Application.DTOs.BusinessExpence
{
    public class UpdateBusinessExpenceDto
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = BusinessExpenceMessages.ExpenceTypeRequired)]
        [StringLength(250)]
        public string ExpenceType { get; set; } = string.Empty;

        [Required(ErrorMessage = BusinessExpenceMessages.DateRequired)]
        public DateOnly Date { get; set; }

        [Required(ErrorMessage = BusinessExpenceMessages.TotalAmountRequired)]
        [Range(0.01, double.MaxValue, ErrorMessage = BusinessExpenceMessages.AmountPositive)]
        public decimal TotalAmount { get; set; }

        public List<UpdateBusinessExpencePaymentDto> Payments { get; set; } = new List<UpdateBusinessExpencePaymentDto>();
    }

    public class UpdateBusinessExpencePaymentDto
    {
        public int? Id { get; set; } // Null if it's a new payment added during update

        [Required(ErrorMessage = BusinessExpenceMessages.AmountRequired)]
        [Range(0.01, double.MaxValue, ErrorMessage = BusinessExpenceMessages.AmountPositive)]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = BusinessExpenceMessages.PaymentAccountRequired)]
        public int PaymentAccountId { get; set; }
    }
}
