using System;

namespace RF.WebApi.Api.Application.DTOs.PaymentAccount
{
    public class PaymentHistoryDto
    {
        public int Id { get; set; }
        public string PaymentAccountName { get; set; } = string.Empty;
        public string? AccountPersonName { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Direction { get; set; } = string.Empty; // "Received" or "Paid"
        public decimal Amount { get; set; }
        public DateOnly Date { get; set; }
        public string PaymentType { get; set; } = string.Empty;
        public string? BillNo { get; set; }
    }

    public class PaymentHistoryFilterDto
    {
        public int? PaymentAccountId { get; set; }
        public string? Direction { get; set; } // "Received", "Paid"
        public string? PaymentType { get; set; }
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
    }
}
