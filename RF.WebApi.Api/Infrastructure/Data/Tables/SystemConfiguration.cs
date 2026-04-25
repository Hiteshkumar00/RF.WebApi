namespace RF.WebApi.Api.Infrastructure.Data.Tables
{
    public class SystemConfiguration
    {
        public int Id { get; set; }
        public string PropertyName { get; set; } = string.Empty;
        public string PropertyDisplayName { get; set; } = string.Empty;
        public string PropertyType { get; set; } = "string"; // string, boolean, number
        public string PropertyValue { get; set; } = string.Empty;
    }
}
