namespace RF.WebApi.Api.Domain.Common
{
    public static class PaymentAccountMessages
    {
        public const string InvalidId = "Invalid Payment Account Id";
        public const string NotFound = "Payment Account not found";
        
        // Custom required messages for validation
        public const string MethodNameRequired = "Method Name is required";
        public const string AccountPersonIdRequired = "Account Person is required";
    }
}
