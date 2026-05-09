namespace RF.WebApi.Api.Application.DTOs.BusinessExpence
{
    public class BusinessExpencePaymentDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int BusinessExpenceId { get; set; }
        public int PaymentAccountId { get; set; }
        public string? PaymentAccountName { get; set; }
        public DateOnly? Date { get; set; }
    }
}
