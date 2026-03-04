namespace RF.WebApi.Api.Application.DTOs.PaymentAccount
{
    public class PaymentAccountDto
    {
        public int Id { get; set; }
        
        public string MethodName { get; set; } = string.Empty;
        
        public int? AccountId { get; set; }
        
        public int? AccountPersonId { get; set; }
    }
}
