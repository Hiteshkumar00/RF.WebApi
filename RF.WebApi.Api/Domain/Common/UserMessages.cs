namespace RF.WebApi.Api.Domain.Common
{
    public static class UserMessages
    {
        // Validation & Errors
        public const string InvalidId = "Invalid User Id";
        public const string NotFound = "User not found";
        public const string EmailExists = "User with email '{0}' already exists.";
        public const string UnauthorizedAction = "You don't have permission to perform this action";
        public const string InvalidSession = "Invalid Session";
        public const string InvalidRole = "Invalid User Role";
        public const string EmailRequired = "Email is required";
        public const string PasswordRequired = "Password is required";
        public const string NewPasswordRequired = "New Password is required";
        public const string UserIdRequired = "User is required";
        public const string FirstNameRequired = "First Name is required";
        public const string SurnameRequired = "Surname is required";
        public const string RoleRequired = "Role is required";
        public const string IsActiveRequired = "IsActive is required";
        public const string AccountRequired = "Account is required";

        // Auth & Password
        public const string InvalidLogin = "Invalid Email or Password";
        public const string Deactivated = "Your account is deactivated | Contact SuperAdmin";
        public const string SelfDeleteProhibited = "You cannot delete your own account";
        public const string SelfDeactivateProhibited = "You cannot deactivate your own account";
        public const string JwtKeyError = "JWT Key must be at least 64 characters long for HS512.";

        // Account Switching
        public const string OnlySuperAdminSwitch = "Only SuperAdmins can switch accounts.";
        public const string TargetAccountNotFound = "Target account not found.";

        // Jwt
        public const string SessionExpired = "Your session has expired. Please login again.";
    }
}
