namespace RF.WebApi.Api.Domain.Common
{
    public static class BusinessExpenceMessages
    {
        // Business Expence
        public const string InvalidId = "Invalid Business Expence Id";
        public const string NotFound = "Business Expence not found";
        public const string ExpenceTypeRequired = "Expence Type is required";
        public const string DateRequired = "Date is required";
        public const string TotalAmountRequired = "Total Amount is required";

        // Business Expence Payment
        public const string PaymentInvalidId = "Invalid Business Expence Payment Id";
        public const string PaymentNotFound = "Business Expence Payment not found";
        public const string AmountRequired = "Amount is required";
        public const string AmountPositive = "Amount must be greater than zero";
        public const string PaymentAccountRequired = "Payment Account is required";
        public const string BusinessExpenceRequired = "Business Expence is required";
    }
}
