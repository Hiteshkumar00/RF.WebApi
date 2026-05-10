namespace RF.WebApi.Api.Domain.Common
{
    public static class PaymentAccountMessages
    {
        public const string InvalidId = "Invalid Payment Account Id";
        public const string NotFound = "Payment Account not found";
        
        // Custom required messages for validation
        public const string MethodNameRequired = "Method Name is required";
        public const string AccountPersonIdRequired = "Account Person is required";

        public const string InUseInSellingBill = "Cannot delete because it is used in selling bill payments";
        public const string InUseInBuyingBill = "Cannot delete because it is used in buying bill payments or expenses";
        public const string InUseInExpense = "Cannot delete because it is used in business expense payments";
        public const string InUseInContribution = "Cannot delete because it is used in contribution payments";
        public const string InUseInTransfer = "Cannot delete because it is used in payment transfers";
    }
}
