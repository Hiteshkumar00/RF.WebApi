namespace RF.WebApi.Api.Domain.Common
{
    public static class SellingBillMessages
    {
        public const string InvalidId = "Invalid Selling Bill Id";
        public const string NotFound = "Selling Bill not found";

        public const string BillNoRequired = "Bill Number is required";
        public const string DateRequired = "Date is required";
        public const string CustomerNameRequired = "Customer Name is required";
        public const string PhoneNoRequired = "Phone Number is required";

        public const string ItemNameRequired = "Item Name is required";
        public const string QuantityPositive = "Quantity must be greater than zero";
        public const string PricePositive = "Price must be greater than zero";

        public const string AmountRequired = "Amount is required";
        public const string AmountPositive = "Amount must be greater than zero";
        
        public const string PaymentAccountRequired = "Payment Account is required";

        public const string WarrantyYearPositive = "Warranty Year must be non-negative";
        public const string WarrantyMonthPositive = "Warranty Month must be non-negative";
        public const string WarrantyDayPositive = "Warranty Day must be non-negative";
    }
}
