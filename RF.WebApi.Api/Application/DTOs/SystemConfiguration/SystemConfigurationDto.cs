namespace RF.WebApi.Api.Application.DTOs.SystemConfiguration
{
    public class SystemConfigurationDto
    {
        public int Id { get; set; }
        public string PropertyName { get; set; } = string.Empty;
        public string PropertyDisplayName { get; set; } = string.Empty;
        public string PropertyType { get; set; } = "string";
        public string PropertyValue { get; set; } = string.Empty;
    }

    public class UpdateSystemConfigurationDto
    {
        public int Id { get; set; }
        public string PropertyValue { get; set; } = string.Empty;
    }
}
