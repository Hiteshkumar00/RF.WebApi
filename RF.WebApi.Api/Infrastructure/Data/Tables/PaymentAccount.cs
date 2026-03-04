namespace RF.WebApi.Api.Infrastructure.Data.Tables
{
    public class PaymentAccount
    {
        public int? Id { get; set; }
        public string? MethodName { get; set; }
        public int? AccountId { get; set; }
        public int? AccountPersonId { get; set; } 
    }
}