namespace RF.WebApi.Api.Domain.Common
{
    public struct ContributionMessages
    {
        public const string NotFound = "Contribution not found.";
        public const string InvalidPayment = "Invalid payment data.";
        public const string CreationFailed = "Failed to create contribution.";
        public const string UpdateFailed = "Failed to update contribution.";
        public const string DeletionFailed = "Failed to delete contribution.";
        public const string UnauthorizedAccess = "You do not have permission to access this contribution.";
    }
}
