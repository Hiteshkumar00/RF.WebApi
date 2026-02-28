using System.Security.Claims;

namespace RF.WebApi.Api.Apis.Authentication
{
    public static class Token
    {
        private static IHttpContextAccessor _httpContextAccessor;
        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static int UserId =>
            int.TryParse(_httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier), out var id)
            ? id : 0;

        public static int AccountId =>
            int.TryParse(_httpContextAccessor?.HttpContext?.User.FindFirst("AccountId")?.Value, out var id)
            ? id : 0;

        public static bool IsSuperAdmin =>
            _httpContextAccessor?.HttpContext?.User.IsInRole("SuperAdmin") ?? false;
    }
}
