using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RF.WebApi.Api.Application.DTOs.AgencyPayment
{
    public class CreateAgencyPaymentDto
    {
        [Required(ErrorMessage = "Agency is required")]
        public int AgencyId { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateOnly Date { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        public List<CreateAgencyPaymentTransactionDto> Transactions { get; set; } = new List<CreateAgencyPaymentTransactionDto>();
    }

    public class CreateAgencyPaymentTransactionDto
    {
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Payment Account is required")]
        public int PaymentAccountId { get; set; }

        public DateOnly? Date { get; set; }
    }
}
