namespace RF.WebApi.Api.Domain.Common
{
    public static class BuyingBillMessages
    {
        public const string InvalidId = "Invalid Buying Bill Id";
        public const string NotFound = "Buying Bill not found";

        public const string AgencyIdRequired = "Agency is required";
        public const string BillNoRequired = "Bill Number is required";
        public const string DateRequired = "Date is required";

        public const string ItemNameRequired = "Item Name is required";
        public const string QuantityPositive = "Quantity must be greater than zero";
        public const string PricePositive = "Price must be greater than zero";

        public const string AmountRequired = "Amount is required";
        public const string AmountPositive = "Amount must be greater than zero";
        
        public const string PaymentAccountRequired = "Payment Account is required";
        public const string ExpenceTypeRequired = "Expense Type is required";
    }
}
