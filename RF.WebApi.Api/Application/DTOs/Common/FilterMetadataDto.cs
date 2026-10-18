namespace RF.WebApi.Api.Application.DTOs.Common
{
    public class FilterMetadataDto
    {
        public object? Value { get; set; }
        public string? MatchMode { get; set; } // contains, startsWith, equals, gt, lt, etc.
        public string? Operator { get; set; } = "and"; // and / or
    }
}
