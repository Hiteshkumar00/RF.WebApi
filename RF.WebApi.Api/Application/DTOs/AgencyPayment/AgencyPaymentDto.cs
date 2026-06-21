using System;
using System.Collections.Generic;

namespace RF.WebApi.Api.Application.DTOs.AgencyPayment
{
    public class AgencyPaymentDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int AgencyId { get; set; }
        public string AgencyName { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public string? Description { get; set; }
        public decimal TotalAmount { get; set; }

        public List<AgencyPaymentTransactionDto> Transactions { get; set; } = new List<AgencyPaymentTransactionDto>();
    }

    public class AgencyPaymentTransactionDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int PaymentAccountId { get; set; }
        public string PaymentAccountName { get; set; } = string.Empty;
        public DateOnly? Date { get; set; }
    }
}
