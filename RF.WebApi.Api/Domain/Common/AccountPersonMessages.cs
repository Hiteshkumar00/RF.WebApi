namespace RF.WebApi.Api.Domain.Common
{
    public static class AccountPersonMessages
    {
        // Validation & Errors
        public const string InvalidId = "Invalid Account Person Id";
        public const string NotFound = "Account Person not found";
        public const string NameRequired = "Name is required";
        public const string InUseInPaymentAccount = "Cannot delete because it is used in payment account: {0}";
    }
}
