using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RF.WebApi.Api.Application.DTOs.AgencyPayment
{
    public class UpdateAgencyPaymentDto
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Agency is required")]
        public int AgencyId { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateOnly Date { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        public List<UpdateAgencyPaymentTransactionDto> Transactions { get; set; } = new List<UpdateAgencyPaymentTransactionDto>();
    }

    public class UpdateAgencyPaymentTransactionDto
    {
        public int? Id { get; set; } // Null if added during update

        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Payment Account is required")]
        public int PaymentAccountId { get; set; }

        public DateOnly? Date { get; set; }
    }
}
