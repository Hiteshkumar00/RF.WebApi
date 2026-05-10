namespace RF.WebApi.Api.Application.DTOs.PaymentAccount
{
    public class PaymentTransferDto
    {
        public int Id { get; set; }
        public int FromPaymentAccountId { get; set; }
        public string? FromPaymentAccountName { get; set; }
        public int ToPaymentAccountId { get; set; }
        public string? ToPaymentAccountName { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public DateOnly Date { get; set; }
    }

    public class CreatePaymentTransferDto
    {
        public int FromPaymentAccountId { get; set; }
        public int ToPaymentAccountId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public DateOnly Date { get; set; }
    }

    public class UpdatePaymentTransferDto : CreatePaymentTransferDto
    {
        public int Id { get; set; }
    }

    public class PaymentTransferFilterDto
    {
        public int? FromPaymentAccountId { get; set; }
        public int? ToPaymentAccountId { get; set; }
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
    }
}
