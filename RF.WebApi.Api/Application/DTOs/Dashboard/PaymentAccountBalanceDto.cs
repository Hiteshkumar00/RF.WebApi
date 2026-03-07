namespace RF.WebApi.Api.Application.DTOs.Dashboard
{
    public class PaymentAccountBalanceDto
    {
        public int PaymentAccountId { get; set; }
        public string PaymentAccountName { get; set; } = string.Empty;
        public decimal Balance { get; set; }
    }
}
